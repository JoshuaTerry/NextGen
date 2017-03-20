using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Common
{
    [Table("City")]
    public class City : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(8)]
        public string PlaceCode { get; set; }

        public Guid? StateId { get; set; }
        public Guid? CountyId { get; set; }

        public int Population { get; set; }

        [Column(TypeName = "money")]
        public decimal PopulationPercentageChange { get; set; }

        [Column(TypeName = "money")]
        public decimal CoordinateNS { get; set; }

        [Column(TypeName = "money")]
        public decimal CoordinateEW { get; set; }        

        // Navigation Properties

        public State State { get; set; }

        public County County { get; set; }

        public ICollection<CityName> CityNames { get; set; }

        public ICollection<Zip> Zips { get; set; }

    }
}
