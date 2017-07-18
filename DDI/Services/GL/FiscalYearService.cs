using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;
using Newtonsoft.Json.Linq;

namespace DDI.Services.GL
{
    public class FiscalYearService : ServiceBase<FiscalYear>, IFiscalYearService
    {
        private FiscalYearLogic _logic;
        public FiscalYearService(IUnitOfWork uow, FiscalYearLogic logic) : base(uow)
        {
            _logic = logic;
        }

        public IDataResponse<FiscalYear> Post(FiscalYear entityToSave)
        {
            SetFiscalPeriods(entityToSave);
            
            return base.Add(entityToSave);
        }

        /// <summary>
        /// Get the equivalent fiscal year for a different business unit.
        /// </summary>
        /// <param name="unitId">Target business unit ID</param>
        /// <param name="fiscalYearId">Fiscal year ID</param>
        public IDataResponse<List<ICanTransmogrify>> GetYearForOtherBusinessUnit(Guid unitId, Guid fiscalYearId)
        {
            FiscalYear year = UnitOfWork.GetById<FiscalYear>(fiscalYearId, p => p.Ledger);
            if (year == null)
            {
                return GetErrorResponse<List<ICanTransmogrify>>(UserMessagesGL.BadFiscalYear);
            }

            year = _logic.GetFiscalYearForBusinessUnit(year, unitId);
            var results = new List<ICanTransmogrify>();
            if (year != null)
            {
                results.Add(year);
            }

            var response = GetIDataResponse(() => results);
            response.TotalResults = results.Count;

            return response;
        }

        /// <summary>
        /// Create a new fiscal year via the "copy" POST route.
        /// </summary>
        /// <param name="sourceFiscalYearId">Id of existing fiscal year</param>
        /// <param name="newYearTemplate">A FiscalYearTemplate object.</param>        
        public IDataResponse<FiscalYear> CopyFiscalYear(Guid sourceFiscalYearId, FiscalYearTemplate newYearTemplate)
        {
            var response = new DataResponse<FiscalYear>();

            try
            {
                Guid id = UnitOfWork.GetBusinessLogic<ClosingLogic>().CreateNewFiscalYear(sourceFiscalYearId, newYearTemplate.Name, newYearTemplate.StartDate.Date, newYearTemplate.CopyInactiveAccounts);

                response.Data = UnitOfWork.GetById<FiscalYear>(id, IncludesForSingle);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return ProcessIDataResponseException(ex);
            }
            return response;
        }

        private void SetFiscalPeriods(FiscalYear entityToSave)
        {
            if (entityToSave.NumberOfPeriods > 0 && entityToSave.StartDate.HasValue)
            {
                if (entityToSave.FiscalPeriods == null)
                    entityToSave.FiscalPeriods = new List<FiscalPeriod>();

                List<DateTime[]> dates = _logic.CalculateDefaultStartEndDates(entityToSave.StartDate.Value, entityToSave.NumberOfPeriods, entityToSave.HasAdjustmentPeriod);
                for (int number = 0; number < entityToSave.NumberOfPeriods; number++)
                {
                    FiscalPeriod period = entityToSave.FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == number + 1);
                    if (period == null)
                    {
                        period = new FiscalPeriod()
                        {
                            PeriodNumber = number + 1,
                            FiscalYear = entityToSave,
                            Status = FiscalPeriodStatus.Open                            
                        };
                        UnitOfWork.Insert(period);
                    }
                    period.StartDate = dates[number][0];
                    period.EndDate = dates[number][1];
                    period.IsAdjustmentPeriod = entityToSave.HasAdjustmentPeriod && number == entityToSave.NumberOfPeriods - 1;
                }

                foreach (var period in entityToSave.FiscalPeriods.Where(p => p.PeriodNumber > entityToSave.NumberOfPeriods).ToList())
                {
                    UnitOfWork.Delete(period);
                }
            }
        }

        protected override bool ProcessJTokenUpdate(IEntity entity, string name, JToken token)
        {
            if (entity is FiscalYear)
            {
                var year = (FiscalYear)entity;

                // Operations based on FiscalYear.Status
                if (StringHelper.IsSameAs(name, nameof(FiscalYear.Status)))
                {
                    // To re-close a fiscal year, set status to "reclose"
                    if (token.ToString().ToLower() == "reclose")
                    {
                        var closingLogic = UnitOfWork.GetBusinessLogic<ClosingLogic>();
                        closingLogic.RecloseFiscalYear(year.Id);
                    }
                    else
                    {
                        // To close or re-open a fiscal year, change the status.
                        var status = EnumHelper.ConvertToEnum<FiscalYearStatus>(token, year.Status);
                        if (status != year.Status)
                        {
                            var closingLogic = UnitOfWork.GetBusinessLogic<ClosingLogic>();
                            if (status == FiscalYearStatus.Closed)
                            {
                                closingLogic.CloseFiscalYear(year.Id); // Changing to Closed will close the fiscal year.
                            }
                            else if (status == FiscalYearStatus.Open || status == FiscalYearStatus.Reopened)
                            {
                                closingLogic.ReopenFiscalYear(year.Id); // Changing to Open or Reopened will re-open the period.
                            }
                        }
                    }

                    return true;
                }

                // Operations for FiscalYear.NumberOfPeriods
                else if (StringHelper.IsSameAs(name, nameof(FiscalYear.NumberOfPeriods)))
                {
                    if ((int)token != year.NumberOfPeriods)
                    {
                        if (year.Status != FiscalYearStatus.Empty)
                        {
                            throw new ValidationException(UserMessagesGL.FiscalYearPeriodsChanged);
                        }
                        year.NumberOfPeriods = (int)token;
                        SetFiscalPeriods(year);
                    }
                    return true;
                }
                // Operations for FiscalYear.HasAdjustmentPeriod
                else if (StringHelper.IsSameAs(name, nameof(FiscalYear.HasAdjustmentPeriod)))
                {
                    if ((bool)token != year.HasAdjustmentPeriod)
                    {
                        // Changing adjustment period
                        if (year.Status != FiscalYearStatus.Empty)
                        {
                            throw new ValidationException(UserMessagesGL.FiscalYearAdjustPeriodChanged);
                        }
                        year.HasAdjustmentPeriod = (bool)token;
                        SetFiscalPeriods(year);
                    }
                    return true;
                }
                // Operations for FiscalYear.StartDate
                else if (StringHelper.IsSameAs(name, nameof(FiscalYear.StartDate)))
                {
                    if (token.ToString() != year.StartDate.ToShortDateString())
                    {
                        throw new ValidationException(UserMessagesGL.FiscalYearDatesChanged);
                    }
                    return true;
                }
                // Operations for FiscalYear.EndDate
                else if (StringHelper.IsSameAs(name, nameof(FiscalYear.EndDate)))
                {
                    if (token.ToString() != year.EndDate.ToShortDateString())
                    {
                        throw new ValidationException(UserMessagesGL.FiscalYearDatesChanged);
                    }
                    return true;
                }

            }
            return base.ProcessJTokenUpdate(entity, name, token);
        }
    }
}
