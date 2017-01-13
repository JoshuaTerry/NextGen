using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.Core
{
    [Table("CustomField")]
    public class CustomField : IEntity
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
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

        public CustomFieldData Answer { get; set; }

        [NotMapped]
        public string DisplayName { get; set; }

        #endregion
    }
}
