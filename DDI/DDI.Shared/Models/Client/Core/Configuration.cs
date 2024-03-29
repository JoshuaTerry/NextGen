﻿using DDI.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("Configuration")]
    public class Configuration : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public ModuleType ModuleType { get; set; }

        // TODO: Add property for Company (Entity) to allow for company-specific settings.

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(4096)]
        public string Value { get; set; }

        public override string DisplayName => $"{ModuleType}.{Name}";
    }
}
