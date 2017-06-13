using System;
using System.Collections.Generic;
using DDI.Business.GL;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;

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
    }
}
