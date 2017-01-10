using System.Collections.Generic;
using System.Linq;
using DDI.Data;
using DDI.Data.Models.Client.Core;
using DDI.Shared;

namespace DDI.WebApi.Services
{
    public class SectionPreferenceService : GenericServiceBase<SectionPreference>, ISectionPreferenceService
    {
        private IRepository<SectionPreference> _repository;

        public SectionPreferenceService() : this(new CachedRepository<SectionPreference>())
        { 
        }
        public SectionPreferenceService(IRepository<SectionPreference> repo) 
        {
            _repository = repo;
        }
        public IDataResponse<dynamic> GetPreferencesBySectionName(string sectionName)
        {
            var results = _repository.Entities.Where(p => p.SectionName == sectionName).ToList();
            return GetIDataResponse(() => results);
        }
    }
}