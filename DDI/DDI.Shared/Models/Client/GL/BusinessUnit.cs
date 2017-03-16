using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class BusinessUnit : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }

        public BusinessUnitType BusinessUnitType { get; set; }

        public string Code { get; set; }
                
        // JFA:
        
        // Also missing is a way to link users to entities.  That was what the EntityUser table was for in the old NextGen.

    }
}
