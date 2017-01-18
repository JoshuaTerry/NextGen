using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.Search
{
    public class ForeignKeySearch : PageableSearch
    {
        public Guid? Id { get; set; }
    }
}
