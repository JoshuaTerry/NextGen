using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDI.Business.GL;
using DDI.Shared;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;
using DDI.Shared.Extensions;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Tests.GL.DataSources
{
    public static class PostedTransactionDataSource
    {
        private static IList<FiscalYear> _years;
        private static IList<Account> _accounts;        
        private static IList<PostedTransaction> _trans;
        private static long _transactionNumber = 0;
        private static Random _random;

        public static IList<PostedTransaction> GetDataSource(IUnitOfWork uow)
        {
            IList<PostedTransaction> existing = uow.GetRepositoryDataSource<PostedTransaction>();
            if (existing != null)
            {
                return existing;
            }

            _years = FiscalYearDataSource.GetDataSource(uow);
            _accounts = AccountDataSource.GetDataSource(uow);

            _trans = new List<PostedTransaction>();

            _random = new Random();
            var repeater = Enumerable.Range(1, 10);

            foreach (var year in _years.Where(p => p.Ledger.BusinessUnit.BusinessUnitType == BusinessUnitType.Common && p.Status != FiscalYearStatus.Empty))
            {
                repeater.ForEach(p =>
                    CreateTransaction(year, "01-100-10-10", "01-470-80-10-05", $"Gift #{p}", TransactionType.FRDGConverted));

                repeater.ForEach(p =>
                    CreateTransaction(year, "02-100-10-10", "02-480-80-10-07", $"NCE Gift #{p}", TransactionType.FRDGConverted));

                repeater.ForEach(p =>
                    CreateTransaction(year, "01-100-10-10", "01-200-00-20", "Investment deposit", TransactionType.INConverted));

                repeater.ForEach(p =>
                    CreateTransaction(year, "01-500-10-01-02", "01-200-00-20", "Investment interest", TransactionType.INConverted));

                decimal amt = CreateTransaction(year, "01-100-10-10", null, "Cash transfer", TransactionType.Journal);
                CreateTransaction(year, "02-100-10-10", null, "Cash transfer", TransactionType.Journal, -amt);
                CreateTransaction(year, "02-150-50-40", null, "Cash transfer", TransactionType.Journal, amt);
                CreateTransaction(year, "01-150-50-43", null, "Cash transfer", TransactionType.Journal, -amt);
            }

            uow.CreateRepositoryForDataSource(_trans);
            AccountBalanceDataSource.CalculateDataSourceForTransactions(uow);

            return _trans;

        }

        private static decimal CreateTransaction(FiscalYear year, string debitAccount, string creditAccount, string description, TransactionType type, decimal amount = 0m)
        {
            if (amount == 0m)
            {
                double factor = 10.0;
                int power = _random.Next(0, 5);
                while (power-- > 0)
                {
                    factor *= 10.0;
                }
                amount = (decimal)Math.Round(_random.NextDouble() * factor, 2);
                ++_transactionNumber;
            }
            DateTime tranDate;
            int periodNumber;

            while (true) {
                tranDate = year.StartDate.Value.AddDays(_random.Next(365));
                var period = year.FiscalPeriods.FirstOrDefault(p => p.StartDate <= tranDate && p.EndDate >= tranDate);
                if (period != null)
                {
                    periodNumber = period.PeriodNumber;
                    break;
                }
            }

            Account account = _accounts.FirstOrDefault(p => p.FiscalYear == year && p.AccountNumber == debitAccount);

            _trans.Add(new PostedTransaction()
            {
                FiscalYear = year,
                Description = description,
                TransactionNumber = _transactionNumber,
                LineNumber = 1,
                TransactionDate = tranDate,
                PeriodNumber = periodNumber,
                TransactionType = type,
                PostedTransactionType = PostedTransactionType.Actual,
                Amount = amount,
                LedgerAccountYear = account.LedgerAccountYears.First(),
                Id = GuidHelper.NewSequentialGuid()
            });


            if (!string.IsNullOrWhiteSpace(creditAccount))
            {
                account = _accounts.FirstOrDefault(p => p.FiscalYear == year && p.AccountNumber == creditAccount);

                _trans.Add(new PostedTransaction()
                {
                    FiscalYear = year,
                    Description = description,
                    TransactionNumber = _transactionNumber,
                    LineNumber = 2,
                    TransactionDate = tranDate,
                    PeriodNumber = periodNumber,
                    TransactionType = type,
                    PostedTransactionType = PostedTransactionType.Actual,
                    Amount = -amount,
                    LedgerAccountYear = account.LedgerAccountYears.First(),
                    Id = GuidHelper.NewSequentialGuid()
                });
            }

            return amount;
        }
    }

   
}