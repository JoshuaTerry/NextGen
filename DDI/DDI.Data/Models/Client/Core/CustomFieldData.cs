using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client.Core
{
    [Table("CustomFieldData")]
    public class CustomFieldData : IEntity
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid CustomFieldId { get; set; }
        public string EntityType { get; set; }
        public Guid? ParentEntityId { get; set; }
        public string Value { get; set; }
        public CustomField CustomField { get; set; }

        [NotMapped]
        public string DisplayName { get; set; }

        #endregion
    }
}
