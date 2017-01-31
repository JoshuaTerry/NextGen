using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Enums.CP;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CP;
using Newtonsoft.Json.Linq;

namespace DDI.Services
{
    public class PaymentPreferenceService : ServiceBase<PaymentMethodBase>, IPaymentPreferenceService
    {
        public IDataResponse GetPaymentMethods()
        {
            var response = GetDataResponse(EnumHelper.GetDescriptions<PaymentMethod>);

            return response;
        }

        public IDataResponse GetAccountTypes()
        {
            var response = GetDataResponse(EnumHelper.GetDescriptions<EFTAccountType>);

            return response;
        }

        public IDataResponse GetPaymentMethodStatuses()
        {
            var response = GetDataResponse(EnumHelper.GetDescriptions<PaymentMethodStatus>);

            return response;
        }
    }
}
