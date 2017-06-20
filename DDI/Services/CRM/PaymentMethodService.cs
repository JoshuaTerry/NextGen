using DDI.Shared.Models.Client.CP;
using System;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics.CRM;

namespace DDI.Services
{
    public class PaymentMethodService : ServiceBase<PaymentMethod>
    {
        public PaymentMethodService(IUnitOfWork uow) : base(uow) { }

        protected override Action<PaymentMethod, string> FormatEntityForGet => (p, fields) => SetDateTimeKind(p, q => q.StatusDate);

        public override IDataResponse<PaymentMethod> Add(PaymentMethod entity)
        {
            Constituent constituent = null;

            if (entity.ConstituentId.HasValue)
            {
                constituent = UnitOfWork.GetById<Constituent>(entity.ConstituentId.Value, p => p.PaymentMethods);
            }

            if (constituent == null)
            {
                return GetErrorResponse<PaymentMethod>(UserMessagesCRM.ConstituentIdInvalid);
            }

            constituent.PaymentMethods.Add(entity);

            return base.Add(entity);
        }
    }
}
