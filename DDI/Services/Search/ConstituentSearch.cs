using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        public string State { get; set; }
        public string ZipFrom { get; set; }
        public string ZipTo { get; set; }
    }
}
