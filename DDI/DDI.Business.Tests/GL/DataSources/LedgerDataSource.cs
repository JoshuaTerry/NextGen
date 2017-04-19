using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.GL;
using DDI.Data;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Tests.GL.DataSources
{
    public static class LedgerDataSource
    {
        public const string NEW_LEDGER_CODE = "NEW";

        public static IList<Ledger> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<Ledger> existing = uow.GetRepositoryOrNull<Ledger>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            var businessUnits = BusinessUnitDataSource.GetDataSource(uow);
            var ledgers = new List<Ledger>();
            var levels = new List<SegmentLevel>();

            foreach (var unit in businessUnits)
            {
                Ledger ledger = new Ledger()
                {
                    AccountGroup1Title = "Category",
                    AccountGroup2Title = "Class",
                    AccountGroup3Title = "Type",
                    AccountGroup4Title = "Other",
                    AccountGroupLevels = 3,
                    ApproveJournals = true,
                    BusinessUnit = unit,
                    IsParent = (unit.BusinessUnitType == BusinessUnitType.Organization),
                    Code = unit.Code,
                    Name = unit.Name,
                    CapitalizeHeaders = true,
                    CopyCOAChanges = true,
                    FundAccounting = true,
                    FixedBudgetName = "Approved Budget",
                    WorkingBudgetName = "Working Budget",
                    WhatIfBudgetName = "What If Budget",
                    Status = LedgerStatus.Active,
                    NumberOfSegments = 5,
                    LedgerAccounts = new List<LedgerAccount>(),                    
                    PriorPeriodPostingMode = PriorPeriodPostingMode.Prohibit,
                    Id = GuidHelper.NewSequentialGuid()
                };

                BuildSegmentLevels(ledger);
                ledgers.Add(ledger);

                if (unit.Code == BusinessUnitDataSource.UNIT_CODE_SEPARATE)
                {
                    // The separate business unit gets a second ledger.
                    ledger = new Ledger()
                    {
                        AccountGroup1Title = "Category",
                        AccountGroup2Title = "",
                        AccountGroup3Title = "",
                        AccountGroup4Title = "",
                        AccountGroupLevels = 1,
                        ApproveJournals = true,
                        BusinessUnit = unit,
                        IsParent = false,
                        Code = NEW_LEDGER_CODE,
                        Name = "New ledger for " + unit.Code,
                        CapitalizeHeaders = false,
                        CopyCOAChanges = true,
                        FundAccounting = false,
                        FixedBudgetName = "Approved Budget",
                        WorkingBudgetName = "Working Budget",
                        WhatIfBudgetName = "What If Budget",
                        Status = LedgerStatus.Empty,
                        NumberOfSegments = 2,
                        LedgerAccounts = new List<LedgerAccount>(),
                        Id = GuidHelper.NewSequentialGuid()
                    };

                    BuildSegmentLevelsForNewLedger(ledger);
                    ledgers.Add(ledger);
                }
            }

            Ledger orgLedger = ledgers.First(p => p.IsParent == true);
            foreach (var entry in ledgers.Where(p => p.IsParent == false && p.BusinessUnit.BusinessUnitType == BusinessUnitType.Common))
            {
                entry.OrgLedger = orgLedger;
            }

            uow.CreateRepositoryForDataSource(ledgers);

            levels = ledgers.SelectMany(p => p.SegmentLevels).ToList();
            uow.CreateRepositoryForDataSource(levels);

            // Force the set of ledgers into the LedgerLogic's LedgerCache.
            uow.GetBusinessLogic<LedgerLogic>().LedgerCache = new CachedRepository<Ledger>(uow.GetRepository<Ledger>(), ledgers.AsQueryable());
            
            return ledgers;
        }

        private static void BuildSegmentLevels(Ledger ledger)
        {
            ledger.SegmentLevels = new List<SegmentLevel>();
            ledger.SegmentLevels.Add(new SegmentLevel
            {
                Ledger = ledger,
                Level = 1,
                SortOrder = 1,
                Abbreviation = "Fund",
                Name = "Fund",
                Format = SegmentFormat.Numeric,
                IsLinked = false,
                Length = 2,
                Separator = "-",
                Type = SegmentType.Fund
            });

            ledger.SegmentLevels.Add(new SegmentLevel
            {
                Ledger = ledger,
                Level = 2,
                SortOrder = 2,
                Abbreviation = "Acct",
                Name = "Account",
                Format = SegmentFormat.Numeric,
                IsLinked = false,
                Length = 3,
                Separator = "-",
            });

            ledger.SegmentLevels.Add(new SegmentLevel
            {
                Ledger = ledger,
                Level = 3,
                SortOrder = 3,
                Abbreviation = "Sub",
                Name = "Subaccount",
                Format = SegmentFormat.Numeric,
                IsLinked = true,
                Length = 2,
                Separator = "-",
            });

            ledger.SegmentLevels.Add(new SegmentLevel
            {
                Ledger = ledger,
                Level = 4,
                SortOrder = 4,
                Abbreviation = "Det",
                Name = "Detail",
                Format = SegmentFormat.Numeric,
                IsLinked = true,
                Length = 2,
                Separator = "-",
            });

            ledger.SegmentLevels.Add(new SegmentLevel
            {
                Ledger = ledger,
                Level = 5,
                SortOrder = 5,
                Abbreviation = "Dep",
                Name = "Department",
                Format = SegmentFormat.Numeric,
                IsLinked = false,
                Length = 2,
                Separator = "",
            });
        }

        private static void BuildSegmentLevelsForNewLedger(Ledger ledger)
        {
            ledger.SegmentLevels = new List<SegmentLevel>();
            ledger.SegmentLevels.Add(new SegmentLevel
            {
                Ledger = ledger,
                Level = 1,
                SortOrder = 1,
                Abbreviation = "Acct",
                Name = "Account",
                Format = SegmentFormat.Numeric,
                IsLinked = false,
                Length = 4,
                Separator = ".",
            });

            ledger.SegmentLevels.Add(new SegmentLevel
            {
                Ledger = ledger,
                Level = 2,
                SortOrder = 2,
                Abbreviation = "Dept",
                Name = "Department",
                Format = SegmentFormat.Alpha,
                IsLinked = false,
                Length = 4,
                Separator = "",
            });

           
        }


    }
}
