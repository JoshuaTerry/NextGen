using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DDI.Data.Models.Common
{
    public enum EvenOddType { Any, Even, Odd }

    [Table("ZipPlus4")]
    public class ZipPlus4 : BaseEntity, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

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
