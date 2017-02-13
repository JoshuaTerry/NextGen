using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;
using Nest;

namespace DDI.Search.Models
{
    /// <summary>
    /// Elasticsearch address document (CRM).
    /// </summary>
    [ElasticsearchType(Name = "address")]
    public class AddressDocument : ISearchDocument, IAutoMappable
    {
        [Keyword(IncludeInAll = false)]
        public Guid Id { get; set; }

        [Text]
        public string StreetAddress { get; set; }

        [Text]
        public string City { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid CountryId { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid StateId { get; set; }

        [Keyword]
        public string PostalCode { get; set; }

        [Boolean(Ignore = true)]
        public bool IsPrimary { get; set; }

        public void AutoMap(MappingsDescriptor mappings)
        {
            mappings.Map<AddressDocument>(p => p.AutoMap());
        }

    }
}
