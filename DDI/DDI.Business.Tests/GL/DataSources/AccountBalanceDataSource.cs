using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Shared;
using DDI.Shared.Enums.GL;
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

        public static IList<AccountBalance> CalculateDataSourceForTransactions(IUnitOfWork uow)
        {
            var balances = new List<AccountBalance>();

            // Build the AccountBalances "view" data
            foreach (var tran in uow.Where<PostedTransaction>(p => p.PostedTransactionType != PostedTransactionType.BeginBal && p.PostedTransactionType != PostedTransactionType.EndBal))
            {
                string dbcr = tran.Amount > 0m ? "DB" : "CR";

                var balance = balances.FirstOrDefault(p => p.Id == tran.LedgerAccountYear.Account.Id && p.PeriodNumber == tran.PeriodNumber && p.DebitCredit == dbcr);
                if (balance == null)
                {
                    balance = new AccountBalance
                    {
                        Id = tran.LedgerAccountYear.Account.Id,
                        Account = tran.LedgerAccountYear.Account,
                        PeriodNumber = tran.PeriodNumber,
                        DebitCredit = dbcr
                    };
                    balances.Add(balance);
                }

                balance.TotalAmount += Math.Abs(tran.Amount);
            }

            uow.CreateRepositoryForDataSource(balances);

            foreach (var account in uow.GetEntities<Account>())
            {
                account.AccountBalances = balances.Where(p => p.Account == account).ToList();
            }

            return balances;
        }

        public static IList<AccountBalance> GetDataSource(IUnitOfWork uow)
        {
            IList<AccountBalance> existing = uow.GetRepositoryDataSource<AccountBalance>();
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

            account.AccountBalances = new List<AccountBalance>();

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
                account.AccountBalances.Add(debitBalance);
                account.AccountBalances.Add(creditBalance);
            }

        }
       
        

    }

    
}
