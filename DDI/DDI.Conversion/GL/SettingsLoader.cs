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
    internal class SettingsLoader : ConversionBase
    {
        public enum ConversionMethod
        {
            Codes = 70001,
            BusinessUnits = 70002,
            BusinessUnitUsers = 70003,
            Ledgers = 70004,
            FiscalYears = 70005,
            FiscalPeriods = 70006,
        }

        private string _glDirectory;
        private string _outputDirectory;
        private Dictionary<int, Guid> _ledgerIds;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _glDirectory = Path.Combine(baseDirectory, DirectoryName.GL);
            _outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.GL);
            _ledgerIds = new Dictionary<int, Guid>();

            // Make sure the IS Payload directory exists.
            Directory.CreateDirectory(_outputDirectory);

            RunConversion(ConversionMethod.Codes, () => LoadLegacyCodes(InputFile.GL_FWCodes));
            RunConversion(ConversionMethod.BusinessUnits, () => LoadBusinessUnits(InputFile.GL_BusinessUnits));
            RunConversion(ConversionMethod.BusinessUnitUsers, () => LoadBusinessUnitUsers(InputFile.GL_BusinessUnitUsers));
            RunConversion(ConversionMethod.Ledgers, () => LoadLedgers(InputFile.GL_Ledgers));
            RunConversion(ConversionMethod.FiscalYears, () => LoadFiscalYears(InputFile.GL_FiscalYears));
            RunConversion(ConversionMethod.FiscalPeriods, () => LoadFiscalPeriods(InputFile.GL_FiscalPeriods));
        }

        private void LoadLegacyCodes(string filename)
        {
            DomainContext context = new DomainContext();
            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                while (importer.GetNextRow())
                {
                    int codeSet = importer.GetInt(0);
                    string code = importer.GetString(1);
                    string description = importer.GetString(2);
                    int int1 = importer.GetInt(3);
                    int int2 = importer.GetInt(4);
                    string text1 = importer.GetString(5);
                    string text2 = importer.GetString(6);

                    /* THIS CODE CAN BE UNCOMMENTED & MODIFIED IF WE NEED TO LOAD ANY LEGACY CODE VALUES...
                     * 
                    switch (codeSet)
                    {
                        case EFT_FORMAT_SET:
                            context.CP_EFTFormats.AddOrUpdate(
                                prop => prop.Code,
                                new EFTFormat { Code = code, Name = description, IsActive = true });
                            break;
                    }
                    */
                }
            }
            context.SaveChanges();
        } // LoadLegacyCodes

        private void LoadBusinessUnits(string filename)
        {
            DomainContext context = new DomainContext();

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        continue;
                    }
                    
                    string name = importer.GetString(1);
                    BusinessUnitType type = importer.GetEnum<BusinessUnitType>(2);

                    context.GL_BusinessUnits.AddOrUpdate(p => p.Code,
                        new BusinessUnit()
                        {
                            Code = code,
                            Name = name,
                            BusinessUnitType = type
                        });
                    count++;
                }

                context.SaveChanges();
            }
        }

        private void LoadBusinessUnitUsers(string filename)
        {
            DomainContext context = new DomainContext();

            IList<BusinessUnit> businessUnits = LoadEntities(context.GL_BusinessUnits);
            IList<User> users = LoadEntities(context.Users, nameof(User.DefaultBusinessUnit));

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        continue;
                    }

                    BusinessUnit bu = businessUnits.FirstOrDefault(p => p.Code == code);
                    if (bu == null)
                    {
                        importer.LogError($"Invalid business unit code \"{code}\".");
                        continue;
                    }
                    
                    string username = importer.GetString(1);
                    User user = GetUserByName(users, username);
                    if (user == null)
                    {
                        importer.LogError($"Invalid user name \"{username}\".");
                        continue;
                    }

                    bool isDefault = importer.GetBool(2);

                    if (!user.BusinessUnits.Any(p => p.Id == bu.Id))
                    {
                        user.BusinessUnits.Add(bu);
                    }

                    if (isDefault)
                    {
                        user.DefaultBusinessUnit = bu;
                    }

                    count++;
                }

                context.SaveChanges();
            }
        }

        private void LoadLedgers(string filename)
        {
            DomainContext context = new DomainContext();

            FileExport<LegacyToID> companyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, OutputFile.BusinessUnitIdMappingFile), false, true);
            FileExport<LegacyToID> LedgerIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, OutputFile.LedgerIdMappingFile), false, true);

            var businessUnits = LoadEntities(context.GL_BusinessUnits);

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;
                Ledger orgLedger = null;

                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        continue;
                    }

                    BusinessUnit bu = businessUnits.FirstOrDefault(p => p.Code == code);
                    if (bu == null)
                    {
                        importer.LogError($"Invalid business unit code \"{code}\".");
                        continue;
                    }

                    int companyId = importer.GetInt(15);
                    companyIdFile.AddRow(new LegacyToID(companyId, bu.Id));

                    Ledger ledger = context.GL_Ledgers.Include(p => p.BusinessUnit)
                                                      .Include(p => p.OrgLedger)
                                                      .Include(p => p.DefaultFiscalYear)
                                                      .FirstOrDefault(p => p.Code == code);
                    if (ledger == null)
                    {
                        ledger = new Ledger();
                        ledger.AssignPrimaryKey();
                        ledger.BusinessUnit = bu;
                        ledger.Code = code;
                        context.GL_Ledgers.Add(ledger);
                    }

                    ledger.OrgLedger = orgLedger;
                    ledger.Name = importer.GetString(1);
                    ledger.Status = importer.GetEnum<LedgerStatus>(2);
                    ledger.NumberOfSegments = importer.GetInt(3);
                    ledger.FixedBudgetName = importer.GetString(4);
                    ledger.WorkingBudgetName = importer.GetString(5);
                    ledger.WhatIfBudgetName = importer.GetString(6);
                    ledger.PriorPeriodPostingMode = importer.GetEnum<PriorPeriodPostingMode>(9);
                    ledger.CapitalizeHeaders = importer.GetBool(10);
                    ledger.ApproveJournals = importer.GetBool(14);
                    ledger.AccountGroupLevels = importer.GetInt(16);
                    ledger.AccountGroup1Title = importer.GetString(17);
                    ledger.AccountGroup2Title = importer.GetString(18);
                    ledger.AccountGroup3Title = importer.GetString(19);
                    ledger.AccountGroup4Title = importer.GetString(20);
                    ledger.PostAutomatically = importer.GetBool(21);
                    ledger.CopyCOAChanges = importer.GetBool(22);
                    ledger.IsParent = importer.GetBool(23);
                    ledger.FundAccounting = importer.GetBool(24);

                    if (ledger.BusinessUnit.BusinessUnitType == BusinessUnitType.Organization)
                    {
                        orgLedger = ledger;
                    }

                    LedgerIdFile.AddRow(new LegacyToID(companyId, ledger.Id));

                    count++;
                }

                context.SaveChanges();
            }

            companyIdFile.Dispose();
            LedgerIdFile.Dispose();
        }

        private void LoadFiscalYears(string filename)
        {
            DomainContext context = new DomainContext();

            LoadLedgerIds();
            var ledgers = LoadEntities(context.GL_Ledgers, nameof(Ledger.DefaultFiscalYear));

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        continue;
                    }

                    int cid = 0;
                    Guid ledgerId;

                    if (!int.TryParse(code, out cid) || !_ledgerIds.TryGetValue(cid, out ledgerId))
                    {
                        importer.LogError($"Invalid legacy company ID \"{code}\"");
                        continue;
                    }

                    Ledger ledger = ledgers.FirstOrDefault(p => p.Id == ledgerId);

                    string yearName = importer.GetString(1);

                    FiscalYear year = context.GL_FiscalYears.FirstOrDefault(p => p.LedgerId == ledgerId && p.Name == yearName);
                    if (year == null)
                    {
                        year = new FiscalYear();
                        context.GL_FiscalYears.Add(year);
                        year.LedgerId = ledgerId;
                        year.Name = yearName;
                    }

                    year.StartDate = importer.GetDateTime(2);
                    year.EndDate = importer.GetDateTime(3);
                    year.NumberOfPeriods = importer.GetInt(4);
                    year.Status = importer.GetEnum<FiscalYearStatus>(5);
                    year.CurrentPeriodNumber = importer.GetInt(8);

                    if (importer.GetBool(9))
                    {
                        ledger.DefaultFiscalYear = year;
                    }

                    ImportCreatedModifiedInfo(year, importer, 10);

                    count++;        
                }

                context.SaveChanges();
            }
        }

        private void LoadFiscalPeriods(string filename)
        {
            DomainContext context = new DomainContext();

            LoadLedgerIds();
            var years = LoadEntities(context.GL_FiscalYears, nameof(FiscalYear.FiscalPeriods));
            var periods = LoadEntities(context.GL_FiscalPeriods, nameof(FiscalPeriod.FiscalYear));

            using (var importer = CreateFileImporter(_glDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        continue;
                    }

                    int cid = 0;
                    Guid ledgerId;

                    if (!int.TryParse(code, out cid) || !_ledgerIds.TryGetValue(cid, out ledgerId))
                    {
                        importer.LogError($"Invalid legacy company ID \"{code}\"");
                        continue;
                    }

                    string yearName = importer.GetString(1);

                    FiscalYear year = years.FirstOrDefault(p => p.LedgerId == ledgerId && p.Name == yearName);
                    if (year == null)
                    {
                        importer.LogError($"No fiscal year \"{yearName}\" found for company ID \"{code}\".");
                    }

                    int periodNumber = importer.GetInt(2);

                    FiscalPeriod period = periods.FirstOrDefault(p => p.FiscalYearId == year.Id && p.PeriodNumber == periodNumber);
                    if (period == null)
                    {
                        period = new FiscalPeriod();
                        context.GL_FiscalPeriods.Add(period);
                        year.FiscalPeriods.Add(period);
                        period.PeriodNumber = periodNumber;
                    }

                    period.StartDate = importer.GetDateTime(3);
                    period.EndDate = importer.GetDateTime(4);
                    period.IsAdjustmentPeriod = importer.GetBool(5);
                    period.Status = importer.GetEnum<FiscalPeriodStatus>(6);

                    count++;
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// If necessary, load legacy constituent IDs into dictionary.
        /// </summary>
        private void LoadLedgerIds()
        {
            if (_ledgerIds.Count == 0)
            {
                _ledgerIds = LoadIntLegacyIds(_outputDirectory, OutputFile.LedgerIdMappingFile);
            }
        }
    }
}
