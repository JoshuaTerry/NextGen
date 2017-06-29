using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CP
{
    [Table("Receipt")]
    public class Receipt : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public string Reference { get; set; }
        public bool IsProcessed { get; set; }        
        public ReceiptType ReceiptType { get; set; }
        [MaxLength(9)]
        public string BankRoutingNumber { get; set; }
        [MaxLength(128)]
        public string BankAccountNumber { get; set; }
        [MaxLength(30)]
        public string CheckNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; } 
        public Guid ReceiptBatchId { get; set; }
        [ForeignKey("ReceiptBatchId")]
        public ReceiptBatch ReceiptBatch { get; set; }
    }
}
