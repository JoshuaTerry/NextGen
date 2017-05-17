using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("CustomField")]
    public class CustomField : AuditableEntityBase
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public string LabelText { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public int? DecimalPlaces { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
        public int? DisplayOrder { get; set; }
        public DDI.Shared.Enums.Common.CustomFieldEntity Entity { get; set; }
        public DDI.Shared.Enums.Common.CustomFieldType FieldType { get; set; }

        public ICollection<CustomFieldOption> Options { get; set; }
        public ICollection<CustomFieldData> Data { get; set; }

        [NotMapped]
        public override string DisplayName => LabelText;

        #endregion
    }
}
