using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations.Model;

namespace DDI.Data.Migrations.Customizations
{
    /// <summary>
    /// EF Migration operation for dropping a SQL view.
    /// </summary>
    public class DropViewOperation : MigrationOperation
    {
        public DropViewOperation(string viewName)
          : base(null)
        {
            ViewName = viewName;
        }

        public string ViewName { get; private set; }

        public override bool IsDestructiveChange
        {
            get { return false; }
        }
    }
}
