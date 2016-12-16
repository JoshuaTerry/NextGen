using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
    [Table("Denomination")]
    public class Denomination : BaseEntity, IEntity
    {
        #region Public Properties       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(128)]
        public string Affiliation { get; set; }

        [MaxLength(128)]
        public string Code { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Religion { get; set; }

        public ICollection<Constituent> Constituents { get; set; }
        #endregion Public Properties
    }
}
