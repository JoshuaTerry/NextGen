using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DDI.Data.Models.Common
{
    [Table("Zip")]
    public class Zip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(5)]
        public string ZipCode { get; set; }

        [Column(TypeName ="money")]
        public decimal CoordinateNS { get; set; }

        [Column(TypeName = "money")]
        public decimal CoordinateEW { get; set; }
        
        public Guid? CityId { get; set; }

        // Navigation Properties

        public virtual City City { get; set; }

        public virtual ICollection<ZipBranch> ZipBranches { get; set; }

        public virtual ICollection<ZipStreet> ZipStreets { get; set; }
    }
}
