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
    public static class AccountDataSource
    {
        private static IList<Ledger> _ledgers;
        private static IList<FiscalYear> _years;
        private static IList<Segment> _segments;
        private static IList<Account> _accounts;
        private static IList<AccountSegment> _accountSegments;
        private static IList<AccountPriorYear> _accountPriorYears;
        private static IList<LedgerAccountYear> _ledgerAccountYears;
        private static IList<LedgerAccount> _ledgerAccounts;
        private static IList<AccountGroup> _accountGroups;
        private static LedgerLogic _ledgerLogic;
        private static SegmentLevel[] _segmentLevels;
        private static AccountGroup _accountGroup1, _accountGroup2, _accountGroup3;

        public static IList<Account> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<Account> existing = uow.GetRepositoryOrNull<Account>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            _ledgers = LedgerDataSource.GetDataSource(uow);
            _years = FiscalYearDataSource.GetDataSource(uow);
            _segments = SegmentDataSource.GetDataSource(uow);
            _accountGroups = AccountGroupDataSource.GetDataSource(uow);

            _accounts = new List<Account>();
            _ledgerAccounts = new List<LedgerAccount>();
            _ledgerAccountYears = new List<LedgerAccountYear>();
            _accountPriorYears = new List<AccountPriorYear>();
            _accountSegments = new List<AccountSegment>();
            _ledgerLogic = uow.GetBusinessLogic<LedgerLogic>();

            foreach (var year in _years.Where(p => p.Ledger.BusinessUnit.BusinessUnitType != BusinessUnitType.Organization))
            {
                _segmentLevels = _ledgerLogic.GetSegmentLevels(year.LedgerId.Value);

                if (year.Ledger.Code == LedgerDataSource.NEW_LEDGER_CODE)
                {
                    SetAccountGroups(year, "Assets", "", "");
                    AddAccount(year, "1001.CORP", "Regular Checking", AccountCategory.Asset);
                    AddAccount(year, "1501.CORP", "A/R Corporate", AccountCategory.Asset);
                    AddAccount(year, "1501.MIDW", "A/R Midwest", AccountCategory.Asset);

                    SetAccountGroups(year, "Liabilities", "", "");
                    AddAccount(year, "2002.CORP", "A/P Corporate", AccountCategory.Liability);
                    AddAccount(year, "2002.MIDW", "A/P Midwest", AccountCategory.Liability);

                    SetAccountGroups(year, "Equity", "", "");
                    AddAccount(year, "3001.CORP", "Capital", AccountCategory.Fund);
                    AddAccount(year, "3001.CORP", "Retained Earnings", AccountCategory.Fund);

                    SetAccountGroups(year, "Revenue", "", "");
                    AddAccount(year, "4001.MIDW", "Sales Midwest", AccountCategory.Revenue);

                    SetAccountGroups(year, "Expense", "", "");
                    AddAccount(year, "5001.CORP", "Office Supplies Corporate", AccountCategory.Expense);
                    AddAccount(year, "5001.MIDW", "Office Supplies Midwest", AccountCategory.Expense);

                    // Create a mapping from 01-100-10-10 / 02-100-10-10 into 1001.CORP for the new ledger.
                    Account current = _accounts.FirstOrDefault(p => p.FiscalYear == year && p.AccountNumber == "1001.CORP");
                    Account prior1 = _accounts.FirstOrDefault(p => p.FiscalYear.Name == FiscalYearDataSource.OPEN_YEAR && 
                            p.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE_SEPARATE && 
                            p.AccountNumber == "01-100-10-10");
                    Account prior2 = _accounts.FirstOrDefault(p => p.FiscalYear.Name == FiscalYearDataSource.OPEN_YEAR &&
                            p.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE_SEPARATE &&
                            p.AccountNumber == "02-100-10-10");

                    _accountPriorYears.Add(new AccountPriorYear()
                    {
                        Account = current,
                        PriorAccount = prior1,
                        Percentage = 100m,
                        Id = GuidHelper.NewGuid()
                    });

                    _accountPriorYears.Add(new AccountPriorYear()
                    {
                        Account = current,
                        PriorAccount = prior2,
                        Percentage = 100m,
                        Id = GuidHelper.NewGuid()
                    });                    
   
                }
                else
                {
                    SetAccountGroups(year, "ASSETS", "CASH", "CHECKING");
                    AddAccount(year, "01-100-10-01", "Cash on Hand", AccountCategory.Asset, 350m);
                    AddAccount(year, "01-100-10-10", "Regular Checking Account", AccountCategory.Asset);
                    AddAccount(year, "02-100-10-10", "Regular Checking Account", AccountCategory.Asset);

                    // Receivables
                    SetAccountGroups(year, "ASSETS", "A/R", "RECEIVABLES");
                    AddAccount(year, "01-150-50-42", "Due From Temp Restricted", AccountCategory.Asset);
                    AddAccount(year, "01-150-50-43", "Due To Temp Restricted", AccountCategory.Asset);
                    AddAccount(year, "01-150-50-44", "Due From Perm Restricted", AccountCategory.Asset);
                    AddAccount(year, "01-150-50-45", "Due To Perm Restricted", AccountCategory.Asset);

                    AddAccount(year, "02-150-50-40", "Due From Unrestricted", AccountCategory.Asset);
                    AddAccount(year, "02-150-50-41", "Due To Unrestricted", AccountCategory.Asset);
                    AddAccount(year, "02-150-50-44", "Due From Perm Restricted", AccountCategory.Asset);
                    AddAccount(year, "02-150-50-45", "Due To Perm Restricted", AccountCategory.Asset);

                    AddAccount(year, "03-150-50-40", "Due From Unrestricted", AccountCategory.Asset);
                    AddAccount(year, "03-150-50-41", "Due To Unrestricted", AccountCategory.Asset);
                    AddAccount(year, "03-150-50-42", "Due From Temp Restricted", AccountCategory.Asset);
                    AddAccount(year, "03-150-50-43", "Due To Temp restricted", AccountCategory.Asset);

                    AddAccount(year, "01-150-50-50", "Due From Other Entities", AccountCategory.Asset);
                    AddAccount(year, "01-150-50-51", "Due To Other Entities", AccountCategory.Asset);
                    AddAccount(year, "01-150-50-52", "Due From ABC Entity", AccountCategory.Asset);
                    AddAccount(year, "01-150-50-53", "Due To ABC Entity", AccountCategory.Asset);
                    AddAccount(year, "01-150-50-54", "Due From DEF Entity", AccountCategory.Asset);
                    AddAccount(year, "01-150-50-55", "Due To DEF Entity", AccountCategory.Asset);

                    SetAccountGroups(year, "LIABILITIES", "NOTES", "");
                    AddAccount(year, "01-200-00-20", "Demand Notes", AccountCategory.Liability, -125m);

                    SetAccountGroups(year, "FUND BALANCES", "GENERAL", "");
                    AddAccount(year, "01-310-50-02", "Accumulated Revenue", AccountCategory.Fund, -225m);
                    SetAccountGroups(year, "FUND BALANCES", "TEMPORARY", "");
                    AddAccount(year, "02-380-50-02", "Accumulated Revenue", AccountCategory.Fund);
                    SetAccountGroups(year, "FUND BALANCES", "PERMANENT", "");
                    AddAccount(year, "03-390-50-02", "Accumulated Revenue", AccountCategory.Fund);

                    // Revenue
                    SetAccountGroups(year, "INCOME", "GIFTS", "");
                    AddAccount(year, "01-470-80-10-05", "Undesignated Gifts", AccountCategory.Revenue);
                    AddAccount(year, "02-480-80-10-07", "NCE Endowment ETF Gifts", AccountCategory.Revenue);

                    // Expense
                    SetAccountGroups(year, "EXPENSE", "INTEREST", "");
                    AddAccount(year, "01-500-10-01-02", "Interest on Notes & Balances", AccountCategory.Revenue);

                }
            }

            uow.CreateRepositoryForDataSource(_accounts);
            uow.CreateRepositoryForDataSource(_ledgerAccounts);
            uow.CreateRepositoryForDataSource(_ledgerAccountYears);
            uow.CreateRepositoryForDataSource(_accountPriorYears);
            uow.CreateRepositoryForDataSource(_accountSegments);

            return _accounts;
        }

        private static void AddAccount(FiscalYear year, string accountNumber, string description, AccountCategory category, decimal beginBalance = 0m)
        {
            var ledgerAccount = _ledgerAccounts.FirstOrDefault(p => p.Ledger == year.Ledger && p.AccountNumber == accountNumber);
            if (ledgerAccount == null)
            {
                ledgerAccount = new LedgerAccount()
                {
                    AccountNumber = accountNumber,
                    Name = description,
                    Ledger = year.Ledger,
                    LedgerAccountYears = new List<LedgerAccountYear>(),                    
                    Id = GuidHelper.NewSequentialGuid()
                };

                _ledgerAccounts.Add(ledgerAccount);
                year.Ledger.LedgerAccounts.Add(ledgerAccount);
            }

            var account = new Account()
            {
                AccountNumber = accountNumber,
                Name = description,
                Category = category,
                BeginningBalance = beginBalance,
                FiscalYear = year,
                IsActive = true,
                IsNormallyDebit = (category == AccountCategory.Asset || category == AccountCategory.Expense),
                SortKey = string.Empty,
                Group1 = _accountGroup1,
                Group2 = _accountGroup2,
                Group3 = _accountGroup3,
                AccountBalances = new List<AccountBalance>(),
                Budgets = new List<AccountBudget>(),
                LedgerAccountYears = new List<LedgerAccountYear>(),
                NextYearAccounts = new List<AccountPriorYear>(),
                PriorYearAccounts = new List<AccountPriorYear>(),
                Id = GuidHelper.NewSequentialGuid()
            };

            account.AccountSegments = GetAccountSegments(year, account);

            if (_accountGroup1 != null)
            {
                _accountGroup1.Group1Accounts.Add(account);
            }

            if (_accountGroup2 != null)
            {
                _accountGroup2.Group2Accounts.Add(account);
            }

            if (_accountGroup3 != null)
            {
                _accountGroup3.Group3Accounts.Add(account);
            }

            var accountYear = new LedgerAccountYear()
            {
                Account = account,
                LedgerAccount = ledgerAccount,
                FiscalYear = year,
                Id = GuidHelper.NewSequentialGuid()
            };

            account.LedgerAccountYears.Add(accountYear);
            ledgerAccount.LedgerAccountYears.Add(accountYear);

            _accounts.Add(account);
            _ledgerAccountYears.Add(accountYear);

        }

        private static IList<AccountSegment> GetAccountSegments(FiscalYear year, Account account)
        {
            List<AccountSegment> list = new List<AccountSegment>();

            string[] codes = account.AccountNumber.Split('-');
            Segment parentSegment = null;
            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < codes.Length; index++)
            {
                string code = codes[index];
                if (index > 0)
                {
                    sb.Append(_segmentLevels[index - 1].Separator);
                }
                sb.Append(code);

                Segment segment = null;

                if (_segmentLevels[index].IsLinked)
                {
                    if (parentSegment != null)
                    {
                        segment = parentSegment.ChildSegments?.FirstOrDefault(p => p.Code == code)
                                    ??
                                    _segments.FirstOrDefault(p => p.ParentSegmentId == parentSegment.Id && p.Code == code);
                    }
                }
                else
                {
                    parentSegment = null;
                    segment = _segments.FirstOrDefault(p => p.Level == index + 1 && p.FiscalYearId == year.Id && p.Code == code);
                }

                if (segment != null)
                {
                    var accountSegment = new AccountSegment()
                    {
                        Segment = segment,
                        Level = segment.Level,
                        Account = account
                    };
                    list.Add(accountSegment);
                    _accountSegments.Add(accountSegment);
                }

                parentSegment = segment;
            }

            // Reformat account number using correct separators
            account.AccountNumber = sb.ToString();

            return list;
        }

        private static void SetAccountGroups(FiscalYear year, string group1, string group2, string group3)
        {
            if (string.IsNullOrWhiteSpace(group1))
            {
                _accountGroup1 = null;
            }
            else
            {
                _accountGroup1 = _accountGroups.FirstOrDefault(p => p.FiscalYear == year && p.Name == group1);
            }

            if (string.IsNullOrWhiteSpace(group2))
            {
                _accountGroup2 = null;
            }
            else
            {
                _accountGroup2 = _accountGroups.FirstOrDefault(p => p.FiscalYear == year && p.Name == group2);
            }

            if (string.IsNullOrWhiteSpace(group3))
            {
                _accountGroup3 = null;
            }
            else
            {
                _accountGroup3 = _accountGroups.FirstOrDefault(p => p.FiscalYear == year && p.Name == group3);
            }


        }

    }

    
}
