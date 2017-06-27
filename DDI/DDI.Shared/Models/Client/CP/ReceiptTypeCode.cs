using DDI.Shared.Enums.CP;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DDI.Shared.Models.Client.CP
{
    [Table("ReceiptTypeCode")]
    public class ReceiptTypeCode : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public ReceiptCategory Category { get; set; }
    }
}
