using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;

namespace DDI.Services
{
    public interface ISectionPreferenceService
    {
        IDataResponse<dynamic> GetPreferencesBySectionName(string sectionName); 
    }
}