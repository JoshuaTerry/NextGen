using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("Gender")]
	public class Gender
	{
		#region Public Properties

		public string Code { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public bool? IsMasculine { get; set; }

		public string Name { get; set; }

		#endregion Public Properties
	}
}
