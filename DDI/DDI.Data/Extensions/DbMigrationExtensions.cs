using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using DDI.Data.Migrations.Customizations;

namespace DDI.Data.Extensions
{
    public static class DbMigrationExtensions
    {
        /// <summary>
        /// Add a SQL view to a migration.
        /// </summary>
        public static void CreateView(this DbMigration migration, string viewName, string viewqueryString)
        {
            ((IDbMigration)migration)
              .AddOperation(new CreateViewOperation(viewName, viewqueryString));
        }

        /// <summary>
        /// Drop a SQL view from a migration.
        /// </summary>
        public static void DropView(this DbMigration migration, string viewName)
        {
            ((IDbMigration)migration)
              .AddOperation(new DropViewOperation(viewName));
        }

        /// <summary>
        /// Add a SQL sequence to a migration.
        /// </summary>
        public static void CreateSequence(this DbMigration migration, string sequenceName, string dataType = "int", int startValue = 1, int increment = 1)
        {
            ((IDbMigration)migration)
              .AddOperation(new CreateSequenceOperation(sequenceName, dataType, startValue, increment));
        }

        /// <summary>
        /// Drop a SQL sequence from a migration.
        /// </summary>
        public static void DropSequence(this DbMigration migration, string sequenceName)
        {
            ((IDbMigration)migration)
              .AddOperation(new DropSequenceOperation(sequenceName));
        }

    }
}
