using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations.Model;

namespace DDI.Data.Migrations.Customizations
{
    /// <summary>
    /// EF Migration operation for dropping a SQL sequence.
    /// </summary>
    public class DropSequenceOperation : MigrationOperation
    {
        public DropSequenceOperation(string sequenceName)
          : base(null)
        {
            SequenceName = sequenceName;
        }

        public string SequenceName { get; private set; }

        public override bool IsDestructiveChange
        {
            get { return false; }
        }
    }
}
