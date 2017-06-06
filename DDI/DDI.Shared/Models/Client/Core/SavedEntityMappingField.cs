using DDI.Shared.Enums.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Core
{
    [Table("SavedEntityMappingField")]
    public class SavedEntityMappingField : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid SavedEntityMappingId { get; set; }
        [ForeignKey(nameof(SavedEntityMappingId))]
        public SavedEntityMapping SavedEntityMapping { get; set; }

        public Guid EntityMappingId { get; set; }
        [ForeignKey(nameof(EntityMappingId))]
        public EntityMapping EntityMapping { get; set; }

        [MaxLength(128)]
        public string ColumnName { get; set; }
    }
}
