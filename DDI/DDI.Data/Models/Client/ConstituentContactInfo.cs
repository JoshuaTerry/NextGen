using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("ConstituentContactInfo")]
	public class ConstituentContactInfo
	{
		#region Public Properties
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
        public virtual Constituent Constituent { get; set; }
        public Guid? ConstituentId { get; set; }
        public virtual ContactInfo ContactInfo { get; set; }
        public Guid? ContactInfoId { get; set; }
        #endregion Public Properties
    }
}
