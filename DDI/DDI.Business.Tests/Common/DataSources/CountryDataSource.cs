using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Common;

namespace DDI.Business.Tests.Common.DataSources
{
    public static class CountryDataSource
    {
        public static IList<Country> GetDataSource(IUnitOfWork uow)
        {
            IList<Country> existing = uow.GetRepositoryDataSource<Country>();
            if (existing != null)
            {
                return existing;
            }
            
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
                Id = GuidHelper.NewSequentialGuid()
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
                Id = GuidHelper.NewSequentialGuid()
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
                Id = GuidHelper.NewSequentialGuid()
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
                Id = GuidHelper.NewSequentialGuid()
            });

            uow.CreateRepositoryForDataSource(list);
            return list;
        }    

    }

    
}
