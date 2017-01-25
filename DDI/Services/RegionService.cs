using DDI.Business.CRM;
using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Services
{
    public class RegionService : ServiceBase<Region>, IRegionService
    {
        private IUnitOfWork _unitOfWork;
        private RegionLogic _logic;
        public RegionService()
        {
            Initialize(new UnitOfWorkEF(), new RegionLogic());
        }

        public RegionService(IUnitOfWork uow, RegionLogic logic)
        {
            Initialize(uow, logic);
        }

        private void Initialize(IUnitOfWork uow, RegionLogic logic)
        {
            _unitOfWork = uow;
            _logic = logic;
        }
        public IDataResponse<List<Region>> GetRegionsByLevel(Guid? id, int level)
        {
            List<Region> result;
            if (id != null && id != Guid.Empty)
            {
                result = UnitOfWork.GetRepository<Region>().Entities.Where(r => r.Level == level && r.ParentRegionId == id).ToList();
            }
            else
            {
                result = UnitOfWork.GetRepository<Region>().Entities.Where(r => r.Level == level).ToList();
            }
             
            return GetIDataResponse(() => result.ToList());
        }

        public IDataResponse<List<Region>> GetAll()
        {
            var result = UnitOfWork.GetRepository<Region>().Entities.Where(r => r.ParentRegionId == null);
            return GetIDataResponse(() => result.ToList());
        }

        public IDataResponse Add(Region entity)
        {
            return base.Add(entity);
        }

        public IDataResponse<List<Region>> GetRegionsByAddress(Guid? countryid, Guid? stateId, Guid? countyId, string city, string zipcode)
        {            
            var result =_logic.GetRegionsByAddress(countryid, stateId, countyId, city, zipcode);
            return GetIDataResponse(() => result.ToList());
        }         
    }
}
