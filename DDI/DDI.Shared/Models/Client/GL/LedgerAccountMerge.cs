using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class LedgerAccountMerge : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid? FromAccountId { get; set; }
        public Guid? ToAccountId { get; set; }

        public string FromAccountNumber { get; set; }    
        public string ToAccountNumber { get; set; }

        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }         
        public User MergedBy { get; set; }         
        public DateTime? MergedDt { get; set; }
        [ForeignKey(nameof(ToAccountId))]
        public LedgerAccount ToAccount { get; set; }
        [ForeignKey(nameof(FromAccountId))]
        public LedgerAccount FromAccount { get; set; }
    }
}
