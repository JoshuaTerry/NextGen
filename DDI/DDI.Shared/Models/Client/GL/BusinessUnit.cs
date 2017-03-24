﻿using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("BusinessUnit")]
    public class BusinessUnit : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_Name", IsUnique = true), MaxLength(128)]
        public string Name { get; set; }

        public BusinessUnitType BusinessUnitType { get; set; }

        [Index("IX_Code", IsUnique = true), MaxLength(16)]
        public string Code { get; set; }
                
        ICollection<User> Users { get; set; }
    }
}