using DDI.Shared.Enums.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Common
{

    [Table("ZipPlus4")]
    public class ZipPlus4 : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index]
        [MaxLength(16)]
        public string UpdateKey { get; set; }

        [MaxLength(16)]
        public string AddressLow { get; set; }

        [MaxLength(16)]
        public string AddressHigh { get; set; }

        [MaxLength(8)]
        public string SecondaryAbbreviation { get; set; }

        [MaxLength(16)]
        public string SecondaryLow { get; set; }

        [MaxLength(16)]
        public string SecondaryHigh { get; set; }

        [MaxLength(4)]
        public string Plus4 { get; set; }

        public EvenOddType AddressType { get; set; }

        public EvenOddType SecondaryType { get; set; }

        public bool IsRange { get; set; }

        public Guid? ZipStreetId { get; set; }

        // Navigation Properties

        public ZipStreet ZipStreet { get; set; }
    }
}
