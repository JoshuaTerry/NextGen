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
    public class SavedEntityMapping : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EntityMappingType MappingType { get; set; }
        public ICollection<SavedEntityMappingField> MappingFields { get; set; }
    }
}
