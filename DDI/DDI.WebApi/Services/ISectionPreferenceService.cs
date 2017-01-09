using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;

namespace DDI.WebApi.Services
{
    public interface ISectionPreferenceService
    {
        IDataResponse<List<SectionPreference>> GetPreferencesBySectionName(string sectionName); 
    }
}