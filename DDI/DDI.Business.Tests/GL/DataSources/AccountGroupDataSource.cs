using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Tests.GL.DataSources
{
    public static class AccountGroupDataSource
    {
        private static int _nextSequence;

        public static IList<AccountGroup> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<AccountGroup> existing = uow.GetRepositoryOrNull<AccountGroup>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            var ledgers = LedgerDataSource.GetDataSource(uow);
            var years = FiscalYearDataSource.GetDataSource(uow);
            var groups = new List<AccountGroup>();

            foreach(var year in years.Where(p => p.Ledger.BusinessUnit.BusinessUnitType != BusinessUnitType.Organization))
            {
                if (year.Segments == null)
                {
                    year.Segments = new List<Segment>();
                }

                if (year.Ledger.Code == LedgerDataSource.NEW_LEDGER_CODE)
                {
                    AddGroup(groups, year, "Assets", AccountCategory.Asset);
                    AddGroup(groups, year, "Liabilities", AccountCategory.Liability);
                    AddGroup(groups, year, "Equity", AccountCategory.Fund);
                    AddGroup(groups, year, "Revenue", AccountCategory.Revenue);
                    AddGroup(groups, year, "Expense", AccountCategory.Expense);
                }
                else
                { 
                    var group1 = AddGroup(groups, year, "ASSETS", AccountCategory.Asset);
                    var group2 = AddGroup(groups, year, "CASH", AccountCategory.Asset, group1);
                    AddGroup(groups, year, "CHECKING", AccountCategory.Asset, group2);

                    group2 = AddGroup(groups, year, "A/R", AccountCategory.Asset, group1);
                    AddGroup(groups, year, "RECEIVABLES", AccountCategory.Asset, group2);
                    AddGroup(groups, year, "OTHERS", AccountCategory.Asset, group2);

                    group1 = AddGroup(groups, year, "LIABILITIES", AccountCategory.Liability);
                    group2 = AddGroup(groups, year, "NOTES", AccountCategory.Asset, group1);

                    group1 = AddGroup(groups, year, "FUND BALANCES", AccountCategory.Fund);
                    AddGroup(groups, year, "GENERAL", AccountCategory.Asset, group1);
                    AddGroup(groups, year, "TEMPORARY", AccountCategory.Asset, group1);
                    AddGroup(groups, year, "PERMANENT", AccountCategory.Asset, group1);

                    group1 = AddGroup(groups, year, "INCOME", AccountCategory.Revenue);
                    AddGroup(groups, year, "GIFTS", AccountCategory.Revenue);

                    group1 = AddGroup(groups, year, "EXPENSE", AccountCategory.Revenue);
                    AddGroup(groups, year, "INTEREST", AccountCategory.Revenue);                    

                }


            }

            uow.CreateRepositoryForDataSource(groups);

            return groups;
        }    

        private static AccountGroup AddGroup(IList<AccountGroup> list, FiscalYear year, string name, AccountCategory category, AccountGroup parent = null)
        {
            AccountGroup group = new AccountGroup()
            {
                Name = name,
                FiscalYear = year,
                Id = GuidHelper.NewSequentialGuid(),
                ParentGroup = parent,
                ChildGroups = new List<AccountGroup>(),
                Category = category,
                Sequence = _nextSequence++,                
                Group1Accounts = new List<Account>(),
                Group2Accounts = new List<Account>(),
                Group3Accounts = new List<Account>(),
                Group4Accounts = new List<Account>()
            };

            list.Add(group);            

            if (parent != null)
            {
                parent.ChildGroups.Add(group);
            }

            return group;
        }

    }

    
}
