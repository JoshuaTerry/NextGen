using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DDI.Data.Models.Common
{
    [Table("Country")]
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(4)]
        public string CountryCode { get; set; }

        public string Description { get; set; }

        [MaxLength(2)]
        public string ISOCode { get; set; }

        [MaxLength(4)]
        public string LegacyCode { get; set; }

        public string StateName { get; set; }

        [MaxLength(4)]
        public string StateAbbreviation { get; set; }

        public string PostalCodeFormat { get; set; }

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

    }
}
