using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("ImportFile")]
    public class ImportFile : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public bool ContainsHeaders { get; set; } = true;                

        public Guid FileId { get; set; }

        [ForeignKey("FileId")]
        public FileStorage File { get; set; }
    }
}
