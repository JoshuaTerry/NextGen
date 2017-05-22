using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models
{
    public class DatabaseConstraintException : Exception
    {
        public DatabaseConstraintException() : base("The record you are trying to add or edit conflicts with an existing record.") { }
    }

    public class DatabaseConstraintDeleteException : Exception
    {
        public DatabaseConstraintDeleteException() : base("The record you are trying to delete is currently in use.") { }
    }
}
