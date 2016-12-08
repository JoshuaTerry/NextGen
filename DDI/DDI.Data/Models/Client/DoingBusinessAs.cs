using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("DoingBusinessAs")]
	public class DoingBusinessAs
	{
		#region Public Properties

		public DateTime? EndDate { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public DateTime? StartDate { get; set; }

		#endregion Public Properties
	}
}
