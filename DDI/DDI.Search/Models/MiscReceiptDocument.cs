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
    /// Elasticsearch Misc. Receipt document (CP).
    /// </summary>
    [ElasticsearchType(Name = "miscreceipt")]
    public class MiscReceiptDocument : ISearchDocument, IAutoMappable
    {
        [Keyword(IncludeInAll = false)]
        public Guid Id { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? BusinessUnitId { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? FiscalYearId { get; set; }

        [Number]
        public int MiscReceiptType { get; set; }

        [Number]
        public int MiscReceiptNumber { get; set; }
        
        [Text]
        public string Comment { get; set; }

        [Number]
        public decimal Amount { get; set; }

        [Text]
        public string Status { get; set; }

        [Date]
        public DateTime? TransactionDate { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? ConstituentId { get; set; }

        [Keyword]
        public string CreatedBy { get; set; }

        [Date]
        public DateTime? CreatedOn { get; set; }

        [Text]
        public string LineItemComments { get; set; }
        
        public void AutoMap(MappingsDescriptor mappings)
        {
            mappings.Map<MiscReceiptDocument>(p => p.AutoMap());
        }

    }
}
