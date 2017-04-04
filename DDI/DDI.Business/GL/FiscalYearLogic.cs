﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.GL
{
    public class FiscalYearLogic : EntityLogicBase<FiscalYear>
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(FiscalYearLogic));
        public FiscalYearLogic() : this(new UnitOfWorkEF()) { }
        private IRepository<FiscalYear> _fiscalYearRepository;
        private LedgerLogic _ledgerLogic;       

        public FiscalYearLogic(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _fiscalYearRepository = unitOfWork.GetRepository<FiscalYear>();
            _ledgerLogic = unitOfWork.GetBusinessLogic<LedgerLogic>();
        }

        public override void Validate(FiscalYear fiscalYear)
        {
            bool isModified = false;
            List<string> modifiedProperties = null;
            if (_fiscalYearRepository.GetEntityState(fiscalYear) != EntityState.Added)
            {
                modifiedProperties = _fiscalYearRepository.GetModifiedProperties(fiscalYear);
                isModified = (modifiedProperties.Count > 0);
            }
            else
            {
                isModified = true;
            }

            if (isModified)
            {
              
            }            
        }
        
        public FiscalYear GetCachedFiscalYear(Guid? fiscalYearId)
        {
            if (fiscalYearId == null)
            {
                return null;
            }

            return _ledgerLogic.LedgerCache.Entities.SelectMany(p => p.FiscalYears).FirstOrDefault(p => p.Id == fiscalYearId);
        }

        private CachedDates GetFiscalPeriodDates(FiscalYear year)
        {
            if (year == null)
            {
                throw new ArgumentNullException(nameof(year));
            }

            var dates = new CachedDates(year.NumberOfPeriods);
            foreach (var period in UnitOfWork.GetReference(year, p => p.FiscalPeriods)
                                             .Where(p => p.PeriodNumber > 0 && p.PeriodNumber <= year.NumberOfPeriods)
                                             .OrderBy(p => p.PeriodNumber))
            {
                dates.StartDates[period.PeriodNumber - 1] = period.StartDate;
                dates.EndDates[period.PeriodNumber - 1] = period.EndDate;
            }
            return dates;
        }

        private CachedDates GetCachedPeriodDates(FiscalYear year)
        {
            string cacheKey = year.Id.ToString();
            return CacheHelper.GetEntry(cacheKey, () => GetFiscalPeriodDates(year));
        }

        /// <summary>
        /// Get an array of start dates for each accounting period in the fiscal year.
        /// </summary>
        public DateTime?[] GetFiscalPeriodStartDates(FiscalYear year)
        {
            return GetCachedPeriodDates(year).StartDates;
        }

        /// <summary>
        /// Get an array of end dates for each accounting period in the fiscal year.
        /// </summary>
        public DateTime?[] GetFiscalPeriodEndDates(FiscalYear year)
        {
            return GetCachedPeriodDates(year).EndDates;
        }

        public FiscalYear GetFiscalYear (BusinessUnit unit, DateTime? date)
        {
            if (unit == null)
            {
                return null;
            }
            return GetFiscalYear(unit.Id, date);
        }

        public FiscalYear GetFiscalYear(Ledger ledger, DateTime? date)
        {
            if (ledger == null)
            {
                return null;
            }
            return GetFiscalYear(ledger.Id, date);
        }

        public FiscalYear GetFiscalYear(Guid id, DateTime? date)
        {
            if (date == null)
            {
                return null;
            }

            DateTime dateValue = date.Value.Date;

            return _ledgerLogic.LedgerCache.Entities
                                           .Where(p => p.Id == id || p.BusinessUnitId == id)
                                           .SelectMany(p => p.FiscalYears.Where(y => y.StartDate <= dateValue && y.EndDate >= dateValue))
                                           .FirstOrDefault();
        }
        
        public FiscalPeriod GetFiscalPeriod(FiscalYear year, int periodNumber)
        {
            return UnitOfWork.GetReference(year, p => p.FiscalPeriods).FirstOrDefault(p => p.PeriodNumber == periodNumber);
        }

        public FiscalPeriod GetFiscalPeriod(FiscalYear year, DateTime date)
        {
            int periodNumber = GetFiscalPeriodNumber(year, date);
            if (periodNumber >= 1)
            {
                return GetFiscalPeriod(year, periodNumber);
            }
            return null;
        }

        /// <summary>
        /// Convert a date to a period number (1 - 14) for the fiscal year.  Returns 0 if the date is not in the fiscal year.
        /// </summary>
        public int GetFiscalPeriodNumber(FiscalYear year, DateTime date)
        {
            date = date.Date;

            if (date < year.StartDate || date > year.EndDate)
            {
                return 0;
            }

            CachedDates dates = GetCachedPeriodDates(year);

            for (int period = 0; period < year.NumberOfPeriods; period++)
            {
                if (dates.StartDates[period].HasValue && dates.EndDates[period].HasValue && dates.StartDates[period].Value <= date && dates.EndDates[period].Value >= date)
                {
                    return period + 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// Gets the number of days in a period (1 - 14) for the fiscal year.
        /// </summary>
        public int GetDaysInFiscalPeriod(FiscalYear year, int period)
        {
            int days = 0;

            if (period <= 0 || period > year.NumberOfPeriods)
            {
                return days;
            }

            CachedDates dates = GetCachedPeriodDates(year);

            DateTime? start = dates.StartDates[period - 1];
            DateTime? end = dates.EndDates[period - 1];

            if (start.HasValue && end.HasValue)
            {
                days = (int)(end.Value - start.Value).TotalDays + 1;
            }

            if (days < 0)
            {
                days = 0;
            }

            return days;
        }

        public FiscalYear GetPriorFiscalYear(FiscalYear year, bool allLedgers = false)
        {
            if (year?.StartDate == null)
            {
                return null;
            }

            if (allLedgers)
            {
                Guid? unitId = UnitOfWork.GetReference(year, p => p.Ledger).BusinessUnitId;

                return _ledgerLogic.LedgerCache.Entities.Where(p => p.BusinessUnitId == unitId)
                                                        .SelectMany(p => p.FiscalYears.Where(y => y.EndDate < year.StartDate))
                                                        .OrderByDescending(p => p.EndDate.Value).FirstOrDefault();
            }
            return _ledgerLogic.GetCachedLedger(year.LedgerId).FiscalYears.Where(p => p.EndDate < year.StartDate).OrderByDescending(p => p.EndDate.Value).FirstOrDefault();
        }


        public FiscalYear GetNextFiscalYear(FiscalYear year, bool allLedgers = false)
        {
            if (year?.EndDate == null)
            {
                return null;
            }

            if (allLedgers)
            {
                Guid? unitId = UnitOfWork.GetReference(year, p => p.Ledger).BusinessUnitId;

                return _ledgerLogic.LedgerCache.Entities.Where(p => p.BusinessUnitId == unitId)
                                                        .SelectMany(p => p.FiscalYears.Where(y => y.StartDate > year.EndDate))
                                                        .OrderBy(p => p.StartDate.Value).FirstOrDefault();
            }

            return _ledgerLogic.GetCachedLedger(year.LedgerId).FiscalYears.Where(p => p.StartDate > year.EndDate).OrderBy(p => p.StartDate.Value).FirstOrDefault();
        }

        /// <summary>
        /// Get the next fiscal period.  Returns null if next fiscal period is undefined.
        /// </summary>
        /// <param name="includeAdjustmentPeriods">TRUE to include adjustment periods.</param>
        /// <param name="thisYearOnly">TRUE to return null if this is the final period in the fiscal year.</param>
        /// <returns></returns>
        public FiscalPeriod GetNextFiscalPeriod(FiscalPeriod period, bool includeAdjustmentPeriods = false, bool thisYearOnly = false)
        {
            if (period == null)
            {
                return null;
            }
            
            int periodNumber = period.PeriodNumber + 1;

            // Ensure all necessary navigation properties are loaded.
            if (period.FiscalYear == null)
            {
                period = UnitOfWork.GetById<FiscalPeriod>(period.Id, p => p.FiscalYear.FiscalPeriods);
            }
            else if (period.FiscalYear.FiscalPeriods == null)
            {
                UnitOfWork.LoadReference(period.FiscalYear, p => p.FiscalPeriods);
            }

            FiscalYear year = period.FiscalYear;
            while (true)
            {
                FiscalPeriod nextPeriod = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == periodNumber);
                if (nextPeriod == null)
                {
                    if (thisYearOnly)
                    {
                        return null;
                    }
                    year = GetNextFiscalYear(year);
                    periodNumber = 1;
                    if (year == null)
                    {
                        return null;
                    }
                }
                else if (nextPeriod.IsAdjustmentPeriod && !includeAdjustmentPeriods)
                {
                    periodNumber++;
                }
                else
                {
                    return nextPeriod;
                }
            }
        }


        /// <summary>
        /// Get the prior fiscal period.  Returns null if prior fiscal period is undefined.
        /// </summary>
        /// <param name="includeAdjustmentPeriods">TRUE to include adjustment periods.</param>
        /// <param name="thisYearOnly">TRUE to return null if this is the first period in the fiscal year.</param>
        /// <returns></returns>
        public FiscalPeriod GetPriorFiscalPeriod(FiscalPeriod period, bool includeAdjustmentPeriods = false, bool thisYearOnly = false)
        {
            if (period == null)
            {
                return null;
            }

            int periodNumber = period.PeriodNumber - 1;

            // Ensure all necessary navigation properties are loaded.
            if (period.FiscalYear == null)
            {
                period = UnitOfWork.GetById<FiscalPeriod>(period.Id, p => p.FiscalYear.FiscalPeriods);
            }
            else if (period.FiscalYear.FiscalPeriods == null)
            {
                UnitOfWork.LoadReference(period.FiscalYear, p => p.FiscalPeriods);
            }

            FiscalYear year = period.FiscalYear;
            while (true)
            {
                if (periodNumber < 1)
                {
                    if (thisYearOnly)
                    {
                        return null;
                    }
                    year = GetPriorFiscalYear(year);
                    if (year == null)
                    {
                        return null;
                    }
                    periodNumber = year.NumberOfPeriods;
                }

                FiscalPeriod priorPeriod = year.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == periodNumber);
                if (priorPeriod == null)
                {
                    return null;
                }
                else if (priorPeriod.IsAdjustmentPeriod && !includeAdjustmentPeriods)
                {
                    periodNumber--;
                }
                else
                {
                    return priorPeriod;
                }
            }
        }

        /// <summary>
        /// Get an instance of this fiscal year in a different business unit.  (I.e. a fiscal year that contain's this fiscal year's start date.)
        /// </summary>
        public FiscalYear GetFiscalYearForBusinessUnit(FiscalYear year, BusinessUnit unit)
        {
            if (unit == null)
            {
                return null;
            }
            return GetFiscalYearForBusinessUnit(year, unit.Id);
        }

        /// <summary>
        /// Get an instance of this fiscal year in a different business unit.  (I.e. a fiscal year that contain's this fiscal year's start date.)
        /// </summary>
        public FiscalYear GetFiscalYearForBusinessUnit(FiscalYear year, Guid unitId)
        {
            if (year == null)
            {
                return null;
            }

            Ledger ledger = UnitOfWork.GetReference(year, p => p.Ledger);

            if (ledger.BusinessUnitId == unitId)
                return year;

            // Select all the years in all ledgers for given business unit that contain this fiscal year's start date.  (There should be zero or one).
            return _ledgerLogic.LedgerCache.Entities.Where(p => p.BusinessUnitId == unitId)
                                                    .SelectMany(p => p.FiscalYears.Where(f => f.StartDate <= year.StartDate && f.EndDate >= year.EndDate))
                                                    .FirstOrDefault();
        }


        /// <summary>
        /// Calculate a list of default start and end dates for a fiscal year based on a number of periods.  
        /// </summary>
        public List<DateTime[]> CalculateDefaultStartEndDates(DateTime startDate, int numPeriods, bool adjustmentPeriod)
        {
            List<DateTime[]> result = new List<DateTime[]>();
            DateTime nextDate;
            DateTime endDate = DateTime.MinValue;

            // If final period is an adjustment period, get number of non-adjustment periods.
            if (adjustmentPeriod && numPeriods > 1)
            {
                numPeriods--;
            }

            if (numPeriods == 1)
            {
                // For only 1 period, add one year and subtract one day.
                endDate = startDate.AddYears(1).AddDays(-1);
                result.Add(new DateTime[2] { startDate, endDate });
            }
            else if (numPeriods == 13)
            {
                // For 13 periods, the first 12 are spaced 28 days apart.
                nextDate = startDate;
                for (int count = 0; count < 12; count++)
                {
                    result.Add(new DateTime[2] { nextDate, nextDate.AddDays(27) });
                    nextDate = nextDate.AddDays(28);
                }

                // For the 13th period end date, calculate the date on which the next fiscal year should start:
                // It should be on the same day of the week and in the same week number for that month.
                DayOfWeek weekday = startDate.DayOfWeek; // day of week
                int weekNum = (startDate.Day / 7); // week number

                // Start at the first of the starting month in the next year.
                endDate = new DateTime(startDate.Year + 1, startDate.Month, 1);

                // Move forward until we hit the same day of the week.
                while (endDate.DayOfWeek != weekday)
                { 
                    endDate = endDate.AddDays(1);
                }

                /// Move forward 1 week at a time until we hit the same week.
                while ((endDate.Day / 7) < weekNum)
                {
                    endDate = endDate.AddDays(7);
                }

                // Go back one day.
                endDate = endDate.AddDays(-1);

                // Store final period start/end date.
                result.Add(new DateTime[2] { nextDate, endDate });
            }
            else
            {
                // All others (2 to 12 periods):
                // Get the day of the month for the start date.  If its on the last day of that month, use 31.
                int day = startDate.Day;
                if (startDate.Month != startDate.AddDays(1).Month)
                {
                    day = 31;
                }

                // Determine how many months each period has
                int increment = 1;

                if (numPeriods == 2)
                {
                    increment = 6;
                }
                else if (numPeriods == 3)
                {
                    increment = 4;
                }
                else if (numPeriods == 4)
                {
                    increment = 3;
                }
                else if (numPeriods <= 6)
                {
                    increment = 2;
                }

                // Calculate start/end dates, advancing (increment) number of months.
                for (int count = 0; count < numPeriods; count++)
                {
                    nextDate = DateHelper.GetNearestValidDate(startDate.Month + increment, day, startDate.Year, true);
                    endDate = nextDate.AddDays(-1);
                    result.Add(new DateTime[2] { startDate, endDate });
                    startDate = nextDate;
                }
            }

            // If there's an adjustment period, the final period starts and ends on the end date of the last normal period.
            if (adjustmentPeriod)
            {
                result.Add(new DateTime[2] { endDate, endDate });
            }

            return result;
        }


        /// <summary>
        /// Nested class that holds a set of start and end dates for all fiscal periods in a year.
        /// </summary>
        private class CachedDates
        {
            public DateTime?[] StartDates { get; private set; }
            public DateTime?[] EndDates { get; private set; }

            public CachedDates(int numberOfPeriods)
            {
                StartDates = new DateTime?[numberOfPeriods];
                EndDates = new DateTime?[numberOfPeriods];
            }
        }

    }
}
