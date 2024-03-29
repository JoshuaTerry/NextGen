﻿using DDI.Shared.Enums.GL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("FundFromTo")]
    public class FundFromTo : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_FiscalYear_Funds", IsUnique = true, Order = 1)]
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        [Index("IX_FiscalYear_Funds", IsUnique = true, Order = 2)]
        public Guid? FundId { get; set; }
        [ForeignKey(nameof(FundId))]
        public Fund Fund { get; set; }

        [Index("IX_FiscalYear_Funds", IsUnique = true, Order = 3)]
        public Guid? OffsettingFundId { get; set; }
        [ForeignKey(nameof(OffsettingFundId))]
        public Fund OffsettingFund { get; set; }

        public Guid? FromAccountId { get; set; }
        [ForeignKey(nameof(FromAccountId))]
        public LedgerAccount FromAccount { get; set; }

        public Guid? ToAccountId { get; set; }
        [ForeignKey(nameof(ToAccountId))]
        public LedgerAccount ToAccount { get; set; }

    }
}
