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
    public class FiscalPeriodService : ServiceBase<FiscalPeriod>, IFiscalPeriodService
    {
        public FiscalPeriodService(IUnitOfWork uow) : base(uow)
        {
        }

        public IDataResponse<List<ICanTransmogrify>> GetFiscalPeriodsByAccountId(Guid accountId)
        {
            Account account = UnitOfWork.GetById<Account>(accountId);
            if (account != null)
            {
                return GetAllWhereExpression(fp => fp.FiscalYearId == account.FiscalYearId);
            }

            return GetErrorResponse<List<ICanTransmogrify>>("Invalid account ID.");
        }

        protected override bool ProcessJTokenUpdate(IEntity entity, string name, JToken token)
        {
            if (entity is FiscalPeriod)
            {
                var period = (FiscalPeriod)entity;
                
                // To close or re-open a fiscal period, change the status:
                if (string.Compare(name, nameof(FiscalPeriod.Status), true) == 0)
                {
                    var status = EnumHelper.ConvertToEnum<FiscalPeriodStatus>(token);
                    if (status != period.Status)
                    {
                        var closingLogic = UnitOfWork.GetBusinessLogic<ClosingLogic>();
                        if (status == FiscalPeriodStatus.Closed)
                        {
                            closingLogic.CloseFiscalPeriod(period.Id); // Changing to Closed will close the period.
                        }
                        else if (status == FiscalPeriodStatus.Open || status == FiscalPeriodStatus.Reopened)
                        {
                            closingLogic.ReopenFiscalPeriod(period.Id); // Changing to Open or Reopened will re-open the period.
                        }                        
                    }
                    return true;
                }
                else if (string.Compare(name, nameof(FiscalPeriod.PeriodNumber), true) == 0)
                {
                    if (period.PeriodNumber.ToString() != token.ToString())
                    {
                        throw new ValidationException(UserMessagesGL.FiscalPeriodNumberChanged);
                    }
                }
            }
            return base.ProcessJTokenUpdate(entity, name, token);
        }

        

    }
}
