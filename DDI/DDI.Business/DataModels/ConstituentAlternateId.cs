using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace DDI.Business.DataModels
{
    [Table("ConstituentAlternateId")]
    public class ConstituentAlternateId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public virtual AlternateId AlternateId { get; set; }
        public Guid? AlternateIdId { get; set; }
        public virtual Constituent Constituent { get; set; }
        public Guid? ConstituentId { get; set; }
    }
}