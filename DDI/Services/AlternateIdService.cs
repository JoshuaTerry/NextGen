using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Services
{
    public class AlternateIdService : ServiceBase<AlternateId>, IAlternateIdService
    {
        private IUnitOfWork _unitOfWork;
        public AlternateIdService() : this(new UnitOfWorkEF()) { }

        public AlternateIdService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public IDataResponse<List<AlternateId>> GetAlternateIdsByConstituent(Guid constituentId)
        {            
            List<AlternateId> result;
            result = UnitOfWork.GetRepository<AlternateId>().Entities.Where(a => a.ConstituentId == constituentId).ToList();
             
            return GetIDataResponse(() => result.ToList());
        }
    }
}
