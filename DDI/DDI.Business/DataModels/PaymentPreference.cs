using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Business.DataModels
{
    [Table("PaymentPreference")]
    public class PaymentPreference  
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ABANumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
    }
}