using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using DDI.Data.Models.Common;

namespace DDI.Data.Models.Client
{
	[Table("Address")]
	public class Address
	{
		#region Public Properties

		public string City { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Line1 { get; set; }

		public string Line2 { get; set; }

		public State State { get; set; }

		public Guid? StateId { get; set; }

		public string Zip { get; set; }

		#endregion Public Properties
	}
}
