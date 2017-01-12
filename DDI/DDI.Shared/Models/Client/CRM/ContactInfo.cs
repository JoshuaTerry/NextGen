using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Attributes;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("ContactInfo"), EntityType("CRM_ContactInfo")]
    public class ContactInfo : EntityBase
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public string Info { get; set; }

        public bool IsPreferred { get; set; }       

        [MaxLength(255)]
        public string Comment { get; set; }

        public Guid? ConstituentId { get; set; }
        public Guid? ContactTypeId { get; set; }        

        // Navigation Properties
        public Constituent Constituent { get; set; }
        public ContactType ContactType { get; set; }

        #endregion Public Properties
    }
}
