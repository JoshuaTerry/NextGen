using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;
using WebGrease.Css.Extensions;

namespace DDI.Services
{
    public class PaymentMethodService : ServiceBase<PaymentMethod>
    {
        protected override Action<PaymentMethod> FormatEntityForGet => p => SetDateTimeKind(p, q => q.StatusDate);
    }
}
