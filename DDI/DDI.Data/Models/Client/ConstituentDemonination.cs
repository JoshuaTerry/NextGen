using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("ConstituentDemonination")]
	public class ConstituentDemonination
	{
		#region Public Properties

		public virtual Constituent Constituent { get; set; }

		public Guid? ConstituentId { get; set; }

		public virtual Denomination Denomination { get; set; }

		public Guid? DenominationId { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		#endregion Public Properties
	}
}
