using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("Fund")]
    public class Fund : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_FiscalYear_FundSegment", IsUnique = true, Order = 1)]
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        [Index("IX_FiscalYear_FundSegment", IsUnique = true, Order = 2)]
        public Guid? FundSegmentId { get; set; }
        [ForeignKey(nameof(FundSegmentId))]
        public Segment FundSegment { get; set; }

        [Column("FundBalanceAccountId")]
        public Guid? FundBalanceLedgerAccountId { get; set; }
        [ForeignKey(nameof(FundBalanceLedgerAccountId))]
        public LedgerAccount FundBalanceLedgerAccount { get; set; }

        [Column("ClosingRevenueAccountId")]
        public Guid? ClosingRevenueLedgerAccountId { get; set; }
        [ForeignKey(nameof(ClosingRevenueLedgerAccountId))]
        public LedgerAccount ClosingRevenueLedgerAccount { get; set; }

        [Column("ClosingExpenseAccountId")]
        public Guid? ClosingExpenseLedgerAccountId { get; set; }
        [ForeignKey(nameof(ClosingExpenseLedgerAccountId))]
        public LedgerAccount ClosingExpenseLedgerAccount { get; set; }    

        [NotMapped]
        public Guid? FundBalanceAccountId { get; set; }

        [NotMapped]
        public Guid? ClosingRevenueAccountId { get; set; }

        [NotMapped]
        public Guid? ClosingExpenseAccountId { get; set; }

        [InverseProperty(nameof(FundFromTo.Fund))]
        public ICollection<FundFromTo> FundFromTos { get; set; }

        public override string DisplayName =>  FundSegment?.Code + " " + FundSegment?.Name;
    }
}
