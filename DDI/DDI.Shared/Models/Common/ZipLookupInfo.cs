using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Common
{
    public class ZipLookupInfo : EntityBase
    {
        public override Guid Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public Country Country { get; set; }
        public County County { get; set; }
        public string PostalCode { get; set; }
        public State State { get; set; }
    }
}
