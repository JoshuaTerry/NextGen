using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DDI.Data.Models.Common
{
    [Table("County")]
    public class County
    {
        [Key]
        public Guid Id { get; set; }

        public string Description { get; set; }

        [MaxLength(5)]
        public string FipsCode { get; set; }

        [MaxLength(4)]
        public string LegacyCode { get; set; }

        public Guid? StateId { get; set; }

        public int Population { get; set; }

        [Column(TypeName = "money")]
        public decimal PopPerSqMile { get; set; }

        [Column(TypeName = "money")]
        public decimal PopPctChange { get; set; }

        // Navigation Properties
        
        public virtual State State { get; set; }

        public ICollection<City> Cities { get; set; }

    }
}
