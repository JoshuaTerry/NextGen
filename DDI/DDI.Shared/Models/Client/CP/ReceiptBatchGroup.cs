using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CP
{
    [Table("ReceiptBatchGroup")]
    public class ReceiptBatchGroup : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
        public BankAccount BankAccount { get; set; }
        public bool IsActive { get; set; }

    }
}
