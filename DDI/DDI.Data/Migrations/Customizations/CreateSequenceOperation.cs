using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations.Model;

namespace DDI.Data.Migrations.Customizations
{
    /// <summary>
    /// EF Migration operation for creating a SQL sequence.
    /// </summary>
    public class CreateSequenceOperation : MigrationOperation
    {
        public CreateSequenceOperation(string sequenceName, string dataType, int startValue, int increment)
          : base(null)
        {
            SequenceName = sequenceName;
            DataType = dataType;
            StartValue = startValue;
            Increment = increment;
        }

        public string SequenceName { get; private set; }
        public string DataType { get; private set; }
        public int StartValue { get; set; }
        public int Increment { get; set; }


        public override bool IsDestructiveChange
        {
            get { return false; }
        }
    }
}
