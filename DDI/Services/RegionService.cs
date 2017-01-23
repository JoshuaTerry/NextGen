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
            _logic = new RegionLogic(_unitOfWork);
        }
        public IDataResponse<List<Region>> GetRegionsByLevel(Guid? id, int level)
        {
            var result = UnitOfWork.GetRepository<Region>().Entities.Where(r => r.Level == level && r.ParentRegionId == id);
            return GetIDataResponse(() => result.ToList());
        }

        public override IDataResponse<List<Region>> GetAll()
        {
            var result = UnitOfWork.GetRepository<Region>().Entities.Where(r => r.ParentRegionId == null);
            return GetIDataResponse(() => result.ToList());
        }

        public IDataResponse<List<Region>> GetRegionsByAddress(Guid? countryid, Guid? stateId, Guid? countyId, string city, string zipcode)
        {            
            var result =_logic.GetRegionsByAddress(countryid, stateId, countyId, city, zipcode);
            return GetIDataResponse(() => result.ToList());
        }         
    }
}
