using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Business.Tests.GL.DataSources;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.GL
{
    [TestClass]
    public class FiscalYearLogicTest : TestBase
    {
        private const string TESTDESCR = "Business | GL";
        private UnitOfWorkNoDb _uow;
        private FiscalYearLogic _bl;
        private IList<BusinessUnit> _units;
        private IList<Ledger> _ledgers;
        private IList<FiscalYear> _fiscalYears;
        private const string INVALID_DATE = "1/1/1970";
                
        RepositoryNoDb<FiscalYear> _fiscalYearRepo;
        RepositoryNoDb<FiscalPeriod> _fiscalPeriodRepo;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

            _units = BusinessUnitDataSource.GetDataSource(_uow);
            _ledgers = LedgerDataSource.GetDataSource(_uow);
            _fiscalYears = FiscalYearDataSource.GetDataSource(_uow);

            _bl = new FiscalYearLogic(_uow);
            _fiscalYearRepo = _uow.GetRepository<FiscalYear>() as RepositoryNoDb<FiscalYear>;
            _fiscalPeriodRepo = _uow.GetRepository<FiscalPeriod>() as RepositoryNoDb<FiscalPeriod>;
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetCachedFiscalYear()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1);
            Assert.AreEqual(year.Id, _bl.GetCachedFiscalYear(year.Id)?.Id, "GetCachedFiscalYear returns fiscal year.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetFiscalPeriodStartDates()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1);
            DateTime?[] dates = _bl.GetFiscalPeriodStartDates(year);
            Assert.AreEqual(year.NumberOfPeriods, dates.Length, "Dates array has correct length.");
            CollectionAssert.AreEquivalent(year.FiscalPeriods.OrderBy(p => p.PeriodNumber).Select(p => p.StartDate).ToList(), dates, "Dates array has correct dates.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetFiscalPeriodEndDates()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1);
            DateTime?[] dates = _bl.GetFiscalPeriodEndDates(year);
            Assert.AreEqual(year.NumberOfPeriods, dates.Length, "Dates array has correct length.");
            CollectionAssert.AreEquivalent(year.FiscalPeriods.OrderBy(p => p.PeriodNumber).Select(p => p.EndDate).ToList(), dates, "Dates array has correct dates.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetFiscalYear()
        {
            // Via ledger
            Ledger ledger = _ledgers.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE1);
            FiscalYear year = _bl.GetFiscalYear(ledger, DateTime.Parse($"7/1/{FiscalYearDataSource.OPEN_YEAR}"));
            Assert.IsNotNull(year, "GetFiscalYear returned non-null value.");
            Assert.AreEqual(FiscalYearDataSource.OPEN_YEAR, year.Name, "Fiscal year name is correct.");
            Assert.AreEqual(ledger, year.Ledger, "Fiscal year ledger is correct.");

            // Via business unit
            year = _bl.GetFiscalYear(ledger.BusinessUnit, DateTime.Parse($"7/1/{FiscalYearDataSource.CLOSED_YEAR}"));
            Assert.IsNotNull(year, "GetFiscalYear returned non-null value.");
            Assert.AreEqual(FiscalYearDataSource.CLOSED_YEAR, year.Name, "Fiscal year name is correct.");
            Assert.AreEqual(ledger, year.Ledger, "Fiscal year ledger is correct.");

            // Via ledger id
            year = _bl.GetFiscalYear(ledger.Id, DateTime.Parse($"7/1/{FiscalYearDataSource.EMPTY_YEAR}"));
            Assert.IsNotNull(year, "GetFiscalYear returned non-null value.");
            Assert.AreEqual(FiscalYearDataSource.EMPTY_YEAR, year.Name, "Fiscal year name is correct.");
            Assert.AreEqual(ledger, year.Ledger, "Fiscal year ledger is correct.");

            // Null date, no year for date
            Assert.IsNull(_bl.GetFiscalYear(ledger, null), "Null date returns null");
            Assert.IsNull(_bl.GetFiscalYear(ledger, DateTime.Parse(INVALID_DATE)), "Invalid date returns null");
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetFiscalPeriod()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);

            // Via date
            FiscalPeriod period = _bl.GetFiscalPeriod(year, DateTime.Parse($"7/1/{FiscalYearDataSource.OPEN_YEAR}"));
            Assert.IsNotNull(period, "GetFiscalPeriod returned non-null value.");
            Assert.AreEqual(7, period.PeriodNumber, "Period number is correct.");
            Assert.AreEqual(year, period.FiscalYear, "Year is correct.");

            // Via period number
            period = _bl.GetFiscalPeriod(year, 3);
            Assert.IsNotNull(period, "GetFiscalPeriod returned non-null value.");
            Assert.AreEqual(3, period.PeriodNumber, "Period number is correct.");
            Assert.AreEqual(year, period.FiscalYear, "Year is correct.");

            // Invalid date
            period = _bl.GetFiscalPeriod(year, DateTime.Parse(INVALID_DATE));
            Assert.IsNull(period, "Invalid date returns null.");

            // Invalid period.
            period = _bl.GetFiscalPeriod(year, 42);
            Assert.IsNull(period, "Invalid period returns null.");
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetFiscalPeriodNumber()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            int periodNumber = _bl.GetFiscalPeriodNumber(year, DateTime.Parse($"7/1/{FiscalYearDataSource.OPEN_YEAR}"));
            Assert.AreEqual(7, periodNumber, "Returned correct period number.");

            periodNumber = _bl.GetFiscalPeriodNumber(year, DateTime.Parse(INVALID_DATE));
            Assert.AreEqual(0, periodNumber, "invalid date returns zero.");

            AssertThrowsException<ArgumentNullException>(() => _bl.GetFiscalPeriodNumber(null, DateTime.Parse(INVALID_DATE)), "Null year throws exception.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetDaysInFiscalPeriod()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);

            Assert.AreEqual(31, _bl.GetDaysInFiscalPeriod(year, 7), "Test for July.");
            Assert.AreEqual(30, _bl.GetDaysInFiscalPeriod(year, 11), "Test for November.");
            Assert.AreEqual(0, _bl.GetDaysInFiscalPeriod(year, 0), "Period zero returns zero.");
            Assert.AreEqual(0, _bl.GetDaysInFiscalPeriod(year, 15), "Invalid period number returns zero.");

            AssertThrowsException<ArgumentNullException>(() => _bl.GetDaysInFiscalPeriod(null, 7), "Null year throws exception.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetPriorFiscalYear()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            FiscalYear priorYear = _bl.GetPriorFiscalYear(year);
            Assert.IsNotNull(priorYear, "GetPriorFiscalYear returns non-null value.");
            Assert.AreEqual(FiscalYearDataSource.CLOSED_YEAR, priorYear.Name, "Year name is correct.");
            Assert.AreEqual(year.Ledger, priorYear.Ledger, "Prior year in correct ledger.");

            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == LedgerDataSource.NEW_LEDGER_CODE);
            priorYear = _bl.GetPriorFiscalYear(year, false);
            Assert.IsNull(priorYear, "New ledger test: allLedgers = false returns null.");
            priorYear = _bl.GetPriorFiscalYear(year, true);
            Assert.IsNotNull(priorYear, "New ledger test: allLedgers = true returns non-null value.");
            Assert.AreEqual(FiscalYearDataSource.OPEN_YEAR, priorYear.Name, "New ledger test: allLedgers = true, year name is correct.");
            Assert.AreNotEqual(year.Ledger, priorYear.Ledger, "New ledger test: allLedgers = true, ledgers are not the same.");

            Assert.IsNull(_bl.GetPriorFiscalYear(null), "Null year returns null.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetNextFiscalYear()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            FiscalYear nextYear = _bl.GetNextFiscalYear(year);
            Assert.IsNotNull(nextYear, "GetNextFiscalYear returns non-null value.");
            Assert.AreEqual(FiscalYearDataSource.EMPTY_YEAR, nextYear.Name, "Year name is correct.");
            Assert.AreEqual(year.Ledger, nextYear.Ledger, "Next year in correct ledger.");

            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE_SEPARATE && p.Name == FiscalYearDataSource.OPEN_YEAR);
            nextYear = _bl.GetNextFiscalYear(year, false);
            Assert.IsNull(nextYear, "New ledger test: allLedgers = false returns null.");
            nextYear = _bl.GetNextFiscalYear(year, true);
            Assert.IsNotNull(nextYear, "New ledger test: allLedgers = true returns non-null value.");
            Assert.AreEqual(FiscalYearDataSource.EMPTY_YEAR, nextYear.Name, "New ledger test: allLedgers = true, year name is correct.");
            Assert.AreNotEqual(year.Ledger, nextYear.Ledger, "New ledger test: allLedgers = true, ledgers are not the same.");

            Assert.IsNull(_bl.GetNextFiscalYear(null), "Null year returns null.");
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetNextFiscalPeriod()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            FiscalPeriod period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 6);
            FiscalPeriod nextPeriod = _bl.GetNextFiscalPeriod(period);
            Assert.IsNotNull(nextPeriod, "GetNextFiscalPeriod returns non-null value.");
            Assert.AreEqual(7, nextPeriod.PeriodNumber, "Period number is correct.");
            Assert.AreEqual(year, period.FiscalYear, "Next period in correct year.");

            Assert.IsNull(_bl.GetNextFiscalPeriod(null), "Null period returns null.");

            // Testing thisYearOnly
            period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 12);
            nextPeriod = _bl.GetNextFiscalPeriod(period, false, true);
            Assert.IsNull(nextPeriod, "Returns null if thisYearOnly = true");

            nextPeriod = _bl.GetNextFiscalPeriod(period, false, false);
            Assert.IsNotNull(nextPeriod, "Returns non-null if thisYearOnly = false");
            Assert.AreEqual(1, nextPeriod.PeriodNumber, "Period number is correct.");
            Assert.AreEqual(FiscalYearDataSource.EMPTY_YEAR, nextPeriod.FiscalYear.Name, "Next fiscal year is correct.");

            // Testing includeAdjustmentPeriods

            period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 12);
            nextPeriod = _bl.GetNextFiscalPeriod(period, true, false);
            Assert.AreEqual(13, nextPeriod.PeriodNumber, "Adjustment period returned when includeAdjustmentPeriods = true.");
            Assert.AreEqual(year, period.FiscalYear, "Next period in correct year.");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetPriorFiscalPeriod()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.EMPTY_YEAR);
            FiscalPeriod period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 6);
            FiscalPeriod priorPeriod = _bl.GetPriorFiscalPeriod(period);
            Assert.IsNotNull(priorPeriod, "GetPriorFiscalPeriod returns non-null value.");
            Assert.AreEqual(5, priorPeriod.PeriodNumber, "Period number is correct.");
            Assert.AreEqual(year, period.FiscalYear, "Prior period in correct year.");

            Assert.IsNull(_bl.GetPriorFiscalPeriod(null), "Null period returns null.");

            // Testing thisYearOnly
            period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 1);
            priorPeriod = _bl.GetPriorFiscalPeriod(period, false, true);
            Assert.IsNull(priorPeriod, "Returns null if thisYearOnly = true");

            priorPeriod = _bl.GetPriorFiscalPeriod(period, false, false);
            Assert.IsNotNull(priorPeriod, "Returns non-null if thisYearOnly = false");
            Assert.AreEqual(12, priorPeriod.PeriodNumber, "Period number is correct.");
            Assert.AreEqual(FiscalYearDataSource.OPEN_YEAR, priorPeriod.FiscalYear.Name, "Prior fiscal year is correct.");

            // Testing includeAdjustmentPeriods

            period = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 1);
            priorPeriod = _bl.GetPriorFiscalPeriod(period, true, false);
            Assert.AreEqual(13, priorPeriod.PeriodNumber, "Adjustment period returned when includeAdjustmentPeriods = true.");
            Assert.AreEqual(FiscalYearDataSource.OPEN_YEAR, priorPeriod.FiscalYear.Name, "Prior fiscal year is correct.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_GetFiscalYearForBusinessUnit()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            BusinessUnit otherUnit = _units.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE2);
            FiscalYear otherYear = _bl.GetFiscalYearForBusinessUnit(year, otherUnit);

            Assert.IsNotNull(otherYear, "GetFiscalYearForBusinessUnit returned non-null value.");
            Assert.AreEqual(year.StartDate, otherYear.StartDate, "Returned year has same start date.");
            Assert.AreEqual(otherUnit, otherYear.Ledger.BusinessUnit, "Returned year in correct business unit.");

            Assert.IsNull(_bl.GetFiscalYearForBusinessUnit(null, otherUnit), "Returns null if year is null.");
            Assert.IsNull(_bl.GetFiscalYearForBusinessUnit(year, null), "Returns null if business unit is null.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_ValidateTransactionDate()
        {
            FiscalYear openYear = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            FiscalYear closedYear = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.CLOSED_YEAR);
            FiscalYear resultYear = null;

            AssertNoException(() => resultYear = _bl.ValidateTransactionDate(openYear.Ledger.BusinessUnit, DateTime.Parse($"12/1/{FiscalYearDataSource.OPEN_YEAR}")),
                "No exception thrown for open date.");

            Assert.AreEqual(resultYear, openYear, "Returns correct fiscal year.");

            // For invalid date
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.ValidateTransactionDate(openYear.Ledger.BusinessUnit, DateTime.Parse(INVALID_DATE)),
                UserMessages.TranDateInvalid, "Throws exception for invalid date.");

            // For closed fiscal year
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.ValidateTransactionDate(openYear.Ledger.BusinessUnit, DateTime.Parse($"1/1/{FiscalYearDataSource.CLOSED_YEAR}")),
                UserMessagesGL.FiscalYearClosed, "Throws exception for closed year.");

            // For closed fiscal period

            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.ValidateTransactionDate(openYear.Ledger.BusinessUnit, DateTime.Parse($"1/1/{FiscalYearDataSource.OPEN_YEAR}")),
                UserMessagesGL.FiscalPeriodClosed, "Throws exception for closed period.");

            // Retest using ledger
            AssertNoException(() => resultYear = _bl.ValidateTransactionDate(openYear.Ledger, DateTime.Parse($"12/1/{FiscalYearDataSource.OPEN_YEAR}")),
                "Using ledger: No exception thrown for open date.");

            Assert.AreEqual(resultYear, openYear, "Using ledger: Returns correct fiscal year.");
            
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_CalculateDefaultStartEndDates()
        {
            List<DateTime[]> result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("1/1/2016"), 12, false);
            string dates = "1/1/2016-1/31/2016,2/1/2016-2/29/2016,3/1/2016-3/31/2016,4/1/2016-4/30/2016,5/1/2016-5/31/2016,6/1/2016-6/30/2016," +
                           "7/1/2016-7/31/2016,8/1/2016-8/31/2016,9/1/2016-9/30/2016,10/1/2016-10/31/2016,11/1/2016-11/30/2016,12/1/2016-12/31/2016";
            TestStartEndDateList(result, dates, "12 periods starting 1/1/2016");

            
            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("1/1/2016"), 13, true);
            TestStartEndDateList(result, dates + ",12/31/2016-12/31/2016", "13 periods starting 1/1/2016 with adjustment period");

            // 7 - 11 months should still increment by one month.
            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("1/1/2016"), 10, false);
            dates = string.Join(",", dates.Split(',').Take(10)); // Get just the first 10 start-end dates.
            TestStartEndDateList(result, dates, "10 periods starting 1/1/2016");

            // Test 6 thru 1 period
            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("1/1/2016"), 7, false);
            dates = string.Join(",", dates.Split(',').Take(7)); // Get just the first 7 start-end dates.
            TestStartEndDateList(result, dates, "7 periods starting 1/1/2016");
        
            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("1/1/2016"), 6, false);
            dates = "1/1/2016-2/29/2016,3/1/2016-4/30/2016,5/1/2016-6/30/2016," +
                    "7/1/2016-8/31/2016,9/1/2016-10/31/2016,11/1/2016-12/31/2016";
            TestStartEndDateList(result, dates, "6 periods starting 1/1/2016");

            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("1/1/2016"), 5, false);
            dates = string.Join(",", dates.Split(',').Take(5)); // Get just the first 7 start-end dates.
            TestStartEndDateList(result, dates, "5 periods starting 1/1/2016");

            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("1/1/2016"), 4, false);
            dates = "1/1/2016-3/31/2016,4/1/2016-6/30/2016,7/1/2016-9/30/2016,10/1/2016-12/31/2016";
            TestStartEndDateList(result, dates, "4 periods starting 1/1/2016");

            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("1/1/2016"), 3, false);
            dates = "1/1/2016-4/30/2016,5/1/2016-8/31/2016,9/1/2016-12/31/2016";
            TestStartEndDateList(result, dates, "3 periods starting 1/1/2016");

            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("1/1/2016"), 2, false);
            dates = "1/1/2016-6/30/2016,7/1/2016-12/31/2016";
            TestStartEndDateList(result, dates, "2 periods starting 1/1/2016");

            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("1/1/2016"), 1, false);
            TestStartEndDateList(result, "1/1/2016-12/31/2016", "1 period starting 1/1/2016");

            // Test 1 period starting on odd day
            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("4/15/2016"), 1, false);
            TestStartEndDateList(result, "4/15/2016-4/14/2017", "1 period starting 4/15/2016");

            // Test 12 periods starting on odd day
            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("4/15/2016"), 12, false);
            dates = "4/15/2016-5/14/2016,5/15/2016-6/14/2016,6/15/2016-7/14/2016,7/15/2016-8/14/2016,8/15/2016-9/14/2016,9/15/2016-10/14/2016," +
                    "10/15/2016-11/14/2016,11/15/2016-12/14/2016,12/15/2016-1/14/2017,1/15/2017-2/14/2017,2/15/2017-3/14/2017,3/15/2017-4/14/2017";
            TestStartEndDateList(result, dates, "12 periods starting 4/15/2016");

            // Test 13 periods w/4 28 days per period
            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("5/2/2016"), 13, false);
            dates = "5/2/2016-5/29/2016,5/30/2016-6/26/2016,6/27/2016-7/24/2016,7/25/2016-8/21/2016,8/22/2016-9/18/2016,9/19/2016-10/16/2016," +
                    "10/17/2016-11/13/2016,11/14/2016-12/11/2016,12/12/2016-1/8/2017,1/9/2017-2/5/2017,2/6/2017-3/5/2017,3/6/2017-4/2/2017,4/3/2017-4/30/2017";
            TestStartEndDateList(result, dates, "13 periods starting 5/2/2016");

            // Test 14 periods with adjustment period
            result = _bl.CalculateDefaultStartEndDates(DateTime.Parse("5/2/2016"), 14, true);
            TestStartEndDateList(result, dates + ",4/30/2017-4/30/2017", "14 periods starting 5/2/2016");
            
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void FiscalYearLogic_Validate()
        {
            FiscalYear openYear = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            FiscalYear emptyYear = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.EMPTY_YEAR);

            string tempString = openYear.Name;
            openYear.Name = string.Empty;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), "Fiscal year name", "Validate blank name.");
            openYear.Name = tempString;

            int tempInt = openYear.NumberOfPeriods;
            openYear.NumberOfPeriods = 0;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalPeriodsRange, "Validate number of periods = 0.");
            openYear.NumberOfPeriods = ConstantsGL.MaxFiscalPeriods + 1;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalPeriodsRange, "Validate number of periods too big.");
            openYear.NumberOfPeriods = tempInt;

            tempInt = openYear.CurrentPeriodNumber;
            openYear.CurrentPeriodNumber = 0;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.CurrentPeriodRange, "Validate current period = 0.");
            openYear.CurrentPeriodNumber = openYear.NumberOfPeriods + 1;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.CurrentPeriodRange, "Validate current period too big.");
            openYear.CurrentPeriodNumber = tempInt;

            // Validate modified properties on non-empty fiscal year.

            _fiscalYearRepo.ModifiedPropertyList = nameof(FiscalYear.StartDate);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalYearNotEditable, "Validate start date changed.");

            _fiscalYearRepo.ModifiedPropertyList = nameof(FiscalYear.EndDate);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalYearNotEditable, "Validate end date changed.");

            _fiscalYearRepo.ModifiedPropertyList = nameof(FiscalYear.NumberOfPeriods);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalYearNotEditable, "Validate number of periods changed.");

            _fiscalYearRepo.ModifiedPropertyList = nameof(FiscalYear.HasAdjustmentPeriod);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalYearNotEditable, "Validate has adjustment period changed.");

            _fiscalYearRepo.ModifiedPropertyList = nameof(FiscalYear.Name);
            AssertNoException(() => _bl.Validate(openYear), "Validate name changed.");

            // Validate modified properties on empty fiscal year.
            _fiscalYearRepo.ModifiedPropertyList = $"{nameof(FiscalYear.StartDate)},{nameof(FiscalYear.EndDate)},{nameof(FiscalYear.NumberOfPeriods)},{nameof(FiscalYear.HasAdjustmentPeriod)}";
            AssertNoException(() => _bl.Validate(emptyYear), "Validate changes for empty year.");

            _fiscalYearRepo.ModifiedPropertyList = null;

            // Fiscal period validation
            // Remove a fiscal period
            FiscalPeriod tempPeriod = openYear.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == 4);
            openYear.FiscalPeriods.Remove(tempPeriod);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalPeriodMissing, "Validate missing period.");
            openYear.FiscalPeriods.Add(tempPeriod);

            // Duplicate a fiscal period
            tempPeriod.PeriodNumber = 3;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalPeriodDuplicate, "Validate duplicate period.");
            tempPeriod.PeriodNumber = 4;

            // Invalid start/end dates
            DateTime tempDate = tempPeriod.StartDate.Value;
            tempPeriod.StartDate = null;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalPeriodDatesInvalid, "Validate null period start date.");
            tempPeriod.StartDate = tempDate;

            tempDate = tempPeriod.EndDate.Value;
            tempPeriod.EndDate = null;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalPeriodDatesInvalid, "Validate null period end date.");

            tempPeriod.EndDate = tempDate.AddYears(-1);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalPeriodDatesInvalid, "Validate period end date < period start date.");
            tempPeriod.EndDate = tempDate;

            // Non-contiguous period dates
            tempDate = tempPeriod.StartDate.Value;
            tempPeriod.StartDate = tempDate.AddDays(1);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalPeriodStartDate, "Validate gap due to wrong period start date.");
            tempPeriod.StartDate = tempDate;

            tempDate = tempPeriod.EndDate.Value;
            tempPeriod.EndDate = tempDate.AddDays(-1);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalPeriodStartDate, "Validate gap due to wrong period end date.");
            tempPeriod.EndDate = tempDate;

            // Set wrong period to be adjustment period
            tempPeriod.IsAdjustmentPeriod = true;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.AdjustmentPeriodNotLast, "Validate non-last period as adjustment period.");
            tempPeriod.IsAdjustmentPeriod = false;

            // Change adjustment period start date
            tempPeriod = openYear.FiscalPeriods.FirstOrDefault(p => p.IsAdjustmentPeriod);
            tempDate = tempPeriod.StartDate.Value;
            tempPeriod.StartDate = DateTime.Parse($"12/1/{FiscalYearDataSource.OPEN_YEAR}");
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.AdjustmentPeriodDates, "Validate adjustment period wrong start date.");
            tempPeriod.StartDate = tempDate;

            // Validate modified fiscal period properties for non-empty fiscal year.
            _fiscalPeriodRepo.ModifiedPropertyList = nameof(FiscalPeriod.StartDate);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalYearNotEditable, "Validate period start date changed.");

            _fiscalPeriodRepo.ModifiedPropertyList = nameof(FiscalPeriod.EndDate);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalYearNotEditable, "Validate period end date changed.");

            _fiscalPeriodRepo.ModifiedPropertyList = nameof(FiscalPeriod.IsAdjustmentPeriod);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(openYear), UserMessagesGL.FiscalYearNotEditable, "Validate 'is adjustment period' changed.");

            // Validate modified properties on empty fiscal year.
            _fiscalPeriodRepo.ModifiedPropertyList = $"{nameof(FiscalPeriod.StartDate)},{nameof(FiscalPeriod.EndDate)},{nameof(FiscalPeriod.IsAdjustmentPeriod)},{nameof(FiscalYear.HasAdjustmentPeriod)}";
            AssertNoException(() => _bl.Validate(emptyYear), "Validate period changes for empty year.");

            _fiscalPeriodRepo.ModifiedPropertyList = null;
        }

        private void TestStartEndDateList(List<DateTime[]> list, string expectedData, string message)
        {
            // Convert string into list of datetime arrays with 2 elements in each (start date, end date).
            List<DateTime[]> expected =
                expectedData.Split(',').Select(entry =>
                {
                    var pair = entry.Split('-');
                    return new DateTime[] { DateTime.Parse(pair[0]), DateTime.Parse(pair[1]) };
                })
                .ToList();

            // Ensure both lists have same # of entries.
            Assert.AreEqual(list.Count, expected.Count, message + ": Result count is correct.");

            // Ensure both lists compare correctly.
            Assert.IsTrue(list.Zip(expected, (s1, s2) => s1[0] == s2[0] && s1[1] == s2[1]).All(p => p), message + " All dates match.");
        }
    }
}