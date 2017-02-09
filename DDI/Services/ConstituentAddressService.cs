using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;

namespace DDI.Services
{
    public class ConstituentAddressService : ServiceBase<ConstituentAddress>
    {
        private AddressService _addressService;
        private IUnitOfWork _unitOfWork;

        public ConstituentAddressService()
            :this(new AddressService(), new UnitOfWorkEF())
        {
            
        }

        internal ConstituentAddressService(IAddressService addressService, IUnitOfWork unitOfWork)
        {
            _addressService = addressService as AddressService;
            _unitOfWork = unitOfWork;
        }

        public override IDataResponse<ConstituentAddress> Update(Guid id, JObject changes)
        {
            var response = new DataResponse<ConstituentAddress>();
            Dictionary<string, object> changedProperties = new Dictionary<string, object>();
            try
            {
                foreach (var property in changes)
                {
                    if (property.Key == "Address")
                    {
                        _addressService.Update(property.Value.ToObject<Address>().Id, JObject.FromObject(property.Value));
                    }
                    else
                    {
                        var convertedProperty = JsonExtensions.ConvertToType<ConstituentAddress>(property);
                        changedProperties.Add(convertedProperty.Key, convertedProperty.Value);

                        _unitOfWork.GetRepository<ConstituentAddress>().UpdateChangedProperties(id, changedProperties);
                        _unitOfWork.SaveChanges();
                    }
                }

                response.Data = _unitOfWork.GetRepository<ConstituentAddress>().GetById(id, IncludesForSingle);

                return response;
            }
            catch (Exception ex)
            {
                return base.ProcessIDataResponseException(ex);
            }
        }
    }
}