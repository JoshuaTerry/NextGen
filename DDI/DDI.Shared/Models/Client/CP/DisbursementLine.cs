using DDI.Shared.Enums.CP;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;

namespace DDI.Shared.Models.Client.CP
{
    [Table("DisbursementLine")]
    public class DisbursementLine : LinkedEntityBase
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

        [DecimalPrecision(14, 2)]
        public decimal DebitAmount { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal CreditAmount { get; set; }

        [MaxLength(64)]
        public string InvoiceNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }


    }
}
