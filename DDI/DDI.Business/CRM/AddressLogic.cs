
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Common;

namespace DDI.Business.CRM
{
    /// <summary>
    /// Address business logic
    /// </summary>
    public class AddressLogic : EntityLogicBase<Address>
    {
        #region Private Fields


        #endregion

        #region Constructors 

        public AddressLogic() : this(new UnitOfWorkEF()) { }

        public AddressLogic(IUnitOfWork uow) : base(uow)
        {
        }

        /// <summary>
        /// Load the Country, State, and County properties for an address.
        /// </summary>
        /// <param name="address"></param>
        public void LoadAllProperties(Address address)
        {
            if (address.CountryId != null)
            {
                var repo = UnitOfWork.GetRepository<Country>();

                address.Country = repo.GetLocal().FirstOrDefault(p => p.Id == address.CountryId) ??
                                  repo.Entities.FirstOrDefault(p => p.Id == address.CountryId);                
            }
            else
            {
                address.Country = null;
            }

            if (address.StateId != null)
            {
                var repo = UnitOfWork.GetRepository<State>();

                address.State = repo.GetLocal().FirstOrDefault(p => p.Id == address.StateId) ??
                                  repo.Entities.FirstOrDefault(p => p.Id == address.StateId);
            }
            else
            {
                address.State = null;
            }

            if (address.CountyId != null)
            {
                var repo = UnitOfWork.GetRepository<County>();

                address.County = repo.GetLocal().FirstOrDefault(p => p.Id == address.CountyId) ??
                                  repo.Entities.FirstOrDefault(p => p.Id == address.CountyId);
            }
            else
            {
                address.County = null;
            }

        }
        #endregion

        #region Public Methods



        public override void Validate(Address address)
        {
            base.Validate(address);           
        }

        #endregion

        #region Inner Classes

        #endregion

    }
}
