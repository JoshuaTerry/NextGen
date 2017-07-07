using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("BusinessUnitFromTo")]
    public class BusinessUnitFromTo : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_FiscalYear_BUs", IsUnique = true, Order = 1)]
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        [Index("IX_FiscalYear_BUs", IsUnique = true, Order = 2)]
        public Guid? BusinessUnitId { get; set; }
        [ForeignKey(nameof(BusinessUnitId))]
        public BusinessUnit BusinessUnit { get; set; }

        [Index("IX_FiscalYear_BUs", IsUnique = true, Order = 3)]
        public Guid? OffsettingBusinessUnitId { get; set; }
        [ForeignKey(nameof(OffsettingBusinessUnitId))]
        public BusinessUnit OffsettingBusinessUnit { get; set; }

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
