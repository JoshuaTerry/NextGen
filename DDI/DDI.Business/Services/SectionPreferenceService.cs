using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDI.Data.Models.Client;
using DDI.Shared;
using DDI.Data;

namespace DDI.Business.Services
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
        public IDataResponse<List<SectionPreference>> GetPreferencesBySectionName(string sectionName)
        {
            var results = _repository.Entities.Where(p => p.SectionName == sectionName).ToList();
            return GetIDataResponse(() => results);
        }
    }
}