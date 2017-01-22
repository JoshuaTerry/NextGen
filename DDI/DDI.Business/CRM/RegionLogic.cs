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
        public RegionLogic() : this(new UnitOfWorkEF()) { }

        public RegionLogic(IUnitOfWork uow) : base(uow)
        {
        }

        public List<Region> GetRegionsByAddress(Guid? countryid, Guid? stateId, Guid? countyId, string city, string zipcode)
        {
            int minLevel = 1;
            int maxLevel = UnitOfWork.GetEntities<RegionLevel>().Max(rl => rl.Level);
            Country us = this.UnitOfWork.FirstOrDefault<Country>(c => c.CountryCode == "US"); // TODO: Change to AddressLogic.DefaultCountryCode once address logic is merged in.

            var regions = new List<Region>();

            for (int level = minLevel; level <= maxLevel; level++)
            {
                string truncatedZip = (countryid == us?.Id && zipcode.Length > 5) ? zipcode.Substring(0, 5) : zipcode;
                int priority = int.MinValue;

                Region region = null;

                // TODO:  Queries should be modified to use CachedRepository, but only after recent caching changes to develop have beem merged in.                

                // State
                if (stateId != null)
                {
                    foreach (var regionArea in UnitOfWork.GetEntities<RegionArea>(p => p.Region).Where(ra => ra.Level == level && ra.StateId == stateId))
                    {
                        if (regionArea.Priority > priority && CheckRegionArea(regionArea, countyId, city, truncatedZip))
                        {
                            region = regionArea.Region;
                            priority = regionArea.Priority;
                        }
                    }
                }

                // Country, state ignored
                if (region == null && countryid != null)
                {
                    foreach (var regionArea in UnitOfWork.GetEntities<RegionArea>(p => p.Region).Where(ra => ra.Level == level && ra.StateId == null && ra.CountryId == countryid))
                    {
                        if (regionArea.Priority > priority && CheckRegionArea(regionArea, countyId, city, truncatedZip))
                        {
                            region = regionArea.Region;
                            priority = regionArea.Priority;
                        }
                    }
                }
                
                // Country ignored, state ignored
                if (region == null)
                {
                    foreach (var regionArea in UnitOfWork.GetEntities<RegionArea>(p => p.Region).Where(ra => ra.Level == level && ra.StateId == null && ra.CountryId == null))
                    {
                        if (regionArea.Priority > priority && CheckRegionArea(regionArea, countyId, city, truncatedZip))
                        {
                            region = regionArea.Region;
                            priority = regionArea.Priority;
                        }
                    }
                }

                if (region != null)
                {
                    regions.Add(region);
                }
            }

            return regions;
        }

        /// <summary>
        /// Checks to see if a RegionArea matches a county id, city, and postal code.  It's assumed the RegionArea is already been selected
        /// for a particular country and state.
        /// </summary>
        private bool CheckRegionArea(RegionArea area, Guid? countyId, string city, string postalCode)
        {
            // Test for county and city
            if (area.CountyId != null && area.CountyId != countyId)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(area.City) && string.Compare(area.City, city, true) != 0)
            {
                return false;
            }

            // Postal code logic
            if (!string.IsNullOrWhiteSpace(area.PostalCodeLow))
            {
                if (string.IsNullOrWhiteSpace(postalCode))
                {
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(area.PostalCodeHigh))
                {
                    // Zip code range
                    if (string.Compare(area.PostalCodeLow, postalCode, true) > 0 ||
                        string.Compare(area.PostalCodeHigh, postalCode, true) < 0)
                    {
                        return false;
                    }
                }
                else if (!area.PostalCodeLow.Contains(','))
                {
                    // Single zip
                    if (string.Compare(area.PostalCodeLow, postalCode, true) != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    // Comma delimited list of zips
                    foreach (var entry in area.PostalCodeLow.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (string.Compare(entry, postalCode, true) == 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }

            }

            return true;
        }


    }
}
