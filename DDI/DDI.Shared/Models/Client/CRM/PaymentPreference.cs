using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("PaymentPreference")]
    public class PaymentPreference : EntityBase
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        public Constituent Constituent { get; set; }

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
