using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DDI.Data.Models.Common
{
    [Table("State")]
    public class State
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(4)]
        public string StateCode { get; set; }

        public string Description { get; set; }

        [MaxLength(2)]
        public string FipsCode { get; set; }

        public Guid? CountryId { get; set; }

        // Navigation Properties
        
        public virtual Country Country { get; set; }

        public virtual ICollection<County> Counties { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
