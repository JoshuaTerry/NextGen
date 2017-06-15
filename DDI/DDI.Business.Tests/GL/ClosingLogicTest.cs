using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Business.Tests.GL.DataSources;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.GL
{
    [TestClass]
    public class ClosingLogicTest : TestBase
    {
        private const string TESTDESCR = "Business | GL";
        private IUnitOfWork _uow;
        private ClosingLogic _bl;
        private IList<BusinessUnit> _units;
        private IList<Ledger> _ledgers;
        private IList<FiscalYear> _fiscalYears;
        private const string INVALID_DATE = "1/1/1970";

        ITestRepository<FiscalYear> _fiscalYearRepo;
        ITestRepository<FiscalPeriod> _fiscalPeriodRepo;

        [TestInitialize]
        public void Initialize()
        {
            Factory.ConfigureForTesting();

            _uow = Factory.CreateUnitOfWork();

            _units = BusinessUnitDataSource.GetDataSource(_uow);
            _ledgers = LedgerDataSource.GetDataSource(_uow);
            _fiscalYears = FiscalYearDataSource.GetDataSource(_uow);

            _bl = new ClosingLogic(_uow);
            _fiscalYearRepo = _uow.GetRepository<FiscalYear>() as ITestRepository<FiscalYear>;
            _fiscalPeriodRepo = _uow.GetRepository<FiscalPeriod>() as ITestRepository<FiscalPeriod>;
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void ClosingLogic_CloseFiscalPeriod()
        {

            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.CloseFiscalPeriod(Guid.Empty), UserMessagesGL.BadFiscalPeriod,
                "Non-existent period should throw exception.");

            // Try to close a period for a closed fiscal year
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == FiscalYearStatus.Closed);
            FiscalPeriod period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 4);

            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.CloseFiscalPeriod(period.Id), UserMessagesGL.FiscalYearClosed,
                "Closing a period in a closed fiscal year should throw exception.");

            // Try to close a closed period in an open fiscal year
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == FiscalYearStatus.Open);
            period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == FiscalYearDataSource.CURRENT_PERIOD - 1);

            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.CloseFiscalPeriod(period.Id), UserMessagesGL.FiscalPeriodClosed,
                "Closing a closed fiscal period should throw exception.");

            // Closing an open period
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == FiscalYearStatus.Open);
            period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == FiscalYearDataSource.CURRENT_PERIOD + 1);

            AssertNoException(() => _bl.CloseFiscalPeriod(period.Id), "Closing an open period should succeed.");
            Assert.IsTrue(year.FiscalPeriods.Where(p => p.PeriodNumber <= period.PeriodNumber).All(p => p.Status == FiscalPeriodStatus.Closed),
                $"All fiscal periods thru period {period.PeriodNumber} should be closed. ");
            Assert.AreEqual(period.PeriodNumber + 1, year.CurrentPeriodNumber, $"Fiscal year current period number was updated to {period.PeriodNumber + 1}.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ClosingLogic_ReopenFiscalPeriod()
        {

            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.ReopenFiscalPeriod(Guid.Empty), UserMessagesGL.BadFiscalPeriod,
                "Non-existent period should throw exception.");

            // Try to reopen a period for a closed fiscal year
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == FiscalYearStatus.Closed);
            FiscalPeriod period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 4);

            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.ReopenFiscalPeriod(period.Id), UserMessagesGL.FiscalYearClosed,
                "Reopening a period in a closed fiscal year should throw exception.");

            // Try to reopen an open period in an open fiscal year
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == FiscalYearStatus.Open);
            period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == FiscalYearDataSource.CURRENT_PERIOD);

            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.ReopenFiscalPeriod(period.Id), UserMessagesGL.FiscalPeriodOpen,
                "Reopening an open fiscal period should throw exception.");

            // Reopening a closed period
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == FiscalYearStatus.Open);
            period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 1);

            AssertNoException(() => _bl.ReopenFiscalPeriod(period.Id), "Reopening a closed period should succeed.");
            Assert.IsTrue(year.FiscalPeriods.All(p => p.Status == FiscalPeriodStatus.Open),
                $"All fiscal periods should be open. ");
            Assert.AreEqual(1, year.CurrentPeriodNumber, $"Fiscal year current period number was updated to 1.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ClosingLogic_ReopenFiscalYear()
        {
            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.ReopenFiscalYear(Guid.Empty), UserMessagesGL.BadFiscalYear,
                "Non-existent year should throw exception.");

            // Try to reopen an open fiscal year.
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == FiscalYearStatus.Open);
            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.ReopenFiscalYear(year.Id), UserMessagesGL.FiscalYearOpen,
    "Reopening an open fiscal year should throw exception.");

            // Reopen a closed year.
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == FiscalYearStatus.Closed);

            AssertNoException(() => _bl.ReopenFiscalYear(year.Id), "Reopening a closed year should succeed.");

            Assert.AreEqual(FiscalYearStatus.Reopened, year.Status, "Fiscal year status is Reopened.");
            Assert.AreEqual(12, year.CurrentPeriodNumber, "Current period is 12.");
            FiscalPeriod period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 12);
            Assert.AreEqual(FiscalPeriodStatus.Open, period.Status, "Period 12 status is open.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ClosingLogic_CloseFiscalYear()
        {
            IList<Account> accounts = AccountDataSource.GetDataSource(_uow);
            PostedTransactionDataSource.GetDataSource(_uow);
            FundDataSource.GetDataSource(_uow);

            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == FiscalYearStatus.Open);

            // Get the next fiscal year.
            FiscalYear nextYear = _uow.GetBusinessLogic<FiscalYearLogic>().GetNextFiscalYear(year);

            // First we must blow away any beginning balances in the next fiscal year.
            foreach (var account in _uow.Where<Account>(p => p.FiscalYear == nextYear))
            {
                account.BeginningBalance = 0m;
            }

            // Close the open year.
            _bl.CloseFiscalYear(year.Id);

            // Need to fixup PostedTransactions that were created
            foreach (var tran in _uow.Where<PostedTransaction>(p => p.LedgerAccountYear == null))
            {
                tran.LedgerAccountYear = _uow.GetById<LedgerAccountYear>(tran.LedgerAccountYearId.Value);
                tran.FiscalYear = _uow.GetById<FiscalYear>(tran.FiscalYearId.Value);
            }

            // Recalculate the AccountBalance "view" datasource.
            AccountBalanceDataSource.CalculateDataSourceForTransactions(_uow);

            // Get the transactions for the now-closed year.
            var trans = _uow.Where<PostedTransaction>(p => p.FiscalYear == year);

            // Verify that there are CloseFrom, CloseTo, and EndBal transactions.
            Assert.IsTrue(trans.Where(p => p.PostedTransactionType == PostedTransactionType.CloseFrom).Count() > 0, "CloseFrom transactions created.");
            Assert.IsTrue(trans.Where(p => p.PostedTransactionType == PostedTransactionType.CloseTo).Count() > 0, "CloseTo transactions created.");
            Assert.IsTrue(trans.Where(p => p.PostedTransactionType == PostedTransactionType.EndBal).Count() > 0, "EndBal transactions created.");

            // Verify that the new year has BeginBal transactions.
            Assert.IsTrue(_uow.Where<PostedTransaction>(p => p.FiscalYear == nextYear && p.PostedTransactionType == PostedTransactionType.BeginBal).Count() > 0, "BeginBal transactions created.");

            // Balance the balance sheet accounts:  Sum their beginning balances and all transactions (excluding EndBal & BeginBal transactions).
            decimal balance = accounts.Where(p => p.FiscalYear == year &&
            (p.Category == AccountCategory.Asset || p.Category == AccountCategory.Liability || p.Category == AccountCategory.Fund))
            .Select(p => p.BeginningBalance + (
                p.LedgerAccountYears
                 .Join(trans, outer => outer.Id, inner => inner.LedgerAccountYearId, (outer, inner) => inner)
                 .Where(t => t.PostedTransactionType != PostedTransactionType.BeginBal && t.PostedTransactionType != PostedTransactionType.EndBal)
                 .Sum(t => t.Amount)))
            .Sum(p => p);

            Assert.AreEqual(0m, balance, "Balance sheet accounts now sum to zero.");

            // All accounts should sum to zero when EndBal transactions are included.
            Assert.IsTrue(_uow.Where<Account>(p => p.FiscalYear == year)
                              .All(p => p.BeginningBalance +
                                        (p.LedgerAccountYears
                                          .Join(trans, outer => outer.Id, inner => inner.LedgerAccountYearId, (outer, inner) => inner)
                                          .Where(t => t.PostedTransactionType != PostedTransactionType.BeginBal)
                                          .Sum(t => t.Amount))
                                   == 0), "All accounts sum to zero.");

            bool isEqual = true;
            AccountLogic accountLogic = _uow.GetBusinessLogic<AccountLogic>();

            // Use the GetAccountActivity logic to calculate the balances and activity for each account in the new year.
            // Final period ending balance for all accounts should equal the beginning balance for the account in the next year.
            foreach (var account in _uow.Where<Account>(p => p.FiscalYear == nextYear))
            {
                var activity = accountLogic.GetAccountActivity(account);
                isEqual &= account.BeginningBalance == activity.Detail.OrderByDescending(p => p.PeriodNumber).First().PriorEndingBalance;
            }
            Assert.IsTrue(isEqual, "Beginning balance and prior year ending balance are equal.");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ClosingLogic_CreateNewFiscalYear()
        {
            IList<Account> accounts = AccountDataSource.GetDataSource(_uow);
            PostedTransactionDataSource.GetDataSource(_uow);
            FundDataSource.GetDataSource(_uow);

            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == FiscalYearStatus.Empty);
            int newYearNumber = int.Parse(year.Name) + 1;
            string newYearName = newYearNumber.ToString();

            // Find an account and flag it as inactive.
            Account inactiveAccount = accounts.FirstOrDefault(p => p.FiscalYear == year && p.AccountNumber == "02-480-80-10-07");
            inactiveAccount.IsActive = false;

            // Create the new fiscal year
            _bl.CreateNewFiscalYear(year.Id, newYearName, new DateTime(newYearNumber, 1, 1), false);

            // Get the new year in UNIT_CODE1
            FiscalYear newYear = _uow.FirstOrDefault<FiscalYear>(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == newYearName);

            // Must populate the new year's fiscal period collection (because we're not using EF.)
            newYear.FiscalPeriods = _uow.Where<FiscalPeriod>(p => p.FiscalYear == newYear).ToList();

            // ASSERTIONS

            // Determine the list of ledgers that should now have the fiscal year.
            IEnumerable<Ledger> ledgers = _uow.Where<Ledger>(p => p.BusinessUnit.BusinessUnitType != BusinessUnitType.Separate);

            Assert.AreEqual(ledgers.Count(), 
                            _uow.Where<FiscalYear>(p => p.Name == newYearName).Count(), 
                            "Each common ledger has the new fiscal year.");

            Assert.AreEqual(ledgers.Count() * year.FiscalPeriods.Count(),
                            _uow.Where<FiscalPeriod>(p => p.FiscalYear.Name == newYearName).Count(),
                            "Each common ledger has the new fiscal periods.");


            // Ensure the fiscal period dates are all in the new year
            Assert.IsTrue(newYear.FiscalPeriods.All(p => p.StartDate.Value.Year == newYearNumber && p.EndDate.Value.Year == newYearNumber), 
                $"Each fiscal period in the new year is in {newYearNumber}.");

            // Ensure all active accounts were copied.
            Assert.AreEqual(_uow.Where<Account>(p => p.FiscalYearId == year.Id & p.IsActive).Count(),
                            _uow.Where<Account>(p => p.FiscalYearId == newYear.Id).Count(),
                            "Each account was copied to the new year.");

            // Ensure all account groups were copied.
            Assert.AreEqual(_uow.Where<AccountGroup>(p => p.FiscalYearId == year.Id).Count(),
                            _uow.Where<AccountGroup>(p => p.FiscalYearId == newYear.Id).Count(),
                            "Each account group was copied to the new year.");

            // Ensure all segments were copied.
            Assert.AreEqual(_uow.Where<Segment>(p => p.FiscalYearId == year.Id).Count(),
                            _uow.Where<Segment>(p => p.FiscalYearId == newYear.Id).Count(),
                            "Each segment was copied to the new year.");

            // Ensure all funds were copied.
            Assert.AreEqual(_uow.Where<Fund>(p => p.FiscalYearId == year.Id).Count(),
                            _uow.Where<Fund>(p => p.FiscalYearId == newYear.Id).Count(),
                            "Each fund was copied to the new year.");

            // Ensure all FundFromTo's were copied.
            Assert.AreEqual(_uow.Where<FundFromTo>(p => p.FiscalYearId == year.Id).Count(),
                            _uow.Where<FundFromTo>(p => p.FiscalYearId == newYear.Id).Count(),
                            "Each FundFromTo was copied to the new year.");

            // Ensure all BusinessUnitFromTo's were copied.
            Assert.AreEqual(_uow.Where<BusinessUnitFromTo>(p => p.FiscalYearId == year.Id).Count(),
                            _uow.Where<BusinessUnitFromTo>(p => p.FiscalYearId == newYear.Id).Count(),
                            "Each BusinessUnitFromTo was copied to the new year.");
        }


    }
}