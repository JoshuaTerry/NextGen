using System.Collections.Generic;
using System.Linq;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.Tests.Common.DataSources
{
    public static class CountyDataSource
    {
        public static IList<County> GetDataSource(IUnitOfWork uow)
        {
            IList<County> existing = uow.GetRepositoryDataSource<County>();
            if (existing != null)
            {
                return existing;
            }

            var states = StateDataSource.GetDataSource(uow);
            var state = states.FirstOrDefault(p => p.StateCode == "IN" && p.Country.CountryCode == AddressDefaults.DefaultCountryCode);
            var countiesList = new List<County>();

            if (state != null)
            {
                var list = new List<County>();

                list.Add(AddCounty(state, "Boone", "18011"));
                list.Add(AddCounty(state, "Brown", "18013"));
                list.Add(AddCounty(state, "Elkhart", "18039"));
                list.Add(AddCounty(state, "Hamilton", "18057"));
                list.Add(AddCounty(state, "Hancock", "18059"));
                list.Add(AddCounty(state, "Hendricks", "18063"));
                list.Add(AddCounty(state, "Henry", "18065"));
                list.Add(AddCounty(state, "Johnson", "18081"));
                list.Add(AddCounty(state, "Marion", "18097"));
                list.Add(AddCounty(state, "Monroe", "18105"));
                list.Add(AddCounty(state, "Morgan", "18109"));
                list.Add(AddCounty(state, "Putnam", "18133"));
                list.Add(AddCounty(state, "Shelby", "18145"));
                list.Add(AddCounty(state, "Boone", "18011"));

                state.Counties = list;
                countiesList.AddRange(list);
            }

            uow.CreateRepositoryForDataSource(countiesList);
            return countiesList;
        }    

        private static County AddCounty(State state, string description, string fips)
        {
            return new County()
            {
                State = state,
                Description = description,
                FIPSCode = fips,
                Id = GuidHelper.NewSequentialGuid()
            };
        }

    }

    
}
