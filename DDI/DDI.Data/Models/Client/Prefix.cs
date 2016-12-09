using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("Prefix")]
	public class Prefix
	{
		#region Public Properties 
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public Gender Gender { get; set; }
        public Guid? GenderId { get; set; }
        #endregion Public Properties
    }
}
