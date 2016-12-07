using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDI.Business.DataModels
{
    [Table("ConstituentPaymentPreference")]
    public class ConstituentPaymentPreference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public virtual PaymentPreference PaymentPreference { get; set; }
        public Guid? PaymentPreferenceId { get; set; }
        public virtual Constituent Constituent { get; set; }
        public Guid? ConstituentId { get; set; }
    }
}