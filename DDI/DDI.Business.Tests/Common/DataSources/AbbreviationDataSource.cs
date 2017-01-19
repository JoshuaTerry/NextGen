using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Common;

namespace DDI.Business.Tests.Common.DataSources
{
    public static class AbbreviationDataSource
    {
        public static IList<Abbreviation> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<Abbreviation> existing = uow.GetRepositoryOrNull<Abbreviation>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }
            
            var list = new List<Abbreviation>();
            list.Add(new Abbreviation { Word = "NORTH", AddressWord = "NORTH", NameWord = "NORTH", USPSAbbreviation = "N", Priority = 2 , Id = GuidHelper.NextGuid()});
            list.Add(new Abbreviation { Word = "N", AddressWord = "NORTH", NameWord = "N.", USPSAbbreviation = "N", Priority = 2, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "SOUTH", AddressWord = "SOUTH", NameWord = "SOUTH", USPSAbbreviation = "S", Priority = 2, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "S", AddressWord = "SOUTH", NameWord = "S.", USPSAbbreviation = "S", Priority = 2, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "EAST", AddressWord = "EAST", NameWord = "EAST", USPSAbbreviation = "E", Priority = 2, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "E", AddressWord = "EAST", NameWord = "E.", USPSAbbreviation = "E", Priority = 2, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "WEST", AddressWord = "WEST", NameWord = "WEST", USPSAbbreviation = "W", Priority = 2, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "W", AddressWord = "WEST", NameWord = "W.", USPSAbbreviation = "W", Priority = 2, Id = GuidHelper.NextGuid() });

            list.Add(new Abbreviation { Word = "RIVER", AddressWord = "RIVER", NameWord = "RIVER", USPSAbbreviation = "RIV", Priority = 1, IsSuffix = true, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "APT", AddressWord = "APT.", NameWord = "APT", USPSAbbreviation = "APT", Priority = 1, IsSecondary = true, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "SUITE", AddressWord = "SUITE", NameWord = "", USPSAbbreviation = "STE", Priority = 0, IsSecondary = true, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "STE", AddressWord = "SUITE", NameWord = "", USPSAbbreviation = "STE", Priority = 0, IsSecondary = true });
            list.Add(new Abbreviation { Word = "FLOOR", AddressWord = "FLOOR", NameWord = "FLOOR", USPSAbbreviation = "FL", Priority = 0, IsSecondary = true, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "FL", AddressWord = "FLOOR", NameWord = "FLOOR", USPSAbbreviation = "FL", Priority = 0, IsSecondary = true, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "HC", AddressWord = "HC", NameWord = "", USPSAbbreviation = "HC", Priority = 1, IsCaps = true, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "PO", AddressWord = "P. O.", NameWord = "", USPSAbbreviation = "PO", Priority = 1, IsCaps = true, Id = GuidHelper.NextGuid() });


            list.Add(new Abbreviation { Word = "ROAD", AddressWord = "ROAD", NameWord = "ROAD", USPSAbbreviation = "RD", Priority = 0, IsSuffix = true, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "RD", AddressWord = "ROAD", NameWord = "ROAD", USPSAbbreviation = "RD", Priority = 0, IsSuffix = true, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "STREET", AddressWord = "STREET", NameWord = "STREET", USPSAbbreviation = "ST", Priority = 0, IsSuffix = true, Id = GuidHelper.NextGuid() });
            list.Add(new Abbreviation { Word = "ST", AddressWord = "STREET", NameWord = "STREET", USPSAbbreviation = "ST", Priority = 0, IsSuffix = true, Id = GuidHelper.NextGuid() });

            uow.CreateRepositoryForDataSource(list);
            return list;
        }    

    }

    
}
