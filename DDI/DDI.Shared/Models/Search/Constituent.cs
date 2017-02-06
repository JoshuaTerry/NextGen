using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Attributes;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Statics;

namespace DDI.Shared.Models.Search
{    
    public class Constituent
    {
        public Guid Id { get; set; }

        public Guid? ConstituentStatusId { get; set; }
        public Guid? ConstituentTypeId { get; set; }

        public string Name { get; set; }

        public string Source { get; set; }

        public IList<ContactInfo> ContactInfo { get; set; }
        public IList<string> AlternateIds { get; set; }
        public IList<Address> Addresses { get; set; }
       
    }
}
