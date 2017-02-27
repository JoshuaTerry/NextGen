using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class RegionDataSource
    {
        public static IList<Region> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<Region> existing = uow.GetRepositoryOrNull<Region>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            IList<RegionLevel> levels = RegionLevelDataSource.GetDataSource(uow);

            var regions = new List<Region>();
            regions.Add(new Region() { Code = "IN", Level = 1, Name = "Indiana", ChildRegions = new List<Region>(), Id = GuidHelper.NextGuid() });
            regions.Add(new Region() { Code = "OH", Level = 1, Name = "Ohio", ChildRegions = new List<Region>(), Id = GuidHelper.NextGuid() });

            Region indiana = regions[0];
            Region child;

            regions.Add(child = new Region() { Code = "INDY", Level = 2, Name = "Indianapolis", ParentRegion = indiana, Id = GuidHelper.NextGuid() });
            indiana.ChildRegions.Add(child);

            regions.Add(child = new Region() { Code = "FISH", Level = 2, Name = "Fishers", ParentRegion = indiana, Id = GuidHelper.NextGuid() });
            indiana.ChildRegions.Add(child);

            regions.Add(child = new Region() { Code = "HAML", Level = 2, Name = "Hamilton County", ParentRegion = indiana, Id = GuidHelper.NextGuid() });
            indiana.ChildRegions.Add(child);


            foreach (var level in levels)
            {
                level.Regions = regions.Where(p => p.Level == level.Level).ToList();
            }
            
            uow.CreateRepositoryForDataSource(regions);
            return regions;
        }    
    }    
}
