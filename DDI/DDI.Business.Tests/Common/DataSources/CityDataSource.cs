using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.Tests.Common.DataSources
{
    public static class CityDataSource
    {
        public static IList<City> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<City> existing = uow.GetRepositoryOrNull<City>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            var states = StateDataSource.GetDataSource(uow);
            var counties = CountyDataSource.GetDataSource(uow);
            var cityNames = new List<CityName>();
            var cities = new List<City>();

            // Only adding a few cities for Hamilton and Marion counties...

            var state = states.FirstOrDefault(p => p.StateCode == "IN" && p.Country.CountryCode == AddressDefaults.DefaultCountryCode);
            var county = counties.FirstOrDefault(p => p.StateId == state.Id && p.Description == "Hamilton");

            if (county != null)
            {
                var list = new List<City>();

                list.Add(AddCity(state, county, cityNames, "Noblesville", "1854180", 51969, 81.77, 40.0499, -86.0215));
                list.Add(AddCity(state, county, cityNames, "Carmel", "1810342", 79191, 109.81, 39.9729, -86.1079));
                list.Add(AddCity(state, county, cityNames, "Fishers", "1823278", 76794, 102.97, 39.9562, -86.0128));

                state.Cities = list;
                county.Cities = list;
                cities.AddRange(list);
            }

            county = counties.FirstOrDefault(p => p.StateId == state.Id && p.Description == "Marion");

            if (county != null)
            {
                var list = new List<City>();

                list.Add(AddCity(state, county, cityNames, "Indianapolis", "1836003", 820445, 4.93, 39.7909, -86.1477));
                list.Add(AddCity(state, county, cityNames, "Beech Grove", "1804204", 14192, -4.62, 39.7177, -86.0913));
                list.Add(AddCity(state, county, cityNames, "Lawrence", "1842426", 46001, 18.21, 39.8627, -85.9943));

                state.Cities = list;
                county.Cities = list;
                cities.AddRange(list);
            }

            uow.CreateRepositoryForDataSource(cities);
            uow.CreateRepositoryForDataSource(cityNames);
            return cities;
        }    

        private static City AddCity(State state, County county, IList<CityName> cityNames, string description, string placeCode, int population, double popPctCng, double coordinateNS, double coordinateEW)
        {

            var city = new City()
            {
                State = state,
                County = county,
                PlaceCode = placeCode,
                Population = population,
                PopulationPercentageChange = (decimal)popPctCng,
                CoordinateEW = (decimal)coordinateEW,
                CoordinateNS = (decimal)coordinateNS,
                Id = GuidHelper.NextGuid()
            };

            var cityName = new CityName()
            {
                City = city,
                Description = description,
                IsPreferred = true,
                Id = GuidHelper.NextGuid()
            };

            city.CityNames = new List<CityName>() { cityName };
            cityNames.Add(cityName);

            return city;
        }

    }

    
}
