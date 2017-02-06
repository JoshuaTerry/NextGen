using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Search
{
    public class Address
    {
        public Guid Id { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public Guid CountryId { get; set; }
        public Guid StateId { get; set; }
        public string PostalCode { get; set; }
        public bool IsPrimary { get; set; }
    }
}
