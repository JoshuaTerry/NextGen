using DDI.Shared.Models.Common;
using DDI.Shared;
using System;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Services
{
    public interface ILocationService
    {
        IDataResponse<Address> RefineAddress(string addressLine1, string addressLine2, string city, Guid? countryId, Guid? countyId, Guid? stateId, string zip);        
    }
}
