using DDI.Shared.Enums.CP;
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
        public decimal DistributedAmount { get; set; }
        public User DistributedBy { get; set; }
        public bool IsDistributed { get; set; }
        public decimal GroupAmount { get; set; }
        public Receipt GroupParent { get; set; }
        public ReceiptTypeCode ReceiptType { get; set; }
        public EFTInfo EFTInfo { get; set; }
        [MaxLength(5)]
        public string CheckNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public ReceiptBatch ReceiptBatch { get; set; }
        public ReceiptBatchType BatchType { get; set; }
        [MaxLength(256)]
        public string BatchGroupDescription { get; set; }
        public ReceiptCategory ReceiptCategory { get; set; }
    }
}
