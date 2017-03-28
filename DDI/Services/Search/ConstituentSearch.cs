using System;

namespace DDI.Services.Search
{
    public class ConstituentSearch : PageableSearch
    {
        public string Address { get; set; }
        public string AlternateId { get; set; }
        public string City { get; set; }
        public int? ConstituentNumber { get; set; }
        public Guid? ConstituentTypeId { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public string Fields { get; set; }
        public string Name { get; set; }
        public string QuickSearch { get; set; }
        public string StateId { get; set; }
        public string CountryId { get; set; }
        public string PostalCodeFrom { get; set; }
        public string PostalCodeTo { get; set; }
        public string RegionId1 { get; set; }
        public string RegionId2 { get; set; }
        public string RegionId3 { get; set; }
        public string RegionId4 { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public string IncludeTags { get; set; }
        public string ExcludeTags { get; set; }
    }

}
