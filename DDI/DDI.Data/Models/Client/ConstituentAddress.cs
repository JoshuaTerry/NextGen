using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("ConstituentAddress")]
	public class ConstituentAddress
	{
		#region Public Properties

		public virtual Address Address { get; set; }

		public Guid? AddressId { get; set; }

		public virtual Constituent Constituent { get; set; }

		public Guid? ConstituentId { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public bool IsPrimary { get; set; }

		#endregion Public Properties
	}
}
