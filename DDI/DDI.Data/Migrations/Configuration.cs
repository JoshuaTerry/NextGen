namespace DDI.Data.Migrations
{
    using Shared.Models.Client.Security;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DDI.Data.DomainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DDI.Data.DomainContext context)
        {
            context.Roles.AddOrUpdate(r => r.Id, new Role() { Id = Guid.Parse("01983988-A48B-475E-80AD-A877B68F90F7"), Name = "CRM-Read" }
                                               , new Role() { Id = Guid.Parse("05CF9C0C-8812-46AA-8BDC-861FED09D261"), Name = "CRM-Read/Write" }
                                               , new Role() { Id = Guid.Parse("BE3A7107-E761-4F04-A1AC-5B5D535301CD"), Name = "CRM-Settings-Read" }
                                               , new Role() { Id = Guid.Parse("915A5AA3-CD5E-4DC3-AF2E-C28AD03C7D85"), Name = "CRM-Settings-Read/Write" }
                                               , new Role() { Id = Guid.Parse("C4271C57-97AA-4E73-94B5-49A5119624F4"), Name = "PaymentPreferences-Read" }
                                               , new Role() { Id = Guid.Parse("A6C2B449-1E69-47F2-B1F3-5BF2994DD8B7"), Name = "PaymentPreferences-Read/Write" }
                                               , new Role() { Id = Guid.Parse("36049936-AB7F-4D59-ACF2-50A941329DFC"), Name = "TaxId-Read" }
                                               , new Role() { Id = Guid.Parse("1F6ACCB8-947A-4E3F-A323-B555525E8534"), Name = "TaxId-Read/Write" }
                                               , new Role() { Id = Guid.Parse("7F46F412-50E8-40D5-B1BD-5B38CFC476DD"), Name = "Notes-Read" }
                                               , new Role() { Id = Guid.Parse("D6F7D868-2BE0-4D06-8C90-F972075832F8"), Name = "Notes-Read/Write" }
                                               , new Role() { Id = Guid.Parse("03EA61E0-36E8-4399-B010-2576B7906885"), Name = "Settings-Read" }
                                               , new Role() { Id = Guid.Parse("2E7DC8E9-A333-4A30-A9A1-ECAB6D0749AF"), Name = "Settings-Read/Write" }
            );

            //  This method will be called after migrating to the latest version.

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
    }
}
