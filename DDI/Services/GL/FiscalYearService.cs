using System;
using System.Collections.Generic;
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
            var response = base.Add(entityToSave);

            SetFiscalPeriods(entityToSave);

            return response;
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
                //return ProcessIDataResponseException(ex);
                ProcessIDataResponseException(ex);
                throw ex;
            }
            return response;
        }

        private void SetFiscalPeriods(FiscalYear entityToSave)
        {
            if (entityToSave.NumberOfPeriods > 0)
            {
                if (entityToSave.FiscalPeriods == null)
                    entityToSave.FiscalPeriods = new List<FiscalPeriod>();

                int currentYear = entityToSave.StartDate.HasValue ? entityToSave.StartDate.Value.Year : DateTime.Now.Year;

                CreateFiscalPeriods(entityToSave, currentYear);

                AddAdjustmentPeriod(entityToSave, currentYear);
            }
        }

        private void CreateFiscalPeriods(FiscalYear entityToSave, int currentYear)
        {
            switch (entityToSave.NumberOfPeriods)
            {
                case 1:
                    
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 6:
                    break;
                case 12:
                    break;
                default:
                    break;
            }
        }

        private void AddAdjustmentPeriod(FiscalYear entityToSave, int currentYear)
        {
            if (entityToSave.HasAdjustmentPeriod)
            {
                entityToSave.FiscalPeriods.Add(new FiscalPeriod()
                {
                    PeriodNumber = entityToSave.NumberOfPeriods + 1,
                    StartDate = new DateTime(currentYear, 12, 31),
                    EndDate = new DateTime(currentYear, 12, 31),
                    IsAdjustmentPeriod = true,
                    Status = Shared.Enums.GL.FiscalPeriodStatus.Open
                });
            }
        }

        protected override bool ProcessJTokenUpdate(IEntity entity, string name, JToken token)
        {
            if (entity is FiscalYear)
            {
                var year = (FiscalYear)entity;

                // Operations based on FiscalYear.Status
                if (string.Compare(name, nameof(FiscalYear.Status), true) == 0)
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
            }
            return base.ProcessJTokenUpdate(entity, name, token);
        }
    }
}
