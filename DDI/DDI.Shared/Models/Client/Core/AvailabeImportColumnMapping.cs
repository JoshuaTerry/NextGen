using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Core
{
    public class AvailabeImportColumnMapping : AuditableEntityBase 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public bool IsActive { get; set; }
           
        public string Name { get; set; }

        public string EntityType { get; set; }
        public string MappingType { get; set; }
    }
}
