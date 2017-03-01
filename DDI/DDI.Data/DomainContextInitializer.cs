using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Data
{
    public class DomainContextInitializer : IDatabaseInitializer<DomainContext>
    {
        private DropCreateDatabaseIfModelChanges<DomainContext> wrapped;

        public DomainContextInitializer()
        {
            wrapped = new DropCreateDatabaseIfModelChanges<DomainContext>();
        }

        public void InitializeDatabase(DomainContext context)
        {
            string databaseName = context.Database.Connection.Database;
            wrapped.InitializeDatabase(context);
            context.Database.ExecuteSqlCommand(
                TransactionalBehavior.DoNotEnsureTransaction,
                string.Format("ALTER DATABASE [{0}] SET READ_COMMITTED_SNAPSHOT ON", databaseName));
            context.Database.ExecuteSqlCommand(
                TransactionalBehavior.DoNotEnsureTransaction,
                string.Format("ALTER DATABASE [{0}] SET ALLOW_SNAPSHOT_ISOLATION ON", databaseName));
        }
    }
}
