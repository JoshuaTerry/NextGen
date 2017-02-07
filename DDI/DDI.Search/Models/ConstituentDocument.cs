using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Shared.Models;
using Nest;

namespace DDI.Search.Models
{    
    /// <summary>
    /// Elasticsearch constituent document (CRM).
    /// </summary>
    public class ConstituentDocument : ISearchDocument
    {
        public Guid Id { get; set; }

        public Guid? ConstituentStatusId { get; set; }
        public Guid? ConstituentTypeId { get; set; }

        public string Name { get; set; }

        public string Source { get; set; }

        [Nested]
        public IList<ContactInfoDocument> ContactInfo { get; set; }

        public IList<string> AlternateIds { get; set; }

        [Nested]
        public IList<AddressDocument> Addresses { get; set; }

        public IList<String> DoingBusinessAs { get; set; }
       
    }
}
