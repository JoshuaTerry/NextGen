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
    internal class AccountConverter : GLConversionBase
    {
        public enum ConversionMethod
        {
            Segments = 70100,
            AccountGroups = 70101,
            Accounts = 70102,
            LedgerAccounts = 70103,
            LedgerAccountYears = 70104,
            AccountPriorYears = 70105,
            LedgerAccountMerges = 70106,
            AccountBudgets = 70107,
        }

        private Dictionary<int, Guid> _accountGroupIds;
        private Dictionary<string, Guid> _accountIds;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            Initialize(baseDirectory);

            // Make sure the IS Payload directory exists.
            Directory.CreateDirectory(OutputDirectory);

            RunConversion(ConversionMethod.Segments, () => ConvertSegments(InputFile.GL_Segments));
            RunConversion(ConversionMethod.AccountGroups, () => ConvertAccountGroups(InputFile.GL_AccountGroups));
            RunConversion(ConversionMethod.Accounts, () => ConvertAccounts(InputFile.GL_Accounts));
            RunConversion(ConversionMethod.LedgerAccounts, () => ConvertLedgerAccounts(InputFile.GL_LedgerAccounts));
            RunConversion(ConversionMethod.LedgerAccountYears, () => ConvertLedgerAccountYears(InputFile.GL_LedgerAccountYears));
            RunConversion(ConversionMethod.AccountPriorYears, () => ConvertAccountPriorYears(InputFile.GL_AccountPriorYears));
            RunConversion(ConversionMethod.LedgerAccountMerges, () => ConvertLedgerAccountMerges(InputFile.GL_LedgerAccountMerges));
            RunConversion(ConversionMethod.AccountBudgets, () => ConvertAccountBudgets(InputFile.GL_AccountBudgets));
        }


        private void ConvertSegments(string filename)
        {
            DomainContext context = new DomainContext();

            LoadLedgerIds();
            LoadFiscalYearIds();
            var levels = LoadEntities(context.GL_SegmentLevels);
            var segments = new Dictionary<string, Segment>();
            var segmentLinks = new Dictionary<string, string>();

            using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    Guid? ledgerId;
                    Guid? fiscalYearID = GetFiscalYearId(importer, 0, false, out ledgerId);

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
                var outputFile = new FileExport<Segment>(Path.Combine(OutputDirectory, OutputFile.GL_SegmentFile), false);
                var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(OutputDirectory, OutputFile.GL_SegmentIdMappingFile), false, true);

                outputFile.AddHeaderRow();

                foreach (var entry in segments)
                {
                    outputFile.AddRow(entry.Value);
                    legacyIdFile.AddRow(new LegacyToID(entry.Key, entry.Value.Id));
                }

                legacyIdFile.Dispose();
                outputFile.Dispose();
            }

            context.Dispose();
        }

        private void ConvertAccountGroups(string filename)
        {
            LoadFiscalYearIds();
            var groupLinks = new Dictionary<int, int>();
            var groups = new Dictionary<int, AccountGroup>();

            using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
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
                var outputFile = new FileExport<AccountGroup>(Path.Combine(OutputDirectory, OutputFile.GL_AccountGroupFile), false);
                var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(OutputDirectory, OutputFile.GL_AccountGroupIdMappingFile), false, true);

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
            LoadFiscalYearIds();
            LoadAccountGroupIds();
            LoadSegmentIds();
            var accounts = new Dictionary<string, Account>();
            var closingAccounts = new Dictionary<string, string>();

            var outputFile = new FileExport<Account>(Path.Combine(OutputDirectory, OutputFile.GL_AccountFile), false);
            var segmentOutputFile = new FileExport<AccountSegment>(Path.Combine(OutputDirectory, OutputFile.GL_AccountSegmentFile), false);
            var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(OutputDirectory, OutputFile.GL_AccountIdMappingFile), false, true);

            outputFile.AddHeaderRow();
            segmentOutputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
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

                    // Dump AccountSegment rows
                    string[] segmentKeys = importer.GetString(2).Split(',');
                    int level = 0;
                    foreach (string key in segmentKeys)
                    {
                        level++;
                        Guid segmentId;
                        if (!SegmentIds.TryGetValue(key, out segmentId))
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
            LoadLedgerIds();
            var outputFile = new FileExport<LedgerAccount>(Path.Combine(OutputDirectory, OutputFile.GL_LedgerAccountFile), false);
            var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(OutputDirectory, OutputFile.GL_LedgerAccountIdMappingFile), false, true);

            outputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
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
            LoadFiscalYearIds();
            LoadLedgerAccountIds();
            LoadAccountIds();

            var outputFile = new FileExport<LedgerAccountYear>(Path.Combine(OutputDirectory, OutputFile.GL_LedgerAccountYearFile), false);
            var legacyIdFile = new FileExport<LegacyToID>(Path.Combine(OutputDirectory, OutputFile.GL_LedgerAccountYearIdMappingFile), false, true);

            outputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
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

                    string accountKey = importer.GetString(5);
                    int ledgerAccountKey = importer.GetInt(6);
                    Guid accountId, ledgerAccountId;

                    if (!_accountIds.TryGetValue(accountKey, out accountId))
                    {
                        importer.LogError($"Invalid account legacy key \"{accountKey}\".");
                        continue;
                    }

                    if (!LedgerAccountIds.TryGetValue(ledgerAccountKey, out ledgerAccountId))
                    {
                        importer.LogError($"Invalid ledger account legacy key \"{ledgerAccountKey}\".");
                        continue;
                    }

                    LedgerAccountYear account = new LedgerAccountYear();
                    account.FiscalYearId = fiscalYearId;
                    account.AccountId = accountId;
                    account.LedgerAccountId = ledgerAccountId;
                    account.IsMerge = importer.GetBool(4);
                    
                    account.AssignPrimaryKey();
                    outputFile.AddRow(account);
                    legacyIdFile.AddRow(new LegacyToID(legacyKey, account.Id));
                    count++;
                }
            }
            legacyIdFile.Dispose();
            outputFile.Dispose();
        }

        private void ConvertAccountPriorYears(string filename)
        {
            LoadFiscalYearIds();
            LoadAccountIds();

            var outputFile = new FileExport<AccountPriorYear>(Path.Combine(OutputDirectory, OutputFile.GL_AccountPriorYearFile), false);

            outputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    Guid? fiscalYearId = GetFiscalYearId(importer, 0);

                    if (fiscalYearId == null)
                    {
                        continue;
                    }

                    string accountKey = importer.GetString(2);
                    string priorAccountKey = importer.GetString(4);
                    Guid accountId, priorAccountId;

                    if (!_accountIds.TryGetValue(accountKey, out accountId))
                    {
                        importer.LogError($"Invalid account legacy key \"{accountKey}\".");
                        continue;
                    }

                    if (!_accountIds.TryGetValue(priorAccountKey, out priorAccountId))
                    {
                        importer.LogError($"Invalid account legacy key \"{priorAccountKey}\".");
                        continue;
                    }

                    AccountPriorYear account = new AccountPriorYear();
                    account.AccountId = accountId;
                    account.PriorAccountId = priorAccountId;
                    account.Percentage = importer.GetDecimal(6);

                    account.AssignPrimaryKey();
                    outputFile.AddRow(account);
                    count++;
                }
            }

            outputFile.Dispose();
        }

        private void ConvertLedgerAccountMerges(string filename)
        {
            DomainContext context = new DomainContext();

            LoadFiscalYearIds();
            LoadLedgerAccountIds();
            IList<User> users = LoadEntities(context.Users);

            var outputFile = new FileExport<LedgerAccountMerge>(Path.Combine(OutputDirectory, OutputFile.GL_LedgerAccountMergeFile), false);

            outputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    Guid? fiscalYearId = GetFiscalYearId(importer, 0);

                    if (fiscalYearId == null)
                    {
                        continue;
                    }

                    int fromKey = importer.GetInt(2);
                    int toKey = importer.GetInt(3);
                    Guid fromLedgerId, toLedgerId;

                    if (!LedgerAccountIds.TryGetValue(fromKey, out fromLedgerId))
                    {
                        importer.LogError($"Invalid ledger account legacy key \"{fromKey}\".");
                        continue;
                    }

                    if (!LedgerAccountIds.TryGetValue(toKey, out toLedgerId))
                    {
                        importer.LogError($"Invalid ledger account legacy key \"{toKey}\".");
                        continue;
                    }

                    LedgerAccountMerge merge = new LedgerAccountMerge();
                    merge.FiscalYearId = fiscalYearId;
                    merge.FromAccountId = fromLedgerId;
                    merge.ToAccountId = toLedgerId;
                    merge.FromAccountNumber = importer.GetString(4, 128);
                    merge.ToAccountNumber = importer.GetString(5, 128);
                    string userName = importer.GetString(6);
                    merge.MergedOn = importer.GetDateTime(7);

                    if (!string.IsNullOrWhiteSpace(userName))
                    {
                        User user = GetUserByName(users, userName);
                        if (user == null)
                        {
                            importer.LogError($"Invalid user name \"{userName}\".");
                        }
                        else
                        {
                            merge.MergedById = user.Id;
                        }
                    }

                    merge.AssignPrimaryKey();
                    outputFile.AddRow(merge);
                    count++;
                }
            }

            outputFile.Dispose();
            context.Dispose();
        }

        private void ConvertAccountBudgets(string filename)
        {
            LoadFiscalYearIds();
            LoadAccountIds();

            var outputFile = new FileExport<AccountBudgetFlattened>(Path.Combine(OutputDirectory, OutputFile.GL_AccountBudgetFile), false);

            outputFile.AddHeaderRow();

            using (var importer = CreateFileImporter(GLDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    Guid? fiscalYearId = GetFiscalYearId(importer, 0);

                    if (fiscalYearId == null)
                    {
                        continue;
                    }

                    string accountKey = importer.GetString(3);
                    
                    Guid accountId;

                    if (!_accountIds.TryGetValue(accountKey, out accountId))
                    {
                        importer.LogError($"Invalid account legacy key \"{accountKey}\".");
                        continue;
                    }               

                    var budget = new AccountBudgetFlattened();
                    budget.AccountId = accountId;
                    budget.BudgetType = importer.GetEnum<BudgetType>(4);
                    budget.YearAmount = importer.GetDecimal(5);

                    budget.Amount01 = importer.GetDecimal(6);
                    budget.Amount02 = importer.GetDecimal(7);
                    budget.Amount03 = importer.GetDecimal(8);
                    budget.Amount04 = importer.GetDecimal(9);
                    budget.Amount05 = importer.GetDecimal(10);
                    budget.Amount06 = importer.GetDecimal(11);
                    budget.Amount07 = importer.GetDecimal(12);
                    budget.Amount08 = importer.GetDecimal(13);
                    budget.Amount09 = importer.GetDecimal(14);
                    budget.Amount10 = importer.GetDecimal(15);
                    budget.Amount11 = importer.GetDecimal(16);
                    budget.Amount12 = importer.GetDecimal(17);
                    budget.Amount13 = importer.GetDecimal(18);

                    budget.Percent01 = importer.GetDecimal(19);
                    budget.Percent02 = importer.GetDecimal(20);
                    budget.Percent03 = importer.GetDecimal(21);
                    budget.Percent04 = importer.GetDecimal(22);
                    budget.Percent05 = importer.GetDecimal(23);
                    budget.Percent06 = importer.GetDecimal(24);
                    budget.Percent07 = importer.GetDecimal(25);
                    budget.Percent08 = importer.GetDecimal(26);
                    budget.Percent09 = importer.GetDecimal(27);
                    budget.Percent10 = importer.GetDecimal(28);
                    budget.Percent11 = importer.GetDecimal(29);
                    budget.Percent12 = importer.GetDecimal(30);
                    budget.Percent13 = importer.GetDecimal(31);

                    ImportCreatedModifiedInfo(budget, importer, 32);              

                    budget.AssignPrimaryKey();
                    outputFile.AddRow(budget);
                    count++;
                }
            }

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
        /// Load legacy account group IDs into a dictionary.
        /// </summary>
        private void LoadAccountGroupIds()
        {
            if (_accountGroupIds == null)
            {
                _accountGroupIds = new Dictionary<int, Guid>();
                _accountGroupIds = LoadIntLegacyIds(OutputDirectory, OutputFile.GL_AccountGroupIdMappingFile);
            }
        }


        /// <summary>
        /// Load legacy account IDs into a dictionary.
        /// </summary>
        private void LoadAccountIds()
        {
            if (_accountIds == null)
            {
                _accountIds = LoadLegacyIds(OutputDirectory, OutputFile.GL_AccountIdMappingFile);
            }
        }

        /// <summary>
        /// FileExport doesn't support complex properties, so the AccountBudget class needs to be flattened.
        /// </summary>
        private class AccountBudgetFlattened : AccountBudget
        {
            public decimal Amount01 { get; set; }
            public decimal Amount02 { get; set; }
            public decimal Amount03 { get; set; }
            public decimal Amount04 { get; set; }
            public decimal Amount05 { get; set; }
            public decimal Amount06 { get; set; }
            public decimal Amount07 { get; set; }
            public decimal Amount08 { get; set; }
            public decimal Amount09 { get; set; }
            public decimal Amount10 { get; set; }
            public decimal Amount11 { get; set; }
            public decimal Amount12 { get; set; }
            public decimal Amount13 { get; set; }

            public decimal Percent01 { get; set; }
            public decimal Percent02 { get; set; }
            public decimal Percent03 { get; set; }
            public decimal Percent04 { get; set; }
            public decimal Percent05 { get; set; }
            public decimal Percent06 { get; set; }
            public decimal Percent07 { get; set; }
            public decimal Percent08 { get; set; }
            public decimal Percent09 { get; set; }
            public decimal Percent10 { get; set; }
            public decimal Percent11 { get; set; }
            public decimal Percent12 { get; set; }
            public decimal Percent13 { get; set; }

        }

    }
}
