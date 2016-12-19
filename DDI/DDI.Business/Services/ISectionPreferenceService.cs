using System;
using System.Collections.Generic; 
using DDI.Data.Models.Client;
using DDI.Shared;

namespace DDI.Business.Services
{
    public interface ISectionPreferenceService
    {
        IDataResponse<List<SectionPreference>> GetPreferencesBySectionName(string sectionName); 
    }
}