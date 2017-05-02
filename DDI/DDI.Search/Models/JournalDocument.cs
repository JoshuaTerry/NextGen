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
    /// Elasticsearch journal document (GL).
    /// </summary>
    [ElasticsearchType(Name = "journal")]
    public class JournalDocument : ISearchDocument, IAutoMappable
    {
        [Keyword(IncludeInAll = false)]
        public Guid Id { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? BusinessUnitId { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? FiscalYearId { get; set; }

        [Number]
        public int JournalType { get; set; }

        [Number]
        public int JournalNumber { get; set; }
        
        [Text]
        public string Comment { get; set; }

        [Number]
        public decimal Amount { get; set; }

        [Keyword]
        public string Status { get; set; }

        [Date]
        public DateTime? TransactionDate { get; set; }

        [Keyword]
        public string CreatedBy { get; set; }

        [Date]
        public DateTime? CreatedOn { get; set; }

        [Text]
        public string LineItemComments { get; set; }
        
        public void AutoMap(MappingsDescriptor mappings)
        {
            mappings.Map<JournalDocument>(p => p.AutoMap());
        }

    }
}
