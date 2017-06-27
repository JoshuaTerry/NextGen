using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CP
{
    [Table("CheckForm")]
    public class CheckForm : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        [MaxLength(128)]
        public string Description { get; set; }
        [Index("IX_Code", IsUnique = true), MaxLength(4)]
        public string Code { get; set; }
        [MaxLength(128)]
        public string Stub2Lines { get; set; }
        [MaxLength(128)]
        public string Stub3Lines { get; set; }
        public byte[] Layout { get; set; }
    }
}
