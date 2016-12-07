using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDI.Business.DataModels
{
    [Table("ConstituentDba")]
    public class ConstituentDba
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public virtual DoingBusinessAs DoingBusinessAs { get; set; }
        public Guid? DoingBusinessAsId { get; set; }
        public virtual Constituent Constituent { get; set; }
        public Guid? ConstituentId { get; set; }
    }
}