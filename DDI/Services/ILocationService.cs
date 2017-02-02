using DDI.Shared.Models.Common;
using DDI.Shared;
using System;

namespace DDI.Services
{
    public interface ILocationService
    {
        IDataResponse<ZipLookupInfo> RefineAddress(string addressLine1, string addressLine2, string city, Guid? countryId, Guid? countyId, Guid? stateId, string zip);        
    }
}
