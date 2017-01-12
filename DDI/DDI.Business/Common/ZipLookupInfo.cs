using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using DDI.Shared.Models.Common;

namespace DDI.Business.Common
{
    /// <summary>
    /// Cass 
    /// </summary>
    public class ZipLookupInfo
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public Country Country { get; set; }
        public County County { get; set; }
        public string PostalCode { get; set; }
        public State State { get; set; }
    }

}
