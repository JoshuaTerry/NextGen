using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.Services
{
    public class ConstituentAddressService : ServiceBase<ConstituentAddress>, IConstituentAddressService
    {
        private IUnitOfWork _unitOfWork;
        public ConstituentAddressService() : this(new UnitOfWorkEF()) { }

        public ConstituentAddressService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public override IDataResponse<ConstituentAddress> GetById(Guid id)
        {
            var result = UnitOfWork.GetRepository<ConstituentAddress>().Entities.Include("Address").Include("AddressType").Where(a => a.Id == id).FirstOrDefault();
            return GetIDataResponse(() => result);
        }
    }
}
