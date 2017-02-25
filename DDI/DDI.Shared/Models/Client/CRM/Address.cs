using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("Address")]
    public class Address : EntityBase
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(255)]
        public string AddressLine1 { get; set; }

        [MaxLength(255)]
        public string AddressLine2 { get; set; }

        [MaxLength(128)]
        public string City { get; set; }

        public Guid? CountryId { get; set; }

        public Guid? CountyId { get; set; }

        [MaxLength(128)]
        public string PostalCode { get; set; }

        public Guid? StateId { get; set; }
        
        public int LegacyKey { get; set; } // Used for data conversion

        public Guid? Region1Id { get; set; }
        public Guid? Region2Id { get; set; }
        public Guid? Region3Id { get; set; }
        public Guid? Region4Id { get; set; }

        // Non-mapped properties

        private Country _country = null;
        [NotMapped]
        public Country Country
        {
            get
            {
                return _country;
            }
            set
            {
                CountryId = value?.Id;
                _country = value;
            }
        }

        private State _state = null;
        [NotMapped]
        public State State
        {
            get
            {
                return _state;
            }
            set
            {
                StateId = value?.Id;
                _state = value;
            }
        }

        private County _county = null;
        [NotMapped]
        public County County
        {
            get
            {
                return _county;
            }
            set
            {
                CountyId = value?.Id;
                _county = value;
            }
        }

        // Navigation Properties
        public ICollection<ConstituentAddress> ConstituentAddresses { get; set; }

        public Region Region1 { get; set; }
        public Region Region2 { get; set; }
        public Region Region3 { get; set; }
        public Region Region4 { get; set; }

        #endregion Public Properties
    }
}
