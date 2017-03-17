using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class LedgerAccountYear : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; } 

        public Guid? LedgerAccountId { get; set; }
        public LedgerAccount LedgerAccount { get; set;}

        public Guid? FiscalYearId { get; set; }
        public FiscalYear FiscalYear { get; set; }

        public Guid? AccountId { get; set; }
        public Account Account { get; set; }

        public bool IsMerge { get; set; }
    }
}
