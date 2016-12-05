using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Data.Models;

namespace DDI.Data
{
	public class DomainContext : DbContext
	{
		#region Public Properties

		public virtual DbSet<LogEntry> LogEntries { get; set; }

		#endregion Public Properties

		#region Public Constructors

		public DomainContext()
			: base("name=DomainContext")
		{
		}

		#endregion Public Constructors
	}
}
