using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("ConstituentEducation")]
	public class ConstituentEducation
	{
		#region Public Properties

		public virtual Constituent Constituent { get; set; }

		public Guid? ConstituentId { get; set; }

		public virtual Education Education { get; set; }

		public Guid? EducationId { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		#endregion Public Properties
	}
}
