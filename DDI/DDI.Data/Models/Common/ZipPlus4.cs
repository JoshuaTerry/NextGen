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
    public class ZipPlus4
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(16)]
        public string UpdateKey { get; set; }

        [MaxLength(16)]
        public string AddrLow { get; set; }

        [MaxLength(16)]
        public string AddrHigh { get; set; }

        [MaxLength(8)]
        public string SecondaryAbbrev { get; set; }

        [MaxLength(16)]
        public string SecondaryLow { get; set; }

        [MaxLength(16)]
        public string SecondaryHigh { get; set; }

        [MaxLength(4)]
        public string Plus4 { get; set; }

        public EvenOddType AddrType { get; set; }

        public EvenOddType SecondaryType { get; set; }

        public bool IsRange { get; set; }

        public Guid? ZipStreetId { get; set; }

        // Navigation Properties

        public virtual ZipStreet ZipStreet { get; set; }

    }
}
