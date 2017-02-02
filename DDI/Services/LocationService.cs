using System;
using System.Linq;
using DDI.Shared;
using DDI.Data;
using DDI.Shared.Models.Common;
using DDI.Business.Common;
using DDI.Business.CRM;

namespace DDI.Services
{
    public class LocationService : ILocationService
    {
        private IUnitOfWork _unitOfWork;
        private AddressLogic _logic;
        public LocationService() : this(new UnitOfWorkEF(), new AddressLogic()) { }
        public LocationService(IUnitOfWork uow, AddressLogic logic)
        {
            _unitOfWork = uow;
            _logic = logic;
        }
        
        public IDataResponse<ZipLookupInfo> RefineAddress(string addressLine1, string addressLine2, string city, Guid? countryId, Guid? countyId, Guid? stateId, string zip)
        {
            Country country = null;
            County county = null;
            State state = null;

            if (countryId != null)
                country =_logic.Countries.FirstOrDefault(c => c.Id == countryId);
                
            if (countyId != null)
                county = _logic.Counties.FirstOrDefault(c => c.Id == countyId);

            if (stateId != null)
                state = _logic.States.FirstOrDefault(s => s.Id == stateId);

            var address = new ZipLookupInfo();
            address.AddressLine1 = addressLine1;
            address.AddressLine2 = addressLine2;
            address.City = city;
            address.PostalCode = zip;

            if (country != null)
                address.Country = country;

            if (county != null)
                address.County = county;

            if (state != null)
                address.State = state;

            string resultAddress = string.Empty;
            ZipLookup zl = new ZipLookup();
            zl.Zip4Lookup(address, out resultAddress);

            IDataResponse<ZipLookupInfo> response = new DataResponse<ZipLookupInfo>();
            response.Data = address;
            return response;
        }
    }
}
