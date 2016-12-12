using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("IncomeLevel")]
	public class IncomeLevel
	{
		#region Public Properties 
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public bool IsActive { get; set; }
		public string Name { get; set; }
        public string Code { get; set; }
        #endregion Public Properties
    }
}
