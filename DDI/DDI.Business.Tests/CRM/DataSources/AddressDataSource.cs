using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Business.Tests.Common.DataSources;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class AddressDataSource
    {
        public static Address CanadianAddress { get; private set; }
        public static Address FrenchAddress { get; private set; }
        public static IList<Address> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<Address> existing = uow.GetRepositoryOrNull<Address>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            CountryDataSource.GetDataSource(uow);
            StateDataSource.GetDataSource(uow);

            var list = new List<Address>();
            list.Add(BuildAddress(uow, "101 W. Ohio St.", "Suite 1650", "Indianapolis", "US", "IN", "46204"));
            list.Add(BuildAddress(uow, "24 SE 1st Ave", "", "Ocala", "US", "FL", "34471")); 
            list.Add(BuildAddress(uow, "204 W Broadway St.", "", "Frankfort", "US", "KY", "40601"));
            list.Add(BuildAddress(uow, "324 S Grand St", "", "Enid", "US", "OK", "73701"));
            list.Add(BuildAddress(uow, "503 Cloverdale Rd #101", "", "Montgomery", "US", "AL", "36106"));
            list.Add(BuildAddress(uow, "416 Central Dr", "", "Cullowhee", "US", "NC", "28723"));

            list.Add(CanadianAddress = BuildAddress(uow, "9853 90 Ave NW", "", "Edmonton", "CA", "AB", "T6E 2T2"));

            list.Add(FrenchAddress = BuildAddress(uow, "20 Place Saint-Georges", "", "Toulouse", "FR", "", "31000"));

            uow.CreateRepositoryForDataSource(list);
            return list;
        }    

        private static Address BuildAddress(IUnitOfWork uow, string line1, string line2, string city, string countryCode, string stateCode, string postalCode)
        {
            Address address = new Address()
            {
                AddressLine1 = line1,
                AddressLine2 = line2,
                PostalCode = postalCode,
                City = city,
                Id = GuidHelper.NextGuid()
            };

            address.Country = uow.FirstOrDefault<Country>(p => p.ISOCode == countryCode);
            address.State = address.Country?.States?.FirstOrDefault(p => p.StateCode == stateCode);
            return address;
        }

    }

    
}
