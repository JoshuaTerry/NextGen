using System.Collections.Generic;
using System.Linq;
using DDI.Business.Tests.Common.DataSources;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class RegionAreaDataSource
    {
        public static IList<RegionArea> GetDataSource(IUnitOfWork uow)
        {
            IList<RegionArea> existing = uow.GetRepositoryDataSource<RegionArea>();
            if (existing != null)
            {
                return existing;
            }

            var regions = RegionDataSource.GetDataSource(uow);
            var states = StateDataSource.GetDataSource(uow);
            var counties = CountyDataSource.GetDataSource(uow);

            var indiana = states.FirstOrDefault(p => p.Country.ISOCode == AddressDefaults.DefaultCountryCode && p.StateCode == "IN");
            var ohio = states.FirstOrDefault(p => p.Country.ISOCode == AddressDefaults.DefaultCountryCode && p.StateCode == "OH");

            var areas = new List<RegionArea>();

            areas.Add(new RegionArea()
            {
                Region = regions.FirstOrDefault(p => p.Level == 1 && p.Code == "IN"),
                State = indiana,
                Priority = 1,
                Level = 1,
                Id = GuidHelper.NewSequentialGuid()
            });

            areas.Add(new RegionArea()
            {
                Region = regions.FirstOrDefault(p => p.Level == 1 && p.Code == "OH"),
                Country = ohio.Country,
                State = ohio,
                Priority = 1,
                Level = 1,
                Id = GuidHelper.NewSequentialGuid()
            });

            areas.Add(new RegionArea()
            {
                Region = regions.FirstOrDefault(p => p.Level == 2 && p.Code == "INDY"),
                Country = indiana.Country,
                PostalCodeLow = "46201",
                PostalCodeHigh = "46299",
                Priority = 1,
                Level = 2,
                Id = GuidHelper.NewSequentialGuid()
            });

            areas.Add(new RegionArea()
            {
                Region = regions.FirstOrDefault(p => p.Level == 2 && p.Code == "HAML"),
                Country = indiana.Country,
                County = indiana.Counties.FirstOrDefault(p => p.Description == "Hamilton"),
                State = indiana,
                Priority = 1,
                Level = 2,
                Id = GuidHelper.NewSequentialGuid()
            });

            areas.Add(new RegionArea()
            {
                Region = regions.FirstOrDefault(p => p.Level == 2 && p.Code == "FISH"),
                Country = indiana.Country,
                County = indiana.Counties.FirstOrDefault(p => p.Description == "Hamilton"),
                State = indiana,
                City = "Fishers",
                Priority = 2,
                Level = 2,
                Id = GuidHelper.NewSequentialGuid()
            });

            uow.CreateRepositoryForDataSource(areas);
            return areas;
        }    

    }

    
}
