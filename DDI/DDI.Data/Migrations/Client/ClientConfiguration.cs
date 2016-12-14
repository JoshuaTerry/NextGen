using System;
using System.Data.Entity.Migrations;
using System.Linq;

using DDI.Data.Enums;
using DDI.Data.Models;
using DDI.Data.Models.Client;

namespace DDI.Data.Migrations.Client
{
    /// <summary>
    /// Migration configuration class for client databases
    /// </summary>
	internal sealed class ClientConfiguration : DbMigrationsConfiguration<DomainContext>
	{
         /*
          * Commands
          * add-migration <Name> -ConfigurationTypeName ClientConfiguration
          * update-database -ConfigurationTypeName ClientConfiguration
          */

        #region Public Constructors

        public ClientConfiguration()
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

            //Additional Setup of CRM data will be required
            //CRM initial Data

            //ConstituentStatuses
            context.ConstituentStatuses.AddOrUpdate(
                p => p.Code,
                new ConstituentStatus { Code = "AC", Name = "Active", IsActive = true },
                new ConstituentStatus { Code = "IN", Name = "Inactive", IsActive = true });

            //Constituent Types
            context.ConstituentTypes.AddOrUpdate(
                p => p.Code, 
                new ConstituentType { BaseType = ConstituentCategory.Individual.ToString(), Code = "I", Name = "Individual", IsActive = true, IsRequired = true },
                new ConstituentType { BaseType = ConstituentCategory.Organization.ToString(), Code = "O", Name = "Organization", IsActive = true, IsRequired = true }
            );

            //Genders
            context.Genders.AddOrUpdate(
                p => p.Code,
                new Gender { Code = "M", IsMasculine = true, Name = "Male"},
                new Gender { Code = "F", IsMasculine = false, Name = "Female"}
            );
        }

		#endregion Protected Methods
	}
}
