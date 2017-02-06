using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Shared.Models.Search
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

        public IList<ContactInfoDocument> ContactInfo { get; set; }
        public IList<string> AlternateIds { get; set; }
        public IList<AddressDocument> Addresses { get; set; }
        public IList<String> DoingBusinessAs { get; set; }
       
    }
}
