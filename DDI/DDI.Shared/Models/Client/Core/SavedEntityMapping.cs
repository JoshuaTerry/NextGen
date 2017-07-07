using DDI.Shared.Enums.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DDI.Shared.Models.Client.Core
{
    [Table("SavedEntityMapping")]
    public class SavedEntityMapping : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_Name", IsUnique = true), MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public EntityMappingType MappingType { get; set; }

        public ICollection<SavedEntityMappingField> MappingFields { get; set; }
    }
}
