using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.Core;

namespace DDI.Shared.Models.Client.GL
{
    [Table("JournalLine")]
    public class JournalLine : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public int LineNumber { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }
        
        public Guid? LedgerAccountId { get; set; }
        [ForeignKey(nameof(LedgerAccountId))]
        public LedgerAccount LedgerAccount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeletedOn { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal Amount { get; set; }

        [DecimalPrecision(5, 2)]
        public decimal Percent { get; set; }

        public DueToMode DueToMode { get; set; }

        public Guid? SourceBusinessUnitId { get; set; }
        [ForeignKey(nameof(SourceBusinessUnitId))]
        public BusinessUnit SourceBusinessUnit { get; set; }

        public Guid? SourceFundId { get; set; }
        [ForeignKey(nameof(SourceFundId))]
        public Fund SourceFund { get; set; }

        public Guid JournalId { get; set; }
        [ForeignKey(nameof(JournalId))]
        public Journal Journal { get; set; }

    }
}
