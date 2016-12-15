using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
    [Table("ContactInfo")]
    public class ContactInfo
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Info { get; set; }

        public bool IsPreferred { get; set; }       

        [MaxLength(255)]
        public string Comment { get; set; }

        public Guid? ConstituentId { get; set; }
        public Guid? ContactTypeId { get; set; }        

        // Navigation Properties
        public virtual Constituent Constituent { get; set; }
        public virtual ContactType ContactType { get; set; }

        #endregion Public Properties
    }
}
