using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("MaritialStatus")]
    public class MaritalStatus : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }         
        [MaxLength(128)]
        public string Name { get; set; }
    }
}
