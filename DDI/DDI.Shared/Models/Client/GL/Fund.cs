﻿using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("GL_Fund")]
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

        public Guid? ClosingRevenueAccountId { get; set; }
        [ForeignKey(nameof(ClosingRevenueAccountId))]
        public LedgerAccount ClosingRevenueAccount { get; set; }

        public Guid? ClosingExpenseAccountId { get; set; }
        [ForeignKey(nameof(ClosingExpenseAccountId))]
        public LedgerAccount ClosingExpenseAccount { get; set; }

        public ICollection<FundFromTo> FundTransfers { get; set; }
    }
}
