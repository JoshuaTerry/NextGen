﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client.Core
{
    [Table("CustomFields")]
    public class CustomFields : IEntity
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
        public DDI.Data.Enums.Common.CustomFieldEntity Entity { get; set; }
        public DDI.Data.Enums.Common.CustomFieldType FieldType { get; set; }

        public ICollection<CustomFieldOptions> Options { get; set; }

        public CustomFieldData Answer { get; set; }

        [NotMapped]
        public string DisplayName { get; set; }

        #endregion
    }
}
