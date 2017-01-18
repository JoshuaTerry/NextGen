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
    public static class CountryDataSource
    {
        public static IList<Country> GetDataSource(UnitOfWorkNoDb uow)
        {
            var list = new List<Country>();
            list.Add(new Country()
            {
                CountryCode = "US",
                Description = "United States",
                ISOCode = "US",
                LegacyCode = "USA",
                StateName = "State",
                StateAbbreviation = "St",
                PostalCodeFormat = "99999,99999-9999",
                AddressFormat = "",
                CallingCode = "1",
                InternationalPrefix = "011",
                TrunkPrefix = "1",
                PhoneFormat = "(XXX) XXX-XXXX",
                Id = Guid.NewGuid()
            });

            list.Add(new Country()
            {
                CountryCode = "CA",
                Description = "Canada",
                ISOCode = "CA",
                LegacyCode = "CANA",
                StateName = "Province",
                StateAbbreviation = "Pr",
                PostalCodeFormat = "A9A 9A9",
                AddressFormat = "$CITY $ST $ZIP",
                CallingCode = "1",
                InternationalPrefix = "011",
                TrunkPrefix = "1",
                PhoneFormat = "(XXX) XXX-XXXX",
                Id = Guid.NewGuid()
            });

            list.Add(new Country()
            {
                CountryCode = "FR",
                Description = "France",
                ISOCode = "FR",
                LegacyCode = "FRAN",
                StateName = "",
                StateAbbreviation = "",
                PostalCodeFormat = "99999",
                AddressFormat = "$CC-$ZIP $CITY",
                CallingCode = "33",
                InternationalPrefix = "00",
                TrunkPrefix = "0",
                PhoneFormat = "X XX XX XX XX",
                Id = Guid.NewGuid()
            });

            list.Add(new Country()
            {
                CountryCode = "BJ",
                Description = "Benin",
                ISOCode = "BJ",
                LegacyCode = "BENI",
                StateName = "",
                StateAbbreviation = "",
                PostalCodeFormat = "99 BP 9999",
                AddressFormat = "$ZIP|$CITY",
                CallingCode = "229",
                InternationalPrefix = "00",
                TrunkPrefix = "",
                PhoneFormat = "XXXX XXXX",
                Id = Guid.NewGuid()
            });

            uow.CreateRepositoryForDataSource(list);
            return list;
        }    

    }

    
}
