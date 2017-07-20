using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;

namespace DDI.Shared.Models.Client.Core
{
    [Table("CustomFieldData")]
    public class CustomFieldData : AuditableEntityBase
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public DDI.Shared.Enums.Common.CustomFieldEntity EntityType { get; set; }

        public Guid? ParentEntityId { get; set; }

        [MaxLength(256)]
        public string Value { get; set; }

        public Guid CustomFieldId { get; set; }
        [ForeignKey("CustomFieldId")]
        public CustomField CustomField { get; set; }

        [NotMapped]
        public override string DisplayName => Value;

        #endregion
    }
}
