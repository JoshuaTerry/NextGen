using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Data.Models
{
	/// <summary>
	/// Information about an event logged by the application.
	/// </summary>
	/// <remarks>
	/// The table name is set explicitly to prevent Entity Framework from creating tables based on
	/// the DbSet names in the context class ("LogEntries" instead of "LogEntry").
	/// </remarks>
	[Table("LogEntry")]
	public class LogEntry
	{
		#region Public Properties

		[Required]
		public DateTime Date { get; set; }

		[StringLength(2000)]
		public string Exception { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[Required]
		[StringLength(50)]
		public string Level { get; set; }

		[Required]
		[StringLength(255)]
		public string Logger { get; set; }

		[Required]
		[StringLength(4000)]
		public string Message { get; set; }

		[Required]
		[StringLength(255)]
		public string Thread { get; set; }

		#endregion Public Properties
	}
}
