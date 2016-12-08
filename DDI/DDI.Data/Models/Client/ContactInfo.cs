using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("ContactInfo")]
	public class ContactInfo
	{
		#region Public Properties

		public string Comment { get; set; }

		public Constituent Constituent { get; set; }

		public Guid? ConstituentId { get; set; }

		public ContactType ContactType { get; set; }

		public Guid? ContactTypeId { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Info { get; set; }

		public bool IsPreferred { get; set; }

		public string Name { get; set; }

		#endregion Public Properties
	}
}
