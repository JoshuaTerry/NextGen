using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("ClergyType")]
	public class ClergyType
	{
		#region Public Properties
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
		#endregion Public Properties
	}
}
