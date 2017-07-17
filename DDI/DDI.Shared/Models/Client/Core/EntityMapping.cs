using DDI.Shared.Enums.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("EntityMapping")]
    public class EntityMapping : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public EntityMappingType MappingType { get; set; }
        public bool IsRequired { get; set; } = false;
        public int SortOrder { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
        [MaxLength(128)]
        public string PropertyName { get; set; }
    }
}
