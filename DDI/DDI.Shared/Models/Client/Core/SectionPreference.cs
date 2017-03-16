using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("SectionPreference")]
    public class SectionPreference : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        [MaxLength(128)]
        public string SectionName { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Value { get; set; }
        public bool IsShown { get; set; }       
    }
}
