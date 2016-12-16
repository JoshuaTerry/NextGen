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
    public class State : BaseEntity, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(4)]
        public string StateCode { get; set; }

        public string Description { get; set; }

        [MaxLength(2)]
        public string FIPSCode { get; set; }

        public Guid? CountryId { get; set; }

        // Navigation Properties
        
        public Country Country { get; set; }

        public ICollection<County> Counties { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
