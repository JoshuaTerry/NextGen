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
    public static class AccountBalanceDataSource
    {
        private static IList<FiscalYear> _years;
        private static IList<Account> _accounts;
        private static IList<AccountBalance> _accountBalances;

        public const decimal TOTAL_DEBITS = 5000m;
        public const decimal TOTAL_CREDITS = 3000m;
        public const string ACCOUNT_NUMBER = "01-100-10-01";

        public static IList<AccountBalance> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<AccountBalance> existing = uow.GetRepositoryOrNull<AccountBalance>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            _years = FiscalYearDataSource.GetDataSource(uow);
            _accounts = AccountDataSource.GetDataSource(uow);
            _accountBalances = new List<AccountBalance>();

            // Balances for open year
            var year = _years.FirstOrDefault(p => p.Name == FiscalYearDataSource.OPEN_YEAR && p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1);
            var account = _accounts.FirstOrDefault(p => p.FiscalYear == year && p.AccountNumber == ACCOUNT_NUMBER);

            CreateAccountBalances(account);

            // Balances for closed (prior) year
            year = _years.FirstOrDefault(p => p.Name == FiscalYearDataSource.CLOSED_YEAR && p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1);
            account = _accounts.FirstOrDefault(p => p.FiscalYear == year && p.AccountNumber == ACCOUNT_NUMBER);

            CreateAccountBalances(account);

            uow.CreateRepositoryForDataSource(_accountBalances);
     
            return _accountBalances;
        }

        private static void CreateAccountBalances(Account account)
        {
            decimal totalDebits = 0, totalCredits = 0;
            Random random = new Random();

            for (int period = 1; period <= account.FiscalYear.NumberOfPeriods; period++)
            {
                var debitBalance = new AccountBalance()
                {
                    Account = account,
                    Id = account.Id,
                    DebitCredit = "DB",
                    PeriodNumber = period,
                };

                var creditBalance = new AccountBalance()
                {
                    Account = account,
                    Id = account.Id,
                    DebitCredit = "CR",
                    PeriodNumber = period,
                };

                if (period < account.FiscalYear.NumberOfPeriods)
                {
                    debitBalance.TotalAmount = random.Next(10000, 38000) / 100.0m;
                    creditBalance.TotalAmount = random.Next(10000, 23000) / 100.0m;
                    totalDebits += debitBalance.TotalAmount;
                    totalCredits += creditBalance.TotalAmount;
                }
                else
                {
                    debitBalance.TotalAmount = TOTAL_DEBITS - totalDebits;
                    creditBalance.TotalAmount = TOTAL_CREDITS - totalCredits;
                }

                _accountBalances.Add(debitBalance);
                _accountBalances.Add(creditBalance);
            }

        }
       
        

    }

    
}
