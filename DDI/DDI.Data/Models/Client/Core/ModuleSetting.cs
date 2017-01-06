using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Enums;

namespace DDI.Data.Models.Client.Core
{
    [Table("ModuleSetting")]
    public class ModuleSetting : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public ModuleType ModuleType { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(4096)]
        public string Value { get; set; }

        public string DisplayName => $"{ModuleType}.{Name}";
    }
}
