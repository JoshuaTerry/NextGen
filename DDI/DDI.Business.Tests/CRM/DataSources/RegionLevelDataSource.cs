using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class RegionLevelDataSource
    {
        public static IList<RegionLevel> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<RegionLevel> existing = uow.GetRepositoryOrNull<RegionLevel>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }
            
            var levels = new List<RegionLevel>();
            levels.Add(new RegionLevel() { Level = 1, Abbreviation = "Reg", Label = "Region", IsChildLevel = false,  Id = GuidHelper.NewSequentialGuid() });
            levels.Add(new RegionLevel() { Level = 2, Abbreviation = "Comm", Label = "Community", IsChildLevel = true, Id = GuidHelper.NewSequentialGuid() });

            uow.CreateRepositoryForDataSource(levels);
            return levels;
        }    

    }

    
}
