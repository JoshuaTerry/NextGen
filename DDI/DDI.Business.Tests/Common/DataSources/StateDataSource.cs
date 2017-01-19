using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Common;

namespace DDI.Business.Tests.Common.DataSources
{
    public static class StateDataSource
    {
        public static IList<State> GetDataSource(UnitOfWorkNoDb uow)
        {
            var countries = uow.GetEntities<Country>();
            var statesList = new List<State>();

            var country = countries.FirstOrDefault(p => p.ISOCode == "US");
            if (country != null)
            {
                var list = new List<State>();
                list.Add(AddState(country, "AK", "Alaska", "02"));
                list.Add(AddState(country, "AL", "Alabama", "01"));
                list.Add(AddState(country, "AR", "Arkansas", "05"));
                list.Add(AddState(country, "AZ", "Arizona", "04"));
                list.Add(AddState(country, "CA", "California", "06"));
                list.Add(AddState(country, "CO", "Colorado", "08"));
                list.Add(AddState(country, "CT", "Connecticut", "09"));
                list.Add(AddState(country, "DC", "District of Columbia", "11"));
                list.Add(AddState(country, "DE", "Delaware", "10"));
                list.Add(AddState(country, "FL", "Florida", "12"));
                list.Add(AddState(country, "GA", "Georgia", "13"));
                list.Add(AddState(country, "HI", "Hawaii", "15"));
                list.Add(AddState(country, "IA", "Iowa", "19"));
                list.Add(AddState(country, "ID", "Idaho", "16"));
                list.Add(AddState(country, "IL", "Illinois", "17"));
                list.Add(AddState(country, "IN", "Indiana", "18"));
                list.Add(AddState(country, "KS", "Kansas", "20"));
                list.Add(AddState(country, "KY", "Kentucky", "21"));
                list.Add(AddState(country, "LA", "Louisiana", "22"));
                list.Add(AddState(country, "MA", "Massachusetts", "25"));
                list.Add(AddState(country, "MD", "Maryland", "24"));
                list.Add(AddState(country, "ME", "Maine", "23"));
                list.Add(AddState(country, "MI", "Michigan", "26"));
                list.Add(AddState(country, "MN", "Minnesota", "27"));
                list.Add(AddState(country, "MO", "Missouri", "29"));
                list.Add(AddState(country, "MS", "Mississippi", "28"));
                list.Add(AddState(country, "MT", "Montana", "30"));
                list.Add(AddState(country, "NC", "North Carolina", "37"));
                list.Add(AddState(country, "ND", "North Dakota", "38"));
                list.Add(AddState(country, "NE", "Nebraska", "31"));
                list.Add(AddState(country, "NH", "New Hampshire", "33"));
                list.Add(AddState(country, "NJ", "New Jersey", "34"));
                list.Add(AddState(country, "NM", "New Mexico", "35"));
                list.Add(AddState(country, "NV", "Nevada", "32"));
                list.Add(AddState(country, "NY", "New York", "36"));
                list.Add(AddState(country, "OH", "Ohio", "39"));
                list.Add(AddState(country, "OK", "Oklahoma", "40"));
                list.Add(AddState(country, "OR", "Oregon", "41"));
                list.Add(AddState(country, "PA", "Pennsylvania", "42"));
                list.Add(AddState(country, "PR", "Puerto Rico", "43"));
                list.Add(AddState(country, "RI", "Rhode Island", "44"));
                list.Add(AddState(country, "SC", "South Carolina", "45"));
                list.Add(AddState(country, "SD", "South Dakota", "46"));
                list.Add(AddState(country, "TN", "Tennessee", "47"));
                list.Add(AddState(country, "TX", "Texas", "48"));
                list.Add(AddState(country, "UT", "Utah", "49"));
                list.Add(AddState(country, "VA", "Virginia", "51"));
                list.Add(AddState(country, "VT", "Vermont", "50"));
                list.Add(AddState(country, "WA", "Washington", "53"));
                list.Add(AddState(country, "WI", "Wisconsin", "55"));
                list.Add(AddState(country, "WV", "West Virginia", "54"));
                list.Add(AddState(country, "WY", "Wyoming", "56"));

                country.States = list;
                statesList.AddRange(list);
            }

            country = countries.FirstOrDefault(p => p.ISOCode == "CA");
            if (country != null)
            {
                var list = new List<State>();
                list.Add(AddState(country, "AB", "Alberta", ""));
                list.Add(AddState(country, "BC", "British Columbia", ""));
                list.Add(AddState(country, "MB", "Manitoba", ""));
                list.Add(AddState(country, "NB", "New Brunswick", ""));
                list.Add(AddState(country, "NL", "Newfoundland and Labrador", ""));
                list.Add(AddState(country, "NS", "Nova Scotia", ""));
                list.Add(AddState(country, "NT", "Northwest Territories", ""));
                list.Add(AddState(country, "NU", "Nunavut", ""));
                list.Add(AddState(country, "ON", "Ontario", ""));
                list.Add(AddState(country, "PE", "Prince Edward Island", ""));
                list.Add(AddState(country, "QC", "Quebec", ""));
                list.Add(AddState(country, "SK", "Saskatchewan", ""));
                list.Add(AddState(country, "YT", "Yukon", ""));

                country.States = list;
                statesList.AddRange(list);
            }
            

            uow.CreateRepositoryForDataSource(statesList);
            return statesList;
        }    

        private static State AddState(Country country, string code, string description, string fips)
        {
            return new State()
            {
                Country = country,
                StateCode = code,
                Description = description,
                FIPSCode = fips,
                Id = Guid.NewGuid()
            };
        }

    }

    
}
