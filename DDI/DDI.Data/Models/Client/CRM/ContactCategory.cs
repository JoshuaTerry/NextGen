using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client.CRM
{
    [Table("ContactCategory")]
    public class ContactCategory : BaseEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        [MaxLength(1)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string SectionTitle { get; set; }

        [MaxLength(128)]
        public string TextBoxLabel { get; set; }

        public Guid? DefaultContactTypeID { get; set; }

        public ContactType DefaultContactType { get; set; }

        public ICollection<ContactType> ContactTypes { get; set; }


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
