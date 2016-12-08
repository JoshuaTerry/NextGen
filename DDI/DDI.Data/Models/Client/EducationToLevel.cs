using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("EducationToLevel")]
	public class EducationToLevel
	{
		#region Public Properties

		public virtual Education Education { get; set; }

		public Guid? EducationId { get; set; }

		public virtual EducationLevel EducationLevel { get; set; }

		public Guid? EducationLevelId { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		#endregion Public Properties
	}
}
