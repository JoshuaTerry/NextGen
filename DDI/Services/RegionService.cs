using System;
using System.Collections.Generic;
using DDI.Business.CRM;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System.Linq;

namespace DDI.Services
{
    public class RegionService : ServiceBase<Region>, IRegionService
    {
        private readonly IRepository<Region> _repository;
        private readonly RegionLogic _regionLogic;

        public RegionService()
            : this(new UnitOfWorkEF())
        {
        }

        public RegionService(IUnitOfWork uow)
            : this(uow, new RegionLogic(uow), uow.GetRepository<Region>())
        {
        }

        private RegionService(IUnitOfWork uow, RegionLogic regionLogic, IRepository<Region> repository)
            :base(uow)
        {
            _regionLogic = regionLogic;
            _repository = repository;
        }

        public IDataResponse<List<Region>> GetRegionsByLevel(Guid? id, int level)
        {
            var result = UnitOfWork.GetRepository<Region>().GetEntities(IncludesForList);

            if (id != null && id != Guid.Empty)
            {
                result = result.Where(r => r.Level == level && r.ParentRegionId == id);
            }
            else
            {
                result = result.Where(r => r.Level == level);
            }

            var response = GetIDataResponse(() => result.ToList());
            response.TotalResults = response.Data.Count;

            return response;
        }
       
        public IDataResponse<List<Region>> GetRegionsByAddress(Guid? countryid, Guid? stateId, Guid? countyId, string city, string zipcode)
        {            
            var result =_regionLogic.GetRegionsByAddress(countryid, stateId, countyId, city, zipcode);

            var response = GetIDataResponse(() => result.ToList());
            response.TotalResults = response.Data.Count;

            return response;
        }
    }
}
