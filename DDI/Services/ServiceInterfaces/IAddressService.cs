using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System;

namespace DDI.Services.ServiceInterfaces
{
    public interface IAddressService : IService<Address>
    {
        IDataResponse<Address> RefineAddress(string addressLine1, string addressLine2, string city, Guid? countryId, Guid? countyId, Guid? stateId, string zip);        
    }
}
