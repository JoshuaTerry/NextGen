using DDI.Shared.Enums.GL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class FundTransfer : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? FiscalYearId { get; set; }
        public FiscalYear FiscalYear { get; set; }

        public Guid? FundId { get; set; }
        public Fund Fund { get; set; }

        public Guid? OffsettingFundId { get; set; }
        public Fund OffsettingFund { get; set; }

        public Guid? FromAccountId { get; set; }
        public LedgerAccount FromAccount { get; set; }

        public Guid? ToAccountId { get; set; }
        public LedgerAccount ToAccount { get; set; }

    }
}
