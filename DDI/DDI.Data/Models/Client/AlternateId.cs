using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("AlternateId")]
	public class AlternateId
	{
		#region Public Properties

		//Is this 1to1 or 1toMany?
		public Constituent Constituent { get; set; }

		public Guid? ConstituentId { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		#endregion Public Properties
	}
}
