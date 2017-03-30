using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations.Model;

namespace DDI.Data.Migrations.Customizations
{
    /// <summary>
    /// EF Migration operation for creating a SQL view.
    /// </summary>
    public class CreateViewOperation : MigrationOperation
    {
        public CreateViewOperation(string viewName, string viewQueryString)
          : base(null)
        {
            ViewName = viewName;
            ViewString = viewQueryString;
        }

        public string ViewName { get; private set; }
        public string ViewString { get; private set; }

        public override bool IsDestructiveChange
        {
            get { return false; }
        }
    }
}
