using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Core
{
    public class UserImportMappingField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public Guid UserImportMappingId { get; set; }

        [ForeignKey(nameof(UserImportMappingId))]
        public UserImportMapping UserImportMapping { get; set; }

        public Guid MappingTypeFieldId { get; set; }

        [ForeignKey(nameof(MappingTypeFieldId))]
        public MappingTypeField MappingTypeField { get; set; }

        [MaxLength(128)]
        public string FileFieldName { get; set; }
    }
}
