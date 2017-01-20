using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Business.CRM
{
    public class RegionLogic : EntityLogicBase<Region>
    {
        private IRepository<Region> _repo = null;
        public RegionLogic() : this(new UnitOfWorkEF()) { }

        public RegionLogic(IUnitOfWork uow) : base(uow)
        {
            _repo = UnitOfWork.GetRepository<Region>();
        }

        public List<Region> GetRegionsByAddress(Guid? countryid, Guid? stateId, Guid? countyId, string city, string zipcode)
        {
            var levels = UnitOfWork.GetRepository<RegionLevel>().Entities.Max(rl => rl.Level);
            var us = this.UnitOfWork.GetRepository<Country>().Entities.FirstOrDefault(c => c.CountryCode == "US");

            var regions = new List<Region>();
            for (int i = 1; i <= levels; i++)
            {
                var zip = (countryid == us?.Id && zipcode.Length > 5) ? zipcode.Substring(0, 5) : zipcode;

                Region region = null;

                if (stateId != null)
                {
                     region = UnitOfWork.GetRepository<RegionArea>().Entities.Where(ra => ra.Level == i && 
                                                                                        ra.CountyId == countyId ||
                                                                                        ra.City == city ||
                                                                                        CompareRegionAreaAgainstZipCode(ra, zipcode)).OrderBy(ra => ra.Priority).FirstOrDefault().Region;                    
                }

                if (region == null && countryid != null)
                {
                    region = UnitOfWork.GetRepository<RegionArea>().Entities.Where(ra => ra.Level == i &&
                                                                                ra.StateId == Guid.Empty &&
                                                                                ra.CountryId == countryid &&
                                                                                ra.CountyId == countyId ||
                                                                                ra.City == city ||
                                                                                CompareRegionAreaAgainstZipCode(ra, zipcode)).OrderBy(ra => ra.Priority).FirstOrDefault().Region;
                }
                
                if (region == null)
                {
                    region = UnitOfWork.GetRepository<RegionArea>().Entities.Where(ra => ra.Level == i && 
                                                                                ra.CountryId == Guid.Empty &&
                                                                                ra.CountyId == countyId ||
                                                                                ra.City == city ||
                                                                                CompareRegionAreaAgainstZipCode(ra, zipcode)).OrderBy(ra => ra.Priority).FirstOrDefault().Region;
                }
                if (region != null)
                    regions.Add(region);
            }

            return regions;
        }

        public bool CompareRegionAreaAgainstZipCode(RegionArea area, string zipcode)
        {
            if (string.IsNullOrEmpty(zipcode))
                return false;

            if (!string.IsNullOrEmpty(area.PostalCodeHigh) && !string.IsNullOrEmpty(area.PostalCodeLow))
            {
                if(string.Compare(area.PostalCodeLow, zipcode, true) > 0 || string.Compare(area.PostalCodeHigh, zipcode, true) < 0)
                    return false;                
            }
            else if (!string.IsNullOrEmpty(area.PostalCodeLow))
            {
                var zipLow = area.PostalCodeLow.Split(',');
                if (string.Compare(area.PostalCodeLow, zipcode, true) != 0)
                    return false;
            }

            return true;
        }
    }
}
