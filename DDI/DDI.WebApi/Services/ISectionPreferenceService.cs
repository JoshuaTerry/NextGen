using System.Collections.Generic;
using DDI.Data.Models.Client.Core;
using DDI.Shared;

namespace DDI.WebApi.Services
{
    public interface ISectionPreferenceService
    {
        IDataResponse<List<SectionPreference>> GetPreferencesBySectionName(string sectionName); 
    }
}