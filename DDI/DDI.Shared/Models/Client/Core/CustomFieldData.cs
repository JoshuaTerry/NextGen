using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.Core
{
    [Table("CustomFieldData")]
    public class CustomFieldData : EntityBase
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public Guid CustomFieldId { get; set; }
        public DDI.Shared.Enums.Common.CustomFieldEntity EntityType { get; set; }
        public Guid? ParentEntityId { get; set; }
        public string Value { get; set; }

        [NotMapped]
        public string DisplayName { get; set; }

        #endregion
    }
}
