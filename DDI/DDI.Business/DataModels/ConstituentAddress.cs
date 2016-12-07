using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDI.Business.DataModels
{
    [Table("ConstituentAddress")]
    public class ConstituentAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public virtual Constituent Constituent { get; set; }
        public Guid? ConstituentId { get; set; }
        public virtual Address Address { get; set; }
        public Guid? AddressId { get; set; }
        public bool IsPrimary { get; set; }
    }
}