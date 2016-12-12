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
        public string Name { get; set; }
        public int ABANumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        #endregion Public Properties
    }
}
