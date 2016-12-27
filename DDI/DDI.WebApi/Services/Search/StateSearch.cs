using System;

namespace DDI.WebApi.Services.Search
{
    public class StateSearch : PageableSearch
    {
        public Guid? CountryId { get; set; }
    }
}