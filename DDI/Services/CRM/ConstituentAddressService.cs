using DDI.Business;
using DDI.Business.CRM;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using DDI.Shared.Models;

namespace DDI.Services
{
    public class ConstituentAddressService : ServiceBase<ConstituentAddress>
    {
        private AddressService _addressService;
        private IUnitOfWork _unitOfWork;
        private readonly ConstituentAddressLogic _logic;


        public ConstituentAddressService()
            : this(new AddressService(), new UnitOfWorkEF())
        {

        }

        internal ConstituentAddressService(IAddressService addressService, IUnitOfWork unitOfWork) : this(addressService, unitOfWork, new ConstituentAddressLogic(unitOfWork))
        {
        }


        internal ConstituentAddressService(IAddressService addressService, IUnitOfWork unitOfWork, ConstituentAddressLogic logic)
        {
            _logic = logic;
            _addressService = addressService as AddressService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Override to handle updating Address passed via ConstituentAddress.
        /// </summary>
        protected override bool ProcessJTokenUpdate(IEntity entity, string name, JToken token)
        {
            if (name == nameof(ConstituentAddress.Address) && entity is ConstituentAddress)
            {
                if (token is JObject)
                {
                    var constituentAddress = (ConstituentAddress)entity;
                    constituentAddress.Address = AddUpdateFromJObject<Address>((JObject)token);
                    constituentAddress.AddressId = constituentAddress.Address?.Id;
                    return true;
                }
            }
            return false;
        }

        public override IDataResponse<ConstituentAddress> Add(ConstituentAddress entity)
        {
            _logic.Validate(entity);
            return base.Add(entity);
        }
    }
}