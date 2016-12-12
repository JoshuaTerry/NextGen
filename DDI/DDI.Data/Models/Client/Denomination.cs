using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("Denomination")]
	public class Denomination
	{
		#region Public Properties     	
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
        public string Affiliation { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
		public string Name { get; set; }
		public string Religion { get; set; }
        public virtual Constituent Constituent { get; set; }
        public Guid? ConstituentId { get; set; }
        #endregion Public Properties
    }
}
