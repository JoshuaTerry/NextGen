using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Core
{
    public enum MappingType {  Accounts = 0, Budget = 1 }
    public class ImportMappingDefinition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [MaxLength(128)]
        [Index("IX_Name", IsUnique = true)]
        public MappingType MappingType { get; set; }

        public ICollection<MappingTypeField> Fields { get; set; }

    }
}
