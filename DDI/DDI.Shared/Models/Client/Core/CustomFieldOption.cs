using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("CustomFieldOption")]
    public class CustomFieldOption : AuditableEntityBase
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(16)]
        public string Code { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public int SortOrder { get; set; }

        public Guid CustomFieldId { get; set; }
        [ForeignKey("CustomFieldId")]
        public CustomField CustomField { get; set; }

        [NotMapped]
        public override string DisplayName
        {
            get
            {
                return string.Format("{0}: {1}", Code, Description);
            }
        }

        #endregion
    }
}
