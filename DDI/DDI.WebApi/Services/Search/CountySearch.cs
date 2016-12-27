using System;

namespace DDI.WebApi.Services.Search
{
    public class CountySearch : PageableSearch
    {
        public Guid? StateId { get; set; }
    }
}