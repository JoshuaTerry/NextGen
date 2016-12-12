using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("ConstituentAlternateId")]
	public class ConstituentAlternateId
	{
		#region Public Properties
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
        public virtual AlternateId AlternateId { get; set; }
        public Guid? AlternateIdId { get; set; }
        public virtual Constituent Constituent { get; set; }
        public Guid? ConstituentId { get; set; }
        #endregion Public Properties
    }
}
