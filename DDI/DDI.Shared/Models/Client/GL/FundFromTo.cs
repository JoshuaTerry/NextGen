using DDI.Shared.Enums.GL;
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

        [Column("FromAccountId")]
        public Guid? FromLedgerAccountId { get; set; }
        [ForeignKey(nameof(FromLedgerAccountId))]
        public LedgerAccount FromLedgerAccount { get; set; }

        [Column("ToAccountId")]
        public Guid? ToLedgerAccountId { get; set; }
        [ForeignKey(nameof(ToLedgerAccountId))]
        public LedgerAccount ToLedgerAccount { get; set; }

        [NotMapped]
        public Guid? FromAccountId { get; set; }

        [NotMapped]
        public Guid? ToAccountId { get; set; }

        [NotMapped]
        public string Name { get; set; }

        public override string DisplayName => Name;
    }
}
