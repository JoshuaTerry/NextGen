using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;

namespace DDI.Shared.Models.Client.CP
{
    [Table("Receipt")]
    public class Receipt : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public int ReceiptNumber { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal Amount { get; set; }

        [MaxLength(128)]
        public string Reference { get; set; }

        public bool IsProcessed { get; set; } 
        
        public bool IsReversed { get; set; }

        public Guid? ReceiptTypeId { get; set; }
        [ForeignKey(nameof(ReceiptTypeId))]
        public ReceiptType ReceiptType { get; set; }

        [MaxLength(64)]
        public string AccountNumber { get; set; }

        [MaxLength(64)]
        public string RoutingNumber { get; set; }

        [MaxLength(30)]
        public string CheckNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; } 

        public Guid ReceiptBatchId { get; set; }
        [ForeignKey(nameof(ReceiptBatchId))]
        public ReceiptBatch ReceiptBatch { get; set; }

        public override string DisplayName => ReceiptNumber.ToString();
    }
}
