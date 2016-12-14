using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
    [Table("PaymentPreference")]
    public class PaymentPreference
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public virtual Constituent Constituent { get; set; }

        public Guid? ConstituentId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public int ABANumber { get; set; }

        [MaxLength(128)]
        public string AccountNumber { get; set; }

        [MaxLength(128)]
        public string AccountType { get; set; }
        #endregion Public Properties
    }
}
