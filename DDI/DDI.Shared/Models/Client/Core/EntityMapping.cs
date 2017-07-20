﻿using DDI.Shared.Enums.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("EntityMapping")]
    public class EntityMapping : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public EntityMappingType MappingType { get; set; }
        [MaxLength(128)]
        public string PropertyName { get; set; }
    }
}
