using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.Core
{
    [Table("CustomFieldData")]
    public class CustomFieldData : AuditableEntityBase
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid CustomFieldId { get; set; }
        public DDI.Shared.Enums.Common.CustomFieldEntity EntityType { get; set; }
        public Guid? ParentEntityId { get; set; }
        public string Value { get; set; }
        [ForeignKey("CustomFieldId")]
        public CustomField CustomField { get; set; }

        [NotMapped]
        public override string DisplayName => Value;

        #endregion
    }
}
