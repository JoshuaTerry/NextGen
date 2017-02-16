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
    [ElasticsearchType(Name = "constituent")]
    public class ConstituentDocument : ISearchDocument, IAutoMappable
    {
        [Keyword(IncludeInAll = false)]
        public Guid Id { get; set; }

        [Number]
        public int ConstituentNumber { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? ConstituentStatusId { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? ConstituentTypeId { get; set; }

        [Text]
        public string Name { get; set; }

        [Text]
        public string PrimaryAddress { get; set; }

        [Keyword]
        public string Source { get; set; }

        [Nested] 
        public IList<ContactInfoDocument> ContactInfo { get; set; }

        [Keyword]
        public IList<string> AlternateIds { get; set; }

        [Nested] 
        public IList<AddressDocument> Addresses { get; set; }

        [Text]
        public IList<String> DoingBusinessAs { get; set; }

        public void AutoMap(MappingsDescriptor mappings)
        {
            mappings.Map<ConstituentDocument>(p => p.AutoMap());
        }

        // TODO:  More stuff needs to be added for DC-367 (POC Cleanup)

    }
}
