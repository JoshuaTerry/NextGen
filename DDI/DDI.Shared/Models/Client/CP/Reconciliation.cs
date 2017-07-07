using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CP
{
    [Table("Reconciliation")]
    public class Reconciliation : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
        [MaxLength(512)]
        public string Notes { get; set; }
    }
}
