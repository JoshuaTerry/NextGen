using System;
using System.Linq;
using DDI.Business.Common;
using DDI.Business.CRM;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;

namespace DDI.Services
{
    public class AddressService : ServiceBase<Address>, IAddressService
    {
        private IRepository<Address> _repository;
        private AddressLogic _addressLogic;
        private ZipLookup _zipLookup;

        public AddressService(IUnitOfWork uow) : base(uow)
        {
            _addressLogic = uow.GetBusinessLogic<AddressLogic>();
            _zipLookup = new ZipLookup(uow);
            _repository = uow.GetRepository<Address>();
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

            var addressToFormat = new Address
            {
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                City = city,
                PostalCode = zip,
                Country = country,
                County = county,
                State = state
            };

            string resultAddress = string.Empty;

            _zipLookup.GetZipPlus4(ref addressToFormat, out resultAddress);
            UpdateAddressRegions(addressToFormat);

            IDataResponse<Address> response = new DataResponse<Address>();
            response.Data = addressToFormat;
            return response;
        }


        #endregion Public Methods

        #region Private Methods

        private void UpdateAddressRegions(Address address)
        {
            RegionLogic rl = UnitOfWork.GetBusinessLogic<RegionLogic>();
            var regions = rl.GetRegionsByAddress(address.CountryId, address.StateId, address.CountyId, address.City, address.PostalCode);

            address.Region1 = regions.Where(r => r.Level == 1).FirstOrDefault();
            address.Region1Id = address.Region1?.Id;
            address.Region2 = regions.Where(r => r.Level == 2).FirstOrDefault();
            address.Region2Id = address.Region2?.Id;
            address.Region3 = regions.Where(r => r.Level == 3).FirstOrDefault();
            address.Region3Id = address.Region3?.Id;
            address.Region4 = regions.Where(r => r.Level == 4).FirstOrDefault();
            address.Region4Id = address.Region4?.Id;
        }

        #endregion
    }
}
