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
    public class EntityMapping : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public EntityMappingType MappingType { get; set; }

        public string PropertyName { get; set; }

        public string PropertyValue { get; set; }

        public EntityMappingPropertyType PropertyType { get; set; }
    }
}
