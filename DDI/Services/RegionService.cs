using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Services
{
    public class RegionService : ServiceBase<Region>, IRegionService
    {
        private IRepository<Region> _repository;

        public RegionService() : this (new Repository<Region>()) { }
        public RegionService(IRepository<Region> repo)
        {
            _repository = repo;
        }

        public IDataResponse<List<Region>> GetByLevel(int level, Guid? id)
        {
            var results = _repository.Entities.Where(r => r.Level == level).ToList();
            if (id.HasValue)
                results = results.Where(r => r.ParentRegionId == id.Value).ToList();

            return GetIDataResponse(() => results);
        }
    }
}
