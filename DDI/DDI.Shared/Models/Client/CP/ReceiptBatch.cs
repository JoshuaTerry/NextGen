using DDI.Shared.Enums.CP;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CP
{
    [Table("ReceiptBatch")]
    public class ReceiptBatch : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public ICollection<Attachment> Attachements { get; set; }
        public BankAccount BankAccount { get; set; }
        public ReceiptBatchGroup BatchGroup { get; set; }
        public ReceiptBatchStatus BatchStatus { get; set; }
        public ReceiptBatchType BatchType { get; set; }
        public ReceiptBatchDistributionMode DistributionMode { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set;}
        public User InUseBy { get; set; }

    }
}
