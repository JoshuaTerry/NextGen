using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.Search
{
    public class StateSearch : PageableSearch
    {
        public Guid? CountryId { get; set; }
    }
}
