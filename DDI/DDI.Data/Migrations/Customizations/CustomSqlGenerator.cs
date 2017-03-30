using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Utilities;
using System.Data.Entity.SqlServer;

namespace DDI.Data.Migrations.Customizations
{
    /// <summary>
    /// Class for implementing the CreateViw and DropView migration operations.
    /// </summary>
    public class CustomSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(MigrationOperation migrationOperation)
        {
            if (migrationOperation is CreateViewOperation)
            {
                var operation = (CreateViewOperation)migrationOperation;

                using (IndentedTextWriter writer = Writer())
                {
                    writer.WriteLine("CREATE VIEW {0} AS {1} ; ",
                                      operation.ViewName,
                                      operation.ViewString);
                    Statement(writer);
                }
            }
            else if (migrationOperation is DropViewOperation)
            {
                var operation = (DropViewOperation)migrationOperation;

                using (IndentedTextWriter writer = Writer())
                {
                    writer.WriteLine("DROP VIEW {0} ; ", operation.ViewName);
                    Statement(writer);
                }
            }
        }
    }
}
