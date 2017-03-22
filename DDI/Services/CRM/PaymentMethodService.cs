using DDI.Shared.Models.Client.CP;
using System;

namespace DDI.Services
{
    public class PaymentMethodService : ServiceBase<PaymentMethod>
    {
        protected override Action<PaymentMethod> FormatEntityForGet => p => SetDateTimeKind(p, q => q.StatusDate);
    }
}
