using System;
using System.Data.Entity.Migrations;
using System.Linq;

using DDI.Data.Enums.CRM;
using DDI.Data.Models.Client.CRM;

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
                new ConstituentStatus { Code = "AC", Name = "Active", IsActive = true, BaseStatus = ConstituentBaseStatus.Active, IsRequired = true },
                new ConstituentStatus { Code = "IN", Name = "Inactive", IsActive = true, BaseStatus = ConstituentBaseStatus.Inactive, IsRequired = true },
                new ConstituentStatus { Code = "BL", Name = "Blocked", IsActive = true, BaseStatus = ConstituentBaseStatus.Blocked, IsRequired = true }
                );

            //Constituent Types
            context.ConstituentTypes.AddOrUpdate(
                p => p.Code, 
                new ConstituentType { Category = ConstituentCategory.Individual, Code = "I", Name = "Individual", IsActive = true, IsRequired = true, NameFormat = "{P}{F}{MI}{L}{S}", SalutationFormal = "Dear {P}{L}", SalutationInformal = "Dear {N}" },
                new ConstituentType { Category = ConstituentCategory.Organization, Code = "O", Name = "Organization", IsActive = true, IsRequired = true, SalutationFormal = "Dear Friends", SalutationInformal = "Dear Friends" },
                new ConstituentType { Category = ConstituentCategory.Organization, Code = "C", Name = "Church", IsActive = true, IsRequired = true, SalutationFormal = "Dear Friends", SalutationInformal = "Dear Friends" },
                new ConstituentType { Category = ConstituentCategory.Individual, Code = "F", Name = "Family", IsActive = true, IsRequired = true, NameFormat = "The {F}{MI}{L} Family", SalutationFormal = "Dear Friends", SalutationInformal = "Dear Friends" }
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
