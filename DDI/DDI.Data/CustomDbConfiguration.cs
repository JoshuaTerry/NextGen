using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DDI.Data.Migrations.Customizations;

namespace DDI.Data
{
    /// <summary>
    /// EF db configuration customizations.
    /// </summary>
    public class CustomDbConfiguration : DbConfiguration
    {
        public CustomDbConfiguration()
        {
            SetMigrationSqlGenerator("System.Data.SqlClient", () => new CustomSqlGenerator());
        }
    }
}
