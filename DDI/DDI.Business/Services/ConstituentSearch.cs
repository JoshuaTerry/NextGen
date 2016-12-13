using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDI.Business.Services
{
    public class ConstituentSearch : IPageable
    {
        public string QuickSearch { get; set; }
        public int? ConstituentNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string OrderBy { get; set; }
    }
}