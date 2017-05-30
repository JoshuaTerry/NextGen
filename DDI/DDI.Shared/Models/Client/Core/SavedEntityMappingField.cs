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
    public class SavedEntityMappingField : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid SavedEntityMappingId { get; set; }
        [ForeignKey("SavedEntityMappingId")]
        public SavedEntityMapping SavedEntityMapping { get; set; }
        public Guid EntityMappingId { get; set; }
        [ForeignKey("EntityMappingId")]
        public EntityMapping EntityMapping { get; set; }
        public string ColumnName { get; set; }
    }
}
