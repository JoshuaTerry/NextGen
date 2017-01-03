using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client.CRM
{
    [Table("ContactType")]
    public class ContactType : BaseEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(128)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public Guid? ContactCategoryId { get; set; }

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
