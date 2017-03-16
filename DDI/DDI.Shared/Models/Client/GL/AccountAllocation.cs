using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class AccountAllocation : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }
        public Guid? SourceAccountId { get; set; }
        [ForeignKey(nameof(SourceAccountId))]
        public Account SourceAccount { get; set; }
        public Guid? DestinationAccountId { get; set; }
        [ForeignKey(nameof(DestinationAccountId))]
        public Account DestinationAccount { get; set; }
        public Guid? BusinessUnitId { get; set; }
        [ForeignKey(nameof(BusinessUnitId))]
        public BusinessUnit BusinessUnit { get; set; }
        public int LineNumber { get; set; }
        public decimal Percentage { get; set; }
    }

}