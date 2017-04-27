using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DDI.Shared.Models.Client.Core
{
    public class UserImportMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public MappingType MappingType { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
    }
}
