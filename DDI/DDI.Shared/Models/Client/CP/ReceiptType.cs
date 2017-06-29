using DDI.Shared.Enums.CP;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DDI.Shared.Models.Client.CP
{
    [Table("ReceiptTypeCode")]
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

        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public ReceiptCategory Category { get; set; }
    }
}
