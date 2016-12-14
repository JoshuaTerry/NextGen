using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using DDI.Data.Models.Common;

namespace DDI.Data.Models.Client
{
    [Table("Address")]
    public class Address
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string StreetAddress { get; set; }

        [MaxLength(128)]
        public string City { get; set; }

        public Guid? CountryId { get; set; }

        public Guid? CountyId { get; set; }

        [MaxLength(128)]
        public string PostalCode { get; set; }

        public Guid? StateId { get; set; }
        
        public int LegacyKey { get; set; } // Used for data conversion

        // Non-mapped properties
        [NotMapped]
        public Country Country { get; set; }

        [NotMapped]
        public State State { get; set; }

        [NotMapped]
        public County County { get; set; }

        // Navigation Properties
        public virtual ICollection<ConstituentAddress> ConstituentAddresses { get; set; }
        
        #endregion Public Properties
    }
}
