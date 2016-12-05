using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DDI.Data.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<DomainContext>
	{
		#region Public Constructors

		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		#endregion Public Constructors

		#region Protected Methods

		protected override void Seed(DomainContext context)
		{
			// This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//
		}

		#endregion Protected Methods
	}
}
