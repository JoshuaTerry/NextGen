using DDI.Shared.Models.Client.GL;
using DDI.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace DDI.Services.GL
{
    public class FiscalYearService : ServiceBase<FiscalYear>, IFiscalYearService
    {
        public FiscalYearService(IUnitOfWork uow) : base(uow)
        {
        }

        public IDataResponse<FiscalYear> Post(FiscalYear entityToSave)
        {
            var response = base.Add(entityToSave);

            SetFiscalPeriods(entityToSave);

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
