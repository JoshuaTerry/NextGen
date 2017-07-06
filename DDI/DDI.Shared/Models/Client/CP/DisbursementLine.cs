using DDI.Shared.Enums.CP;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CP
{
    [Table("DisbursementLine")]
    public class DisbursementLine : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid DisbursementId { get; set; }
        [ForeignKey(nameof(DisbursementId))]
        public Disbursement Disbursement { get; set; }

        public int LineNumber { get; set; }

        public DisbursementLineType LineType { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        public decimal LineDebitAmount { get; set; }

        public decimal LineCreditAmount { get; set; }

        [MaxLength(64)]
        public string InvoiceNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }

        [MaxLength(64)]
        public string InvoiceLabel { get; set; }

        // There still needs to be some way to link these lines to where they came from, such as an AP voucher, investment, or loan.

    }
}
