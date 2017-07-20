using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Common
{
    [Table("Country")]
    public class Country : EntityBase
    {
        #region Public Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(4)]
        public string CountryCode { get; set; }
        [MaxLength(128)]
        public string Description { get; set; }

        [MaxLength(2)]
        public string ISOCode { get; set; }

        [MaxLength(4)]
        public string LegacyCode { get; set; }
        [MaxLength(20)]
        public string StateName { get; set; }

        [MaxLength(4)]
        public string StateAbbreviation { get; set; }
        [MaxLength(128)]
        public string PostalCodeFormat { get; set; }
        [MaxLength(128)]
        public string AddressFormat { get; set; }

        [MaxLength(4)]
        public string CallingCode { get; set; }

        [MaxLength(4)]
        public string InternationalPrefix { get; set; }

        [MaxLength(4)]
        public string TrunkPrefix { get; set; }

        public string PhoneFormat { get; set; }

        // Navigation Properties

        public ICollection<State> States { get; set; }
        #endregion

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return Description;
            }
        }

        #endregion

    }
}
