using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DDI.Data.Models.Common
{
    [Table("City")]
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(8)]
        public string PlaceCode { get; set; }

        public Guid? StateId { get; set; }
        public Guid? CountyId { get; set; }

        public int Population { get; set; }

        [Column(TypeName = "money")]
        public decimal PopPctChange { get; set; }

        [Column(TypeName = "money")]
        public decimal CoordNS { get; set; }

        [Column(TypeName = "money")]
        public decimal CoordEW { get; set; }

        public int OEOid { get; set; }

        // Navigation Properties

        public virtual State State { get; set; }

        public virtual County County { get; set; }

        public virtual ICollection<CityName> CityNames { get; set; }

        public virtual ICollection<Zip> Zips { get; set; }

    }
}
