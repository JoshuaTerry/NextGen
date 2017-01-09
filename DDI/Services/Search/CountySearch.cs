using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.Search
{
    public class CountySearch : PageableSearch
    {
        public Guid? StateId { get; set; }
    }
}
