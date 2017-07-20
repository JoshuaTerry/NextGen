using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Enums.CP;


namespace DDI.Shared.Models.Client.CP
{
    [Table("ReceiptType")]
    public class ReceiptType : AuditableEntityBase, ICodeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public bool IsActive { get; set; }

        [Index("IX_Code", IsUnique = true), MaxLength(128)]
        public string Code { get; set; }

        [Index("IX_Name", IsUnique = true), MaxLength(128)]
        public string Name { get; set; }

        public ReceiptCategory Category { get; set; }

        public override string DisplayName => Name;
    }
}
