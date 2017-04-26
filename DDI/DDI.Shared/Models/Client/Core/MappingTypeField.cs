using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Core
{
    public class MappingTypeField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public Guid ImportMappingDefinitionId { get; set; }

        [ForeignKey(nameof(ImportMappingDefinitionId))]
        public ImportMappingDefinition ImportMappingDefinition { get; set; }
         
        public string Name { get; set; }
    }
}
