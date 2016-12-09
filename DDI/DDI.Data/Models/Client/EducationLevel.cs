using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("EducationLevel")]
	public class EducationLevel
	{
		#region Public Properties

		public string Code { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public bool IsActive { get; set; }

		public string Name { get; set; }
        public virtual Education Education { get; set; }

        public Guid? EducationId { get; set; }
        #endregion Public Properties
    }
}
