using DDI.Shared.Enums.CP;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
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
        [MaxLength(256)]
        public string Name { get; set; }
        public int BatchNumber { get; set; } 
        public Guid BankAccountId { get; set; }
        [ForeignKey("BankAccountId")]
        public BankAccount BankAccount { get; set; }
        public Guid BatchGroupId { get; set; }
        [ForeignKey("BatchGroupId")]
        public ReceiptBatchType BatchGroup { get; set; }
        public ReceiptBatchStatus BatchStatus { get; set; }
        public ReceiptBatchEntryMode BatchType { get; set; }
        public ReceiptBatchDistributionMode EntryMode { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }
        public User InUseBy { get; set; }
        public Guid BusinessUnitId { get; set; }
        [ForeignKey("BusinessUnitId")]
        public BusinessUnit BusinessUnit { get; set; }

    }
}
