using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("ConstituentPaymentPreference")]
	public class ConstituentPaymentPreference
	{
		#region Public Properties

		public virtual Constituent Constituent { get; set; }

		public Guid? ConstituentId { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public virtual PaymentPreference PaymentPreference { get; set; }

		public Guid? PaymentPreferenceId { get; set; }

		#endregion Public Properties
	}
}
