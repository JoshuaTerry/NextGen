using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Business.Tests.GL.DataSources;
using DDI.Shared;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.GL
{
    [TestClass]
    public class PostingLogicTest : TestBase
    {
        private const string TESTDESCR = "Business | GL";
        private IUnitOfWork _uow;
        private PostingLogic _bl;
        private IList<FiscalYear> _fiscalYears;
        private IList<Transaction> _trans;
        private const string INVALID_DATE = "1/1/1970";

        private IList<PostedTransaction> _postedTrans;

        [TestInitialize]
        public void Initialize()
        {
            Factory.ConfigureForTesting();

            _uow = Factory.CreateUnitOfWork();

            BusinessUnitDataSource.GetDataSource(_uow);
            LedgerDataSource.GetDataSource(_uow);
            _fiscalYears = FiscalYearDataSource.GetDataSource(_uow);
            AccountDataSource.GetDataSource(_uow);
            FundDataSource.GetDataSource(_uow);

            _postedTrans = new List<PostedTransaction>();
            _uow.CreateRepositoryForDataSource(_postedTrans);

            _bl = _uow.GetBusinessLogic<PostingLogic>();
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void PostTransaction_Number()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            string description = "Test";
            DateTime postDate = DateTime.Now;

            _trans = new List<Transaction>();

            CreateTransaction(123.45m, "01-100-10-10", "01-200-00-20", year, 1, 1, description);
            CreateTransaction(234.00m, "01-100-10-10", "01-470-80-10-05", year, 1, 2, description);

            _uow.CreateRepositoryForDataSource(_trans);
                        
            _bl.PostTransaction(1);

            Assertions(year, postDate);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void PostTransaction_Enumerable()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            string description = "Test";
            DateTime postDate = DateTime.Now;

            _trans = new List<Transaction>();

            CreateTransaction(123.45m, "01-100-10-10", "01-200-00-20", year, 1, 1, description);
            CreateTransaction(234.00m, "01-100-10-10", "01-470-80-10-05", year, 4, 1, description);

            _uow.CreateRepositoryForDataSource(_trans);

            _bl.PostTransaction(_trans);

            Assertions(year, postDate);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void PostTransaction_Exceptions()
        {
            // Try posting to a closed fiscal year.

            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.CLOSED_YEAR);
            string description = "Test";
            DateTime postDate = DateTime.Now;

            _trans = new List<Transaction>();

            CreateTransaction(123.45m, "01-100-10-10", "01-200-00-20", year, 1, 1, description);

            _uow.CreateRepositoryForDataSource(_trans);
            AssertThrowsExceptionMessageContains<Exception>(() => _bl.PostTransaction(1), UserMessagesGL.FiscalYearClosed);

            // Try posting to a closed fiscal period.
            _trans.Clear();

            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            CreateTransaction(123.45m, "01-100-10-10", "01-200-00-20", year, 1, 1, description);
            Transaction tran = _trans.First();
            tran.TransactionDate = year.StartDate;

            _uow.CreateRepositoryForDataSource(_trans);
            AssertThrowsExceptionMessageContains<Exception>(() => _bl.PostTransaction(1), UserMessagesGL.FiscalPeriodClosed);

            // Null tran date
            tran.TransactionDate = null;
            AssertThrowsExceptionMessageContains<Exception>(() => _bl.PostTransaction(1), UserMessages.TranNoTranDate);

            // Date not valid for fiscal year
            tran.TransactionDate = DateTime.Parse(INVALID_DATE);
            AssertThrowsExceptionMessageContains<Exception>(() => _bl.PostTransaction(1), UserMessagesGL.NoFiscalPeriodForDate);

            // Setup a test for amount imbalance.
            _trans.Clear();
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            CreateTransaction(50.00m, "01-100-10-10", "", year, 1, 1, description);
            CreateTransaction(-60.00m, "01-200-00-20", "", year, 1, 2, description);

            _uow.CreateRepositoryForDataSource(_trans);
            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.PostTransaction(1), UserMessages.TranImbalance);

            // Test for business unit imbalance
            _trans.Clear();
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            CreateTransaction(50.00m, "01-100-10-10", "", year, 1, 1, description);

            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE2 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            CreateTransaction(-50.00m, "01-200-00-20", "", year, 1, 2, description);

            _uow.CreateRepositoryForDataSource(_trans);
            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.PostTransaction(1), UserMessages.TranImbalanceForBU);

            // Test for fund imbalance
            _trans.Clear();
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            CreateTransaction(50.00m, "01-100-10-10", "", year, 1, 1, description);
            CreateTransaction(-50.00m, "02-100-10-10", "", year, 1, 2, description);

            _uow.CreateRepositoryForDataSource(_trans);
            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.PostTransaction(1), UserMessages.TranImbalanceForFund);
        }


        private void Assertions(FiscalYear year, DateTime postDate)
        {
            _postedTrans = _uow.GetEntities<PostedTransaction>().ToList();

            // Assign Transaction based on TransactionId.
            _postedTrans.ForEach(p => p.Transaction = _trans.FirstOrDefault(t => t.Id == p.TransactionId));

            Assert.AreEqual(_trans.Count * 2, _postedTrans.Count, $"{_trans.Count * 2} posted transactions were created.");
            Assert.IsTrue(_trans.All(p => p.Status == TransactionStatus.Posted), "All transactions have status of Posted.");
            Assert.IsTrue(_trans.All(p => p.PostDate >= postDate), "All transactions have a valid post date.");

            Assert.IsTrue(_postedTrans.All(p => p.Transaction != null), "All posted transactions have a transaction.");
            Assert.IsTrue(_postedTrans.All(p => p.TransactionDate == p.Transaction.TransactionDate), "Transaction date assigned correctly.");
            Assert.IsTrue(_postedTrans.All(p => p.TransactionNumber == p.Transaction.TransactionNumber), "Transaction number assigned correctly.");
            Assert.IsTrue(_postedTrans.All(p => p.Description == p.Transaction.Description), "Description assigned correctly.");
            Assert.IsTrue(_postedTrans.All(p => p.TransactionType == p.Transaction.TransactionType), "Transaction type assigned correctly.");
            Assert.IsTrue(_postedTrans.All(p => p.PostedTransactionType == PostedTransactionType.Actual), "Posted transaction type set to Actual.");
            Assert.IsTrue(_postedTrans.All(p => p.FiscalYearId == year.Id), "Fiscal year Id assigned correctly.");

            // Debit trans should referece the debit account
            Assert.IsTrue(_postedTrans.Where(p => p.Amount > 0m).All(p => p.LedgerAccountYearId == p.Transaction.DebitAccountId), "Debit posted trans reference the original debit account.");

            // Credit trans should referece the debit account
            Assert.IsTrue(_postedTrans.Where(p => p.Amount < 0m).All(p => p.LedgerAccountYearId == p.Transaction.CreditAccountId), "Credit posted trans reference the original credit account.");

            // Debit trans reference the transaction amount
            Assert.IsTrue(_postedTrans.Where(p => p.Amount > 0m).All(p => p.Amount == p.Transaction.Amount), "Debit posted trans reference the original tran amount.");

            // Credit trans reference the negated transaction amount
            Assert.IsTrue(_postedTrans.Where(p => p.Amount < 0m).All(p => p.Amount == -p.Transaction.Amount), "Credit posted trans reference the negated tran amount.");
        }

        private void CreateTransaction(decimal amount, string debitAcct, string creditAcct, FiscalYear year, Int64 transactionNumber, int lineNumber, string description)
        {

            var tran = new Transaction()
            {
                Amount = amount,
                DebitAccount = string.IsNullOrEmpty(debitAcct) ? null :  year.LedgerAccounts.First(p => p.Account.AccountNumber == debitAcct),
                CreditAccount = string.IsNullOrEmpty(creditAcct) ? null : year.LedgerAccounts.First(p => p.Account.AccountNumber == creditAcct),
                FiscalYear = year,
                TransactionNumber = transactionNumber,
                LineNumber = lineNumber,
                Status = TransactionStatus.Unposted,
                TransactionType = TransactionType.None,
                Description = description,
            };

            if (year.Status == FiscalYearStatus.Open)
            {
                tran.TransactionDate = year.FiscalPeriods.First(p => p.Status == FiscalPeriodStatus.Open).StartDate;
            }
            else
            {
                tran.TransactionDate = year.StartDate;
            }

            tran.FiscalYearId = tran.FiscalYear.Id;
            tran.DebitAccountId = tran.DebitAccount?.Id;
            tran.CreditAccountId = tran.CreditAccount?.Id;
            tran.AssignPrimaryKey();
            _trans.Add(tran);

        }

    }
}