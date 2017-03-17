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
        public FiscalYear FiscalYear { get; set; }

        public Guid? FundSegmentId { get; set; }
        public Segment FundSegment { get; set; }

        public Guid? FundBalanceAccountId { get; set; }
        public LedgerAccount FundBalanceAccount { get; set; }

        public Guid? ClosingRevenueAccountId { get; set; }
        public LedgerAccount ClosingRevenueAccount { get; set; }

        public Guid? ClosingExpenseAccountId { get; set; }
        public LedgerAccount ClosingExpenseAccount { get; set; }

        public ICollection<FundTransfer> FundTransfers { get; set; }
    }
}
