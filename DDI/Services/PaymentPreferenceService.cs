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
using DDI.Shared.Models;

namespace DDI.Services
{
    public class PaymentPreferenceService : ServiceBase<PaymentMethod>, IPaymentPreferenceService
    {
        public IDataResponse GetPaymentMethods()
        {
            var response = GetDataResponse(EnumHelper.GetDescriptions<PaymentMethod>);
            response.TotalResults = response.Data.Count;

            return response;
        }

        public IDataResponse GetAccountTypes()
        {
            var response = GetDataResponse(EnumHelper.GetDescriptions<EFTAccountType>);
            response.TotalResults = response.Data.Count;

            return response;
        }

        public IDataResponse GetPaymentMethodStatuses()
        {
            var response = GetDataResponse(EnumHelper.GetDescriptions<PaymentMethodStatus>);
            response.TotalResults = response.Data.Count;

            return response;
        }

        public IDataResponse<T> Save<T>(T entityToSave) where T : EntityBase
        {
            var response = new DataResponse<T>();
            try
            {
                UnitOfWork.GetRepository<T>().Insert(entityToSave);
                UnitOfWork.SaveChanges();
                response.Data = UnitOfWork.GetRepository<T>().GetById(entityToSave.Id);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
    }
}
