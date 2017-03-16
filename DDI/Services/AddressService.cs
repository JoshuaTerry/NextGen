﻿using System;
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
        private readonly ZipLookup _zipLookup;

        public AddressService()
            : this(new UnitOfWorkEF())
        {
        }

        public AddressService(IUnitOfWork uow)
            : this(uow, new AddressLogic(uow), uow.GetRepository<Address>(), new ZipLookup(uow))
        {
        }

        private AddressService(IUnitOfWork uow, AddressLogic addressLogic, IRepository<Address> repository, ZipLookup zipLookup)
            :base(uow)
        {
            _addressLogic = addressLogic;
            _repository = repository;
            _zipLookup = zipLookup;
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
            IDataResponse<Address> response = new DataResponse<Address>();
            response.Data = addressToFormat;
            return response;
        }


        #endregion Public Methods
    }
}