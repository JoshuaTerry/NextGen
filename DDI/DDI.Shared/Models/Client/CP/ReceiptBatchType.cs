using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CP
{
    [Table("ReceiptBatchType")]
    public class ReceiptBatchType : AuditableEntityBase, ICodeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public bool IsActive { get; set; }

        [Index("IX_Code", IsUnique = true), MaxLength(128)]
        public string Code { get; set; }

        [Index("IX_Name", IsUnique = true), MaxLength(128)]
        public string Name { get; set; }

        public Guid? BankAccountId { get; set; }
        [ForeignKey(nameof(BankAccountId))]
        public BankAccount BankAccount { get; set; }

        public override string DisplayName => $"{Code}: {Name}";

    }
}
