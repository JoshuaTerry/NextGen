using System.Data.Entity.Migrations;

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


        }

        #endregion Protected Methods
    }
}
