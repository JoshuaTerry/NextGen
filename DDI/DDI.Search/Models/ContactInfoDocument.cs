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
    /// Elasticsearch ContactInfo document (CRM)
    /// </summary>
    [ElasticsearchType(Name = "contactInfo")]
    public class ContactInfoDocument : ISearchDocument
    {
        [Keyword(IncludeInAll = false)]
        public Guid Id { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid ContactCategoryId { get; set; }

        [Text]
        public string Info { get; set; }

        [Boolean(Ignore = true)]   
        public bool IsPrefrerred { get; set; }

        [Text]
        public string Comment { get; set; }

    }
}
