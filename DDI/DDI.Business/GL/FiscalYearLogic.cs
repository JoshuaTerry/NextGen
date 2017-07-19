using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.Helpers;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class FiscalYearLogic : EntityLogicBase<FiscalYear>
    {
        #region Fields

        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(FiscalYearLogic));

        private IRepository<FiscalYear> _fiscalYearRepository;
        private IRepository<FiscalPeriod> _fiscalPeriodRepository;
        private LedgerLogic _ledgerLogic;

        #endregion

        #region Constructors

        public FiscalYearLogic(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _fiscalYearRepository = unitOfWork.GetRepository<FiscalYear>();
            _fiscalPeriodRepository = UnitOfWork.GetRepository<FiscalPeriod>();
            _ledgerLogic = unitOfWork.GetBusinessLogic<LedgerLogic>();
        }

        #endregion

        #region Public Methods

        public override void Validate(FiscalYear fiscalYear)
        {
            if (string.IsNullOrWhiteSpace(fiscalYear.Name))
            {
                throw new ValidationException(UserMessages.MustBeNonBlank, "Fiscal year name");
            }

            if (fiscalYear.NumberOfPeriods < 1 || fiscalYear.NumberOfPeriods > ConstantsGL.MaxFiscalPeriods)
            {
                throw new ValidationException(UserMessagesGL.FiscalPeriodsRange);
            }

            if ((fiscalYear.CurrentPeriodNumber < 1 && fiscalYear.Status != FiscalYearStatus.Empty) || fiscalYear.CurrentPeriodNumber > fiscalYear.NumberOfPeriods)
            {
                throw new ValidationException(UserMessagesGL.CurrentPeriodRange, fiscalYear.NumberOfPeriods.ToString());
            }

            if (fiscalYear.StartDate == null || fiscalYear.EndDate == null || fiscalYear.StartDate > fiscalYear.EndDate)
            {
                throw new ValidationException(UserMessagesGL.FiscalYearDatesInvalid);
            }

            // Validate other modified properties

            bool isNewYear = false;
            bool isModified = false;
            List<string> modifiedProperties = null;
            if (_fiscalYearRepository.GetEntityState(fiscalYear) != EntityState.Added)
            {
                modifiedProperties = _fiscalYearRepository.GetModifiedProperties(fiscalYear);
                isModified = (modifiedProperties.Count > 0);
            }
            else
            {
                isNewYear = isModified = true;
            }

            if (isModified && fiscalYear.Status != FiscalYearStatus.Empty)
            {
                // See if any properties were modified that would affect posted transactions.
                if (modifiedProperties.Intersect(new string[] {
                    nameof(FiscalYear.EndDate), nameof(FiscalYear.StartDate), nameof(FiscalYear.NumberOfPeriods),
                    nameof(FiscalYear.HasAdjustmentPeriod)}).Count() > 0)
                {
                    throw new ValidationException(UserMessagesGL.FiscalYearNotEditable);
                }
            }   
            
            // Validate fiscal periods
            if (fiscalYear.FiscalPeriods != null)
            {
                DateTime nextStartDate = fiscalYear.StartDate.Value;
                for (int periodNumber = 1; periodNumber <= fiscalYear.NumberOfPeriods; periodNumber++)
                {
                    var periods = fiscalYear.FiscalPeriods.Where(p => p.PeriodNumber == periodNumber);
                    if (periods.Count() == 0)
                    {
                        throw new ValidationException(UserMessagesGL.FiscalPeriodMissing, periodNumber.ToString(), fiscalYear.Name);
                    }

                    if (periods.Count() > 1)
                    {
                        throw new ValidationException(UserMessagesGL.FiscalPeriodDuplicate, periodNumber.ToString(), fiscalYear.Name);
                    }

                    FiscalPeriod period = periods.First();

                    if (period.StartDate == null || period.EndDate == null || period.StartDate > period.EndDate)
                    {
                        throw new ValidationException(UserMessagesGL.FiscalPeriodDatesInvalid, periodNumber.ToString(), fiscalYear.Name);
                    }
                    if (period.StartDate < fiscalYear.StartDate)
                    {
                        throw new ValidationException(UserMessagesGL.FiscalPeriodStartDate, periodNumber.ToString(), fiscalYear.Name, fiscalYear.StartDate.ToShortDateString());
                    }
                    if (period.EndDate > fiscalYear.EndDate)
                    {
                        throw new ValidationException(UserMessagesGL.FiscalPeriodEndDate, periodNumber.ToString(), fiscalYear.Name, fiscalYear.EndDate.ToShortDateString());
                    }

                    if (fiscalYear.CurrentPeriodNumber == 0)
                    {
                        fiscalYear.CurrentPeriodNumber = 1;
                    }

                    // Adjustment period must be the final period and have a date range matching the last day of the year.
                    if (period.IsAdjustmentPeriod)
                    {
                        if (periodNumber < fiscalYear.NumberOfPeriods)
                        {
                            throw new ValidationException(UserMessagesGL.AdjustmentPeriodNotLast, fiscalYear.Name);
                        }

                        if (period.StartDate != fiscalYear.EndDate || period.EndDate != fiscalYear.EndDate)
                        {                            
                            throw new ValidationException(UserMessagesGL.AdjustmentPeriodDates, fiscalYear.Name,  fiscalYear.EndDate.ToShortDateString());
                        }
                    }
                    else
                    {
                        // Start/end dates cannot overlap
                        if (period.StartDate < nextStartDate)
                        {
                            throw new ValidationException(UserMessagesGL.FiscalPeriodStartDate, periodNumber.ToString(), fiscalYear.Name, 
                                nextStartDate.ToShortDateString());
                        }
                    }
                    nextStartDate = period.EndDate.Value.AddDays(1);
                }
            }

            // Determine if any fiscal periods were modified in a non-empty fiscal year.  Also make sure none have invalid period numbers.

            if (fiscalYear.FiscalPeriods != null)
            {
                string[] propertiesToCheck = new string[] { nameof(FiscalPeriod.EndDate), nameof(FiscalPeriod.StartDate), nameof(FiscalPeriod.IsAdjustmentPeriod), nameof(FiscalPeriod.PeriodNumber) };

                foreach (var period in fiscalYear.FiscalPeriods)
                {
                    if (period.PeriodNumber < 1 || period.PeriodNumber > fiscalYear.NumberOfPeriods)
                    {
                        throw new ValidationException(UserMessagesGL.FiscalPeriodNumberInvalid, fiscalYear.NumberOfPeriods.ToString());
                    }

                    bool isAdded = false;
                    if (_fiscalPeriodRepository.GetEntityState(period) != EntityState.Added)
                    {
                        modifiedProperties = _fiscalPeriodRepository.GetModifiedProperties(period);                        
                    }
                    else
                    {
                        isAdded = true;
                        modifiedProperties.Clear();
                    }
                    if (isAdded || modifiedProperties.Intersect(propertiesToCheck).Count() > 0)
                    {
                        isModified = true;
                        if (fiscalYear.Status != FiscalYearStatus.Empty)
                        {
                            throw new ValidationException(UserMessagesGL.FiscalYearNotEditable);
                        }
                    }
                }
            }

            // If fiscal year or fiscal periods were modified, synchronize across common ledgers.
            if (isModified)
            {
                SyncronizeFiscalYear(fiscalYear);
            }

            // Ensure there's a fund for this fiscal year if fund accounting not enabled.
            Ledger ledger = _ledgerLogic.GetCachedLedger(fiscalYear.LedgerId);
            if (!ledger.FundAccounting && 
                (isNewYear || !UnitOfWork.Any<Fund>(p => p.FiscalYearId == fiscalYear.Id)))
            {
                Fund fund = new Fund
                {
                    FiscalYear = fiscalYear
                };
                UnitOfWork.Insert(fund);
            }

            _ledgerLogic.InvalidLedgerCache();
        }
        
        /// <summary>
        /// Copy fiscal year settings to all other common business units, including the org. business unit.
        /// </summary>
        /// <remarks>FiscalPeriods must be loaded.</remarks>
        public void SyncronizeFiscalYear(FiscalYear baseYear)
        {
            if (baseYear == null)
            {
                throw new ArgumentNullException(nameof(baseYear));
            }

            Ledger ledger = _ledgerLogic.GetCachedLedger(baseYear.LedgerId);
            
            if (ledger.BusinessUnit.BusinessUnitType == BusinessUnitType.Separate)
            {
                // Separate ledgers are not synchronized.
                return;
            }
            
            // Iterate thru all common/org. business units.
            foreach (var unit in UnitOfWork.Where<BusinessUnit>(p => p.BusinessUnitType != BusinessUnitType.Separate && p.Id != ledger.BusinessUnitId))
            {
                var otherYear = GetFiscalYearForBusinessUnit(baseYear, unit.Id);
                if (otherYear == null)
                {
                    CreateFiscalYearForBusinessUnit(baseYear, unit.Id);
                }
                else
                {
                    SynchronizeFiscalYearForBusinessUnit(baseYear, otherYear);
                }
            }
        }

        /// <summary>
        /// Copy fiscal year settings from one year to another year.  These should be to similar fiscal years in different business units.
        /// </summary>
        private void SynchronizeFiscalYearForBusinessUnit(FiscalYear fromYear, FiscalYear toYear)
        {
            if (toYear.Status != FiscalYearStatus.Empty)
            {
                throw new InvalidOperationException(UserMessagesGL.FiscalYearNotEditable);
            }

            if (toYear.LedgerId == fromYear.LedgerId)
            {
                throw new InvalidOperationException("Fiscal years must be in different ledgers.");
            }

            toYear.StartDate = fromYear.StartDate;
            toYear.EndDate = fromYear.EndDate;
            toYear.HasAdjustmentPeriod = fromYear.HasAdjustmentPeriod;
            toYear.NumberOfPeriods = fromYear.NumberOfPeriods;
            if (toYear.CurrentPeriodNumber > toYear.NumberOfPeriods)
            {
                toYear.CurrentPeriodNumber = toYear.NumberOfPeriods;
            }

            UnitOfWork.LoadReference(toYear, p => p.FiscalPeriods);

            // Add/update fiscal periods in fromYear to toYear.
            foreach (var fromPeriod in fromYear.FiscalPeriods)
            {
                FiscalPeriod toPeriod = toYear.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == fromPeriod.PeriodNumber);
                if (toPeriod == null)
                {
                    toPeriod = new FiscalPeriod
                    {
                        PeriodNumber = fromPeriod.PeriodNumber,
                        FiscalYear = toYear,
                        Status = FiscalPeriodStatus.Open
                    };

                    UnitOfWork.Insert(toPeriod);
                }

                toPeriod.StartDate = fromPeriod.StartDate;
                toPeriod.EndDate = fromPeriod.EndDate;
                toPeriod.IsAdjustmentPeriod = fromPeriod.IsAdjustmentPeriod;                
            }

            // Remove fiscal periods in toYear that don't exist in fromYear.
            foreach (var toPeriod in toYear.FiscalPeriods.ToList())
            {
                if (!fromYear.FiscalPeriods.Any(p => p.PeriodNumber == toPeriod.PeriodNumber))
                {
                    UnitOfWork.Delete(toPeriod);
                }
            }

        }

        /// <summary>
        /// Create a copy of a fiscal year in a different business unit.
        /// </summary>
        private void CreateFiscalYearForBusinessUnit(FiscalYear fromYear, Guid unitId)
        {
            Ledger ledger = _ledgerLogic.GetCurrentLedger(unitId);
            if (ledger == null)
            {
                return;
            }

            if (fromYear.LedgerId == ledger.Id)
            {
                throw new InvalidOperationException("Cannot copy a fiscal year into the same ledger.");
            }

            if (UnitOfWork.Any<FiscalYear>(p => p.LedgerId == ledger.Id && p.Name == fromYear.Name))
            {
                throw new InvalidOperationException(string.Format("Fiscal year {0} already exists in {1} {2} with a different start date.", fromYear.Name,
                    BusinessUnitHelper.GetBusinessUnitLabel(UnitOfWork), ledger.Code));
            }

            // Create new fiscal year and periods 

            FiscalYear year = new FiscalYear
            {
                LedgerId = ledger.Id,
                Name = fromYear.Name,
                CurrentPeriodNumber = 1,
                EndDate = fromYear.EndDate,
                StartDate = fromYear.StartDate,
                Status = fromYear.Status,
                HasAdjustmentPeriod = fromYear.HasAdjustmentPeriod,
                NumberOfPeriods = fromYear.NumberOfPeriods,
                FiscalPeriods = new List<FiscalPeriod>()
            };

            UnitOfWork.Insert(year);

            if (!ledger.FundAccounting)
            {
                Fund fund = new Fund();
                fund.FiscalYear = year;
                UnitOfWork.Insert(fund);
            }
            
            // Copy fiscal periods
            foreach (var fromPeriod in fromYear.FiscalPeriods)
            {
                FiscalPeriod period = new FiscalPeriod
                {
                    FiscalYear = year,
                    IsAdjustmentPeriod = fromPeriod.IsAdjustmentPeriod,
                    EndDate = fromPeriod.EndDate,
                    StartDate = fromPeriod.StartDate,
                    Status = FiscalPeriodStatus.Open,
                    PeriodNumber = fromPeriod.PeriodNumber
                };
                UnitOfWork.Insert(period);
            }

        }

        /// <summary>
        /// Get a cached version of a fiscal year.
        /// </summary>
        /// <param name="fiscalYearId"></param>
        /// <returns></returns>
        public FiscalYear GetCachedFiscalYear(Guid? fiscalYearId)
        {
            if (fiscalYearId == null)
            {
                return null;
            }

            return _ledgerLogic.LedgerCache.Entities.SelectMany(p => p.FiscalYears).FirstOrDefault(p => p.Id == fiscalYearId);
        }

        /// <summary>
        /// Get an array of start dates for each accounting period in the fiscal year.
        /// </summary>
        public DateTime?[] GetFiscalPeriodStartDates(FiscalYear year)
        {
            return GetCachedPeriodDates(year).Select(p => p.StartDate).ToArray();
        }

        /// <summary>
        /// Get an array of end dates for each accounting period in the fiscal year.
        /// </summary>
        public DateTime?[] GetFiscalPeriodEndDates(FiscalYear year)
        {
            return GetCachedPeriodDates(year).Select(p => p.EndDate).ToArray();
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
            if (year == null)
            {
                return null;
            }
            return UnitOfWork.GetReference(year, p => p.FiscalPeriods).FirstOrDefault(p => p.PeriodNumber == periodNumber);
        }

        public FiscalPeriod GetFiscalPeriod(FiscalYear year, DateTime date)
        {
            if (year == null)
            {
                return null;
            }
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
            if (year == null)
            {
                throw new ArgumentNullException(nameof(year));
            }

            date = date.Date;

            if (date < year.StartDate || date > year.EndDate)
            {
                return 0;
            }

            var dates = GetCachedPeriodDates(year);

            int periodNumber = 0;
            if (year.Status == FiscalYearStatus.Closed)
            {
                // If fiscal year is closed and the date corresponds to an adjustment period, use the adjustment period.
                periodNumber = dates.FirstOrDefault(p => p.IsAdjustmentPeriod == true && p.StartDate <= date && p.EndDate >= date).PeriodNumber;
            }
            if (periodNumber == 0)
            {
                periodNumber = dates.FirstOrDefault(p => p.StartDate <= date && p.EndDate >= date).PeriodNumber;
            }

            return periodNumber;
        }

        /// <summary>
        /// Gets the number of days in a period (1 - 14) for the fiscal year.
        /// </summary>
        public int GetDaysInFiscalPeriod(FiscalYear year, int period)
        {
            if (year == null)
            {
                throw new ArgumentNullException(nameof(year));
            }

            int days = 0;

            if (period <= 0 || period > year.NumberOfPeriods)
            {
                return days;
            }

            var dates = GetCachedPeriodDates(year);

            DateTime? start = dates[period - 1].StartDate;
            DateTime? end = dates[period - 1].EndDate;

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
        /// Get an instance of this fiscal year in a different business unit.  (I.e. a fiscal year that contains this fiscal year's start date.)
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
        /// Get an instance of this fiscal year in a different business unit.  (I.e. a fiscal year that contains this fiscal year's start date.)
        /// </summary>
        public FiscalYear GetFiscalYearForBusinessUnit(FiscalYear year, Guid unitId)
        {
            if (year == null)
            {
                return null;
            }

            Ledger ledger = UnitOfWork.GetReference(year, p => p.Ledger);

            if (ledger.BusinessUnitId == unitId)
            {
                return year;
            }            

            // Select all the years in all ledgers for given business unit that contain this fiscal year's start date.  (There should be zero or one).
            return _ledgerLogic.LedgerCache.Entities.Where(p => p.BusinessUnitId == unitId)
                                                    .SelectMany(p => p.FiscalYears.Where(f => f.StartDate <= year.StartDate && f.EndDate >= year.EndDate))
                                                    .FirstOrDefault();
        }

        /// <summary>
        /// Get an instance of this fiscal year in a different ledger.  (I.e. a fiscal year that contains this fiscal year's start date.)
        /// </summary>
        public FiscalYear GetFiscalYearForLedger(FiscalYear year, Guid ledgerId)
        {
            if (year == null)
            {
                return null;
            }

            if (year.LedgerId == ledgerId)
            {
                return year;
            }

            // Select all the years in all ledgers for given business unit that contain this fiscal year's start date.  (There should be zero or one).
            return _ledgerLogic.LedgerCache.Entities.FirstOrDefault(p => p.Id == ledgerId)?
                                                    .FiscalYears.Where(f => f.StartDate <= year.StartDate && f.EndDate >= year.EndDate)
                                                    .FirstOrDefault();
        }

        /// <summary>
        /// Validate a transaction date, including ability to post to closed fiscal periods.
        /// </summary>
        /// <param name="ledger">Ledger</param>
        /// <param name="dt">Transaction date</param>
        /// <returns>Fiscal year for transaction date.</returns>
        public FiscalYear ValidateTransactionDate(Ledger ledger, DateTime? dt)
        {
            if (ledger == null)
            {
                return null;
            }
            return ValidateTransactionDate(ledger.Id, dt);
        }

        /// <summary>
        /// Validate a transaction date, including ability to post to closed fiscal periods.
        /// </summary>
        /// <param name="unit">Business unit</param>
        /// <param name="dt">Transaction date</param>
        /// <returns>Fiscal year for transaction date.</returns>
        public FiscalYear ValidateTransactionDate(BusinessUnit unit, DateTime? dt)
        {
            if (unit == null)
            {
                return null;
            }
            return ValidateTransactionDate(unit.Id, dt);
        }

        /// <summary>
        /// Validate a transaction date, including ability to post to closed fiscal periods.
        /// </summary>
        /// <param name="id">Id of business unit or ledger.</param>
        /// <param name="dt">Transaction date</param>
        /// <returns>Fiscal year for transaction date.</returns>
        public FiscalYear ValidateTransactionDate(Guid id, DateTime? dt)
        {
            FiscalYear year = GetFiscalYear(id, dt);
            if (year == null)
                throw new ValidationException(UserMessages.TranDateInvalid);

            FiscalPeriod prd = GetFiscalPeriod(year, dt.Value);
            if (prd == null)
                throw new ValidationException(UserMessages.TranDateInvalid);

            bool allowPriorPeriod = false;

            switch (year.Ledger.PriorPeriodPostingMode)
            {
                case PriorPeriodPostingMode.Allow:
                    allowPriorPeriod = true;
                    break;
                case PriorPeriodPostingMode.Prohibit:
                    allowPriorPeriod = false;
                    break;
                case PriorPeriodPostingMode.Security:
                    //allowPriorPeriod = ApplicationHelper.SecurityModule.CheckSecurityFunction(SecurityFunctions.GL.PostPriorPeriodTransactions);
                    break;
            }

            if (!allowPriorPeriod)
            {
                if (year.Status == FiscalYearStatus.Closed)
                    throw new ValidationException(UserMessagesGL.FiscalYearClosed, year.Name);


                if (prd.Status == FiscalPeriodStatus.Closed)
                    throw new ValidationException(UserMessagesGL.FiscalPeriodClosed, prd.PeriodNumber.ToString(), year.Name);
            }

            return year;
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

                int increment = 1;
                if (numPeriods >= 1 && numPeriods <= 12)
                {
                    increment = 12 / numPeriods;  // 12, 6, 4, 3, 2, 2, 1...
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

        #endregion

        #region Private Methods

        private Period[] GetFiscalPeriodDates(FiscalYear year)
        {
            if (year == null)
            {
                throw new ArgumentNullException(nameof(year));
            }

            // The array must have an entry for each period, even though the year's FiscalPeriods collection might have missing entries.
            var dates = new Period[year.NumberOfPeriods];
            foreach (var period in UnitOfWork.GetReference(year, p => p.FiscalPeriods)
                                             .Where(p => p.PeriodNumber > 0 && p.PeriodNumber <= year.NumberOfPeriods)
                                             .OrderBy(p => p.PeriodNumber))
            {
                dates[period.PeriodNumber - 1] = period;
            }
            return dates;
        }

        private Period[] GetCachedPeriodDates(FiscalYear year)
        {
            string cacheKey = year.Id.ToString();
            return CacheHelper.GetEntry(cacheKey, () => GetFiscalPeriodDates(year));
        }

        #endregion


        #region Nested Classes

        /// <summary>
        /// Nested struct for use with GetCachedDates.
        /// </summary>

        public struct Period
        {
            public int PeriodNumber;
            public DateTime? StartDate;
            public DateTime? EndDate;
            public bool IsAdjustmentPeriod;

            public static implicit operator Period(FiscalPeriod fp)
            {
                return new Period()
                {
                    PeriodNumber = fp.PeriodNumber,
                    StartDate = fp.StartDate,
                    EndDate = fp.EndDate,
                    IsAdjustmentPeriod = fp.IsAdjustmentPeriod
                };
            }                        
        }

        #endregion
    }
}
