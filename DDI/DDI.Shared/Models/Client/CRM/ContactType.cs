using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("ContactType")]
    public class ContactType : AuditableEntityBase, ICodeEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public bool IsActive { get; set; }

        [Index("IX_Code", Order = 2, IsUnique = true), MaxLength(4)]
        public string Code { get; set; }

        [Index("IX_Name", IsUnique = true), MaxLength(128)]
        public string Name { get; set; }
        [Index("IX_Code", Order = 1, IsUnique = true)]
        public Guid? ContactCategoryId { get; set; }
        [ForeignKey("ContactCategoryId")]
        public ContactCategory ContactCategory { get; set; }

        public bool IsAlwaysShown { get; set; }

        public bool CanDelete { get; set; }

        [InverseProperty("DefaultContactType")]
        public ICollection<ContactCategory> ContactCategoryDefaults { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return Name;
            }
        }

        #endregion

    }
}
