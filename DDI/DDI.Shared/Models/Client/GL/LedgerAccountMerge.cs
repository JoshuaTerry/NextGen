using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("LedgerAccountMerge")]
    public class LedgerAccountMerge : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? FromAccountId { get; set; }
        [ForeignKey(nameof(FromAccountId))]
        public LedgerAccount FromAccount { get; set; }

        public Guid? ToAccountId { get; set; }
        [ForeignKey(nameof(ToAccountId))]
        public LedgerAccount ToAccount { get; set; }

        [MaxLength(128)]
        public string FromAccountNumber { get; set; }

        [MaxLength(128)]
        public string ToAccountNumber { get; set; }

        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }         

        public Guid? MergedById { get; set; }
        [ForeignKey(nameof(MergedById))]
        public User MergedBy { get; set; }  
               
        public DateTime? MergedOn { get; set; }
    }
}
