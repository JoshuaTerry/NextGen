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
        public decimal LineDebitAmount { get; set; }
        public decimal LineCreditAmount { get; set; }
        public string InvoiceNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }
        public DisbursementLineType LineType { get; set; }
    }
}
