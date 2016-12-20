using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDI.Business.Services.Search
{
    public class StateSearch : PageableSearch
    {
        public Guid? CountryId { get; set; }
    }
}