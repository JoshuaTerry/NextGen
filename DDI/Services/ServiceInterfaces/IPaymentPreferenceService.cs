using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.CP;
using Newtonsoft.Json.Linq;

namespace DDI.Services.ServiceInterfaces
{
    public interface IPaymentPreferenceService : IService<PaymentMethodBase>
    {
        IDataResponse GetPaymentMethods();
    }
}
