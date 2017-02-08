using System;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Common; 
using System.Collections.Generic;
using System.Linq;
using DDI.Business.Common;
using DDI.Business.CRM;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Services
{
    public class AddressService : ServiceBase<Address>, IAddressService
    {
        private readonly IRepository<Address> _repository;
        private readonly AddressLogic _addressLogic;

        public AddressService()
            : this(new UnitOfWorkEF())
        {
        }

        public AddressService(IUnitOfWork uow)
            : this(uow, new AddressLogic(uow), uow.GetRepository<Address>())
        {
        }

        private AddressService(IUnitOfWork uow, AddressLogic addressLogic, IRepository<Address> repository)
            :base(uow)
        {
            _addressLogic = addressLogic;
            _repository = repository;
        }
        #region Public Methods
        public IDataResponse<Address> RefineAddress(string addressLine1, string addressLine2, string city, Guid? countryId, Guid? countyId, Guid? stateId, string zip)
        {
            Country country = null;
            County county = null;
            State state = null;

            if (countryId != null)
                country = _addressLogic.Countries.FirstOrDefault(c => c.Id == countryId);

            if (countyId != null)
                county = _addressLogic.Counties.FirstOrDefault(c => c.Id == countyId);

            if (stateId != null)
                state = _addressLogic.States.FirstOrDefault(s => s.Id == stateId);

            var zipLookupInfo = new ZipLookupInfo();
            zipLookupInfo.AddressLine1 = addressLine1;
            zipLookupInfo.AddressLine2 = addressLine2;
            zipLookupInfo.City = city;
            zipLookupInfo.PostalCode = zip;

            if (country != null)
                zipLookupInfo.Country = country;

            if (county != null)
                zipLookupInfo.County = county;

            if (state != null)
                zipLookupInfo.State = state;

            string resultAddress = string.Empty;
            ZipLookup zl = new ZipLookup();
            zl.GetZipPlus4(zipLookupInfo, out resultAddress);
            Address address = new Address
            {
                AddressLine1 = zipLookupInfo.AddressLine1,
                AddressLine2 = zipLookupInfo.AddressLine2,
                City = zipLookupInfo.City,
                PostalCode = zipLookupInfo.PostalCode,
                Country = zipLookupInfo.Country,
                County = zipLookupInfo.County,
                State = zipLookupInfo.State,
                CountryId = zipLookupInfo.Country.Id,
                CountyId = zipLookupInfo.County.Id,
                StateId = zipLookupInfo.State.Id,
            };
            IDataResponse<Address> response = new DataResponse<Address>();
            response.Data = address;
            return response;
        }


        #endregion Public Methods
    }
}
