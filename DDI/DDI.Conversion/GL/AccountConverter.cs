using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;

namespace DDI.Conversion.GL
{    
    internal class AccountConverter : ConversionBase
    {
        public enum ConversionMethod
        {
            Segments = 70100,
            AccountGroups = 701001,
            Accounts = 701002,
            LedgerAccounts = 701003,
            LedgerAccountYears = 701004,
            LedgerAccountMerges = 701005,
            AccountPriorYears = 701006,
        }

        private string _glDirectory;
        private string _outputDirectory;
        private Dictionary<int, Guid> _ledgerIds;
        private Dictionary<string, Guid> _fiscalYearIds;
        private Dictionary<int, Guid> _accountGroupIds;
        private Dictionary<string, Guid> _segmentIds;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _glDirectory = Path.Combine(baseDirectory, DirectoryName.GL);
            _outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.GL);
            _ledgerIds = new Dictionary<int, Guid>();
            _fiscalYearIds = new Dictionary<string, Guid>();
            _accountGroupIds = new Dictionary<int, Guid>();
            _segmentIds = new Dictionary<string, Guid>();

            // Make sure the IS Payload directory exists.
            Directory.CreateDirectory(_outputDirectory);

            RunConversion(ConversionMethod.Segments, () => ConvertSegments(InputFile.GL_Segments));
            RunConversion(ConversionMethod.AccountGroups, () => ConvertAccountGroups(InputFile.GL_AccountGroups));
            RunConversion(ConversionMethod.Accounts, () => ConvertAccounts(InputFile.GL_Accounts));
            RunConversion(ConversionMethod.LedgerAccounts, () => ConvertLedgerAccounts(InputFile.GL_LedgerAccounts));
        }


        private void ConvertSegments(string filename)
        {
            DomainContext context = new DomainContext();

            LoadLedgerIds();
            LoadFiscalYearIds();
            var levels = LoadEntities(context.GL_SegmentLevels);
            var segments = new Dictionary<string, Segment>();
            var segmentLinks = new Dictionary<string, string>();

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    Guid? ledgerId;
                    Guid? fiscalYearID = GetFiscalYearId(importer, 0, out ledgerId);

                    if (fiscalYearID == null)
                    {
                        continue;
                    }

                    Segment segment = new Segment();
                    segment.FiscalYearId = fiscalYearID;
                    segment.Level = importer.GetInt(2);
                    segment.Code = importer.GetString(3, 30);
                    segment.Name = importer.GetString(4, 128);
                    string legacyKey = importer.GetString(5);
                    string parentKey = importer.GetString(6);

                    ImportCreatedModifiedInfo(segment, importer, 7);

                    SegmentLevel segmentLevel = levels.FirstOrDefault(p => p.LedgerId == ledgerId && p.Level == segment.Level);
                    if (segmentLevel == null)
                    {
                        importer.LogError($"Segment \"{segment.Code}\" has invalid level {segment.Level}.");
                        continue;
                    }

                    segment.SegmentLevelId = segmentLevel.Id;

                    if (!string.IsNullOrWhiteSpace(parentKey))
                    {
                        segmentLinks.Add(legacyKey, parentKey);
                    }

                    segment.AssignPrimaryKey();
                    segments.Add(legacyKey, segment);

                    count++;
                }

                // Link segments to parent segments
                foreach (var entry in segmentLinks)
                {
                    Segment parent = segments[entry.Value];
                    Segment child = segments[entry.Key];
                    child.ParentSegmentId = parent.Id;
                }

                // Dump segments
                var outputFile = new FileExport<Segment>(Path.Combine(_outputDirectory, OutputFile.GL_SegmentFile), false);
                var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, OutputFile.SegmentIdMappingFile), false, true);

                outputFile.AddHeaderRow();

                foreach (var entry in segments)
                {
                    outputFile.AddRow(entry.Value);
                    legacyIdFile.AddRow(new LegacyToID(entry.Key, entry.Value.Id));
                }

                legacyIdFile.Dispose();
                outputFile.Dispose();
            }
        }

        private void ConvertAccountGroups(string filename)
        {
            DomainContext context = new DomainContext();

            LoadFiscalYearIds();
            var groupLinks = new Dictionary<int, int>();
            var groups = new Dictionary<int, AccountGroup>();

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    Guid? fiscalYearID = GetFiscalYearId(importer, 0);

                    if (fiscalYearID == null)
                    {
                        continue;
                    }

                    int legacyKey = importer.GetInt(2);
                    if (legacyKey == 0)
                    {
                        continue;
                    }

                    AccountGroup group = new AccountGroup();
                    group.FiscalYearId = fiscalYearID;
                    group.Name = importer.GetString(3, 128);
                    group.Sequence = importer.GetInt(4);
                    int parentKey = importer.GetInt(5);
                    group.Category = importer.GetEnum<AccountCategory>(6);
                    ImportCreatedModifiedInfo(group, importer, 7);

                    if (parentKey > 0)
                    {
                        groupLinks.Add(legacyKey, parentKey);
                    }


                    group.AssignPrimaryKey();
                    groups.Add(legacyKey, group);

                    count++;
                }

                // Link groups to parent groups
                foreach (var entry in groupLinks)
                {
                    AccountGroup parent = groups[entry.Value];
                    AccountGroup child = groups[entry.Key];
                    child.ParentGroupId = parent.Id;
                }

                // Dump account groups
                var outputFile = new FileExport<AccountGroup>(Path.Combine(_outputDirectory, OutputFile.GL_AccountGroupFile), false);
                var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, OutputFile.AccountGroupIdMappingFile), false, true);

                outputFile.AddHeaderRow();

                foreach (var entry in groups)
                {
                    outputFile.AddRow(entry.Value);
                    legacyIdFile.AddRow(new LegacyToID(entry.Key, entry.Value.Id));
                }

                legacyIdFile.Dispose();
                outputFile.Dispose();
            }
        }

        private void ConvertAccounts(string filename)
        {
            DomainContext context = new DomainContext();

            LoadFiscalYearIds();
            LoadAccountGroupIds();
            LoadSegmentIds();
            var accounts = new Dictionary<string, Account>();
            var closingAccounts = new Dictionary<string, string>();

            var outputFile = new FileExport<Account>(Path.Combine(_outputDirectory, OutputFile.GL_AccountFile), false);
            var segmentOutputFile = new FileExport<AccountSegment>(Path.Combine(_outputDirectory, OutputFile.GL_AccountSegmentFile), false);
            var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, OutputFile.AccountIdMappingFile), false, true);

            outputFile.AddHeaderRow();
            segmentOutputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;
                Guid? prevFiscalYearId = null;
                Guid? fiscalYearId = null;

                while (true)
                {
                    bool notEof = importer.GetNextRow();

                    if (notEof)
                    {
                        fiscalYearId = GetFiscalYearId(importer, 0);

                        if (fiscalYearId == null)
                        {
                            continue;
                        }
                    }

                    if (!notEof || fiscalYearId != prevFiscalYearId)
                    {
                        // Breaking on fiscal year or EOF

                        // Link accounts to closing accounts
                        foreach (var entry in closingAccounts)
                        {
                            Account child = accounts[entry.Value];
                            Account parent = accounts[entry.Key];
                            parent.ClosingAccountId = child.Id;
                        }

                        // Dump accounts
                        foreach (var entry in accounts)
                        {
                            outputFile.AddRow(entry.Value);
                            legacyIdFile.AddRow(new LegacyToID(entry.Key, entry.Value.Id));
                        }

                        closingAccounts.Clear();
                        accounts.Clear();
                        prevFiscalYearId = fiscalYearId;

                        if (!notEof)
                        {
                            break;
                        }
                    }
                    
                    string legacyKey = importer.GetString(14);
                    if (string.IsNullOrWhiteSpace(legacyKey))
                    {
                        continue;
                    }

                    Account account = new Account();
                    account.FiscalYearId = fiscalYearId;
                    account.AccountNumber = importer.GetString(3, 128);
                    account.Name = importer.GetString(4, 255);
                    account.Category = importer.GetEnum<AccountCategory>(5);
                    account.IsNormallyDebit = importer.GetBool(11);
                    string closingAccount = importer.GetString(12);
                    account.IsActive = importer.GetBool(13);
                    account.BeginningBalance = importer.GetDecimal(15);
                    account.SortKey = importer.GetString(16, 128);

                    ImportCreatedModifiedInfo(account, importer, 17);

                    if (!string.IsNullOrWhiteSpace(closingAccount))
                    {
                        closingAccounts.Add(legacyKey, closingAccount);
                    }

                    // Account groups
                    account.Group1Id = GetAccountGroup(importer, 6);
                    account.Group2Id = GetAccountGroup(importer, 7);
                    account.Group3Id = GetAccountGroup(importer, 8);
                    account.Group4Id = GetAccountGroup(importer, 9);

                    account.AssignPrimaryKey();

                    string[] segmentKeys = importer.GetString(2).Split(',');
                    int level = 0;
                    foreach (string key in segmentKeys)
                    {
                        level++;
                        Guid segmentId;
                        if (!_segmentIds.TryGetValue(key, out segmentId))
                        {
                            importer.LogError($"Invalid segment Id {key}.");
                        }
                        else
                        {
                            var segment = new AccountSegment();
                            segment.SegmentId = segmentId;
                            segment.Level = level;
                            segment.AccountId = account.Id;
                            segment.AssignPrimaryKey();
                            segmentOutputFile.AddRow(segment);
                        }                           
                    }

                    accounts.Add(legacyKey, account);

                    count++;
                }
                
                legacyIdFile.Dispose();
                outputFile.Dispose();
                segmentOutputFile.Dispose();
            }
        }

        private void ConvertLedgerAccounts(string filename)
        {
            DomainContext context = new DomainContext();

            LoadLedgerIds();
            var outputFile = new FileExport<LedgerAccount>(Path.Combine(_outputDirectory, OutputFile.GL_LedgerAccountFile), false);
            var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, OutputFile.LedgerAccountIdMappingFile), false, true);

            outputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    Guid? ledgerId = GetLedgerId(importer, 0);

                    if (ledgerId == null)
                    {
                        continue;
                    }

                    int legacyKey = importer.GetInt(3);
                    if (legacyKey == 0)
                    {
                        continue;
                    }

                    LedgerAccount account = new LedgerAccount();
                    account.LedgerId = ledgerId;
                    account.AccountNumber = importer.GetString(1, 128);
                    account.Name = importer.GetString(2, 128);
                    account.AssignPrimaryKey();
                    outputFile.AddRow(account);
                    legacyIdFile.AddRow(new LegacyToID(legacyKey, account.Id));
                    count++;
                }
            }
            legacyIdFile.Dispose();
            outputFile.Dispose();

        }

        private void ConvertLedgerAccountYears(string filename)
        {
            DomainContext context = new DomainContext();

            LoadFiscalYearIds();
            var outputFile = new FileExport<LedgerAccountYear>(Path.Combine(_outputDirectory, OutputFile.GL_LedgerAccountYearFile), false);
            var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, OutputFile.LedgerAccountYearIdMappingFile), false, true);

            outputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    Guid? fiscalYearId = GetFiscalYearId(importer, 0);

                    if (fiscalYearId == null)
                    {
                        continue;
                    }

                    string legacyKey = importer.GetString(3);
                    if (string.IsNullOrWhiteSpace(legacyKey))
                    {
                        continue;
                    }

                    LedgerAccountYear account = new LedgerAccountYear();
                    account.FiscalYearId = fiscalYearId;
                    
                    account.AssignPrimaryKey();
                    outputFile.AddRow(account);
                    legacyIdFile.AddRow(new LegacyToID(legacyKey, account.Id));
                    count++;
                }
            }
            legacyIdFile.Dispose();
            outputFile.Dispose();

        }


        private Guid? GetAccountGroup(FileImport importer, int column)
        {
            int key = importer.GetInt(column);
            if (key == 0)
            {
                return null;
            }

            Guid id;
            if (!_accountGroupIds.TryGetValue(key, out id))
            {
                importer.LogError($"Invalid account group legacy key {key}.");
                return null;
            }
            return id;
        }

        /// <summary>
        /// Load legacy ledger IDs into dictionary.
        /// </summary>
        private void LoadLedgerIds()
        {
            if (_ledgerIds.Count == 0)
            {
                _ledgerIds = LoadIntLegacyIds(_outputDirectory, OutputFile.LedgerIdMappingFile);
            }
        }

        /// <summary>
        /// Load legacy fiscal year IDs into dictionary.
        /// </summary>
        private void LoadFiscalYearIds()
        {
            if (_fiscalYearIds.Count == 0)
            {
                _fiscalYearIds = LoadLegacyIds(_outputDirectory, OutputFile.FiscalYearIdMappingFile);
            }
        }

        /// <summary>
        /// Load legacy account group IDs into dictionary.
        /// </summary>
        private void LoadAccountGroupIds()
        {
            if (_accountGroupIds.Count == 0)
            {
                _accountGroupIds = LoadIntLegacyIds(_outputDirectory, OutputFile.AccountGroupIdMappingFile);
            }
        }

        /// <summary>
        /// Load legacy segment IDs into dictionary.
        /// </summary>
        private void LoadSegmentIds()
        {
            if (_segmentIds.Count == 0)
            {
                _segmentIds = LoadLegacyIds(_outputDirectory, OutputFile.SegmentIdMappingFile);
            }
        }

        private Guid? GetFiscalYearId(FileImport importer, int column)
        {
            Guid? ledgerId;
            return GetFiscalYearId(importer, column, out ledgerId);
        }

        private Guid? GetFiscalYearId(FileImport importer, int column, out Guid? ledgerId)
        {
            ledgerId = GetLedgerId(importer, column);
            if (ledgerId == null)
            {
                return null;
            }

            string code = importer.GetString(column);
            string yearName = importer.GetString(column + 1);
            string legacyKey = $"{code},{yearName}";
            Guid fiscalYearId;
            if (_fiscalYearIds.TryGetValue(legacyKey, out fiscalYearId))
            {
                return fiscalYearId;
            }

            importer.LogError($"Invalid year \"{yearName}\" for cid \"{code}\".");
            return null;
        }

        private Guid? GetLedgerId(FileImport importer, int column)
        {
            Guid? ledgerId = null;

            // Legacy company ID
            string code = importer.GetString(column);
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            if (_ledgerIds.Count > 0)
            {
                int cid = 0;
                Guid id;
                if (!int.TryParse(code, out cid) || !_ledgerIds.TryGetValue(cid, out id))
                {
                    importer.LogError($"Invalid legacy company ID \"{code}\"");
                    return null;
                }

                ledgerId = id;
            }

            return ledgerId;
        }


    }
}
