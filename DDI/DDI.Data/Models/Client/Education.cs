using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("Education")]
	public class Education
	{
		#region Public Properties
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public string Major { get; set; }
		public string Name { get; set; }
		public string School { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolOther { get; set; }
		public DateTime? Start { get; set; }
        public EducationLevel Degree { get; set; }
        public Guid? DegreeId { get; set; }
        public string DegreeCode { get; set; }
        public string DegreeOther { get; set; }
        public DateTime? End { get; set; }
        public virtual ICollection<EducationLevel> EducationLevels { get; set; }
        #endregion Public Properties
    }
}
