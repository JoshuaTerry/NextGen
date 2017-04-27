using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums.GL;

namespace DDI.Services.Search
{
    public class JournalSearch : PageableSearch
    {
        public Guid? BusinessUnitId { get; set; }
        public Guid? FiscalYearId { get; set; }
        public JournalType? JournalType { get; set; }
        public int JournalNumber { get; set; }
        public string Comment { get; set; }
        public string LineItemComment { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
        public DateTime? TransactionDateFrom { get; set; }
        public DateTime? TransactionDateTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOnFrom { get; set; }
        public DateTime? CreatedOnTo { get; set; }
        public string JournalStatus { get; set; }
        public string QueryString { get; set; }
    }
}
