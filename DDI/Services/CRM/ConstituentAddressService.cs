using DDI.Business.CRM;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;

namespace DDI.Services
{
    public class ConstituentAddressService : ServiceBase<ConstituentAddress>
    {
        private ConstituentAddressLogic _logic;

        public ConstituentAddressService(IUnitOfWork uow, ConstituentAddressLogic logic) : base(uow)
        {
            _logic = logic;
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