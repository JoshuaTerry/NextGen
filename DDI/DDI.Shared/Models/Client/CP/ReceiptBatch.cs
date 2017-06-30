using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Enums.CP;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;

namespace DDI.Shared.Models.Client.CP
{
    [Table("ReceiptBatch")]
    public class ReceiptBatch : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? BusinessUnitId { get; set; }
        [ForeignKey(nameof(BusinessUnitId))]
        public BusinessUnit BusinessUnit { get; set; }

        public int BatchNumber { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        public Guid? BankAccountId { get; set; }
        [ForeignKey(nameof(BankAccountId))]
        public BankAccount BankAccount { get; set; }

        public Guid? BatchTypeId { get; set; }
        [ForeignKey(nameof(BatchTypeId))]
        public ReceiptBatchType BatchType { get; set; }

        public ReceiptBatchStatus Status { get; set; }

        public ReceiptBatchEntryMode EntryMode { get; set; }

        public ReceiptBatchDistributionMode DistributionMode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        public Guid? EnteredById { get; set; }
        [ForeignKey(nameof(EnteredById))]
        public User EnteredBy { get; set; }

        public Guid? InUseById { get; set; }
        [ForeignKey(nameof(InUseById))]
        public User InUseBy { get; set; }

        public ICollection<Receipt> Receipts { get; set; }

    }
}
