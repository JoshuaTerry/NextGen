using DDI.Shared.Enums.GL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class JournalLine : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid? LedgerAccountId { get; set; }
        [ForeignKey(nameof(LedgerAccountId))]
        public LedgerAccount LedgerAccount { get; set; }         
        public Decimal Percent { get; set; }         
        public DueToMode DueToMode { get; set; }
        public Guid? SourceBusinessUnitId { get; set; }
        [ForeignKey(nameof(SourceBusinessUnitId))]
        public BusinessUnit SourceBusinessUnit { get; set; }
        public Guid? FundId { get; set; }
        [ForeignKey(nameof(FundId))]
        public Fund SourceFund { get; set; }
    }
}

