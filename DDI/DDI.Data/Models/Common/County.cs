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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Description { get; set; }

        [MaxLength(5)]
        public string FIPSCode { get; set; }

        [MaxLength(4)]
        public string LegacyCode { get; set; }

        public Guid? StateId { get; set; }

        public int Population { get; set; }

        [Column(TypeName = "money")]
        public decimal PopulationPerSqaureMile { get; set; }

        [Column(TypeName = "money")]
        public decimal PopulationPercentageChange { get; set; }

        // Navigation Properties
        
        public virtual State State { get; set; }

        public virtual ICollection<City> Cities { get; set; }

    }
}
