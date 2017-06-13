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
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == Shared.Enums.GL.FiscalYearStatus.Closed);
            FiscalPeriod period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 4);

            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.CloseFiscalPeriod(period.Id), UserMessagesGL.FiscalYearClosed,
                "Closing a period in a closed fiscal year should throw exception.");

            // Try to close a closed period in an open fiscal year
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == Shared.Enums.GL.FiscalYearStatus.Open);
            period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == FiscalYearDataSource.CURRENT_PERIOD - 1);

            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.CloseFiscalPeriod(period.Id), UserMessagesGL.FiscalPeriodClosed,
                "Closing a closed fiscal period should throw exception.");

            // Closing an open period
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == Shared.Enums.GL.FiscalYearStatus.Open);
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
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == Shared.Enums.GL.FiscalYearStatus.Closed);
            FiscalPeriod period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 4);

            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.ReopenFiscalPeriod(period.Id), UserMessagesGL.FiscalYearClosed,
                "Reopening a period in a closed fiscal year should throw exception.");

            // Try to reopen an open period in an open fiscal year
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == Shared.Enums.GL.FiscalYearStatus.Open);
            period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == FiscalYearDataSource.CURRENT_PERIOD);

            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.ReopenFiscalPeriod(period.Id), UserMessagesGL.FiscalPeriodOpen,
                "Reopening an open fiscal period should throw exception.");

            // Reopening a closed period
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Status == Shared.Enums.GL.FiscalYearStatus.Open);
            period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 1);

            AssertNoException(() => _bl.ReopenFiscalPeriod(period.Id), "Reopening a closed period should succeed.");
            Assert.IsTrue(year.FiscalPeriods.All(p => p.Status == FiscalPeriodStatus.Open),
                $"All fiscal periods should be open. ");
            Assert.AreEqual(1, year.CurrentPeriodNumber, $"Fiscal year current period number was updated to 1.");
        }

    }
}