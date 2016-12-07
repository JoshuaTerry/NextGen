using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DDI.Data.Migrations.Common
{
	internal sealed class CommonConfiguration : DbMigrationsConfiguration<CommonContext>
	{
        /*
         * Commands
         * add-migration <Name> -ConfigurationTypeName CommonConfiguration
         * update-database -ConfigurationTypeName CommonConfiguration
         */

		#region Public Constructors

		public CommonConfiguration()
		{
			AutomaticMigrationsEnabled = false;
		}

		#endregion Public Constructors

		#region Protected Methods
        
		protected override void Seed(CommonContext context)
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
