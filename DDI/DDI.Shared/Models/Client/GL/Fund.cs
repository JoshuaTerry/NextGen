using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class Fund : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        public Guid? FundSegmentId { get; set; }
        [ForeignKey(nameof(FundSegmentId))]
        public Segment FundSegment { get; set; }

        public Guid? FundBalanceAccountId { get; set; }
        [ForeignKey(nameof(FundBalanceAccountId))]
        public LedgerAccount FundBalanceAccount { get; set; }

        public Guid? CloseRevenueAccountId { get; set; }
        [ForeignKey(nameof(CloseRevenueAccountId))]
        public LedgerAccount CloseRevenueAccount { get; set; }

        public Guid? CloseExpenseAccountId { get; set; }
        [ForeignKey(nameof(CloseExpenseAccountId))]
        public LedgerAccount CloseExpenseAccount { get; set; }

        public ICollection<FundTransfer> FundTransfers { get; set; }

        public Guid? BusinessUnitId { get; set; }
        [ForeignKey(nameof(BusinessUnitId))]
        public BusinessUnit BusinessUnit { get; set; }
    }
}
