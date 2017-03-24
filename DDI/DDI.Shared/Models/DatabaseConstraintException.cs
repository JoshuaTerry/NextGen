using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models
{
    public class DatabaseConstraintException : Exception
    {
        public DatabaseConstraintException() : base("Insert or update violates contraint for Uniqueness.") { }
    }

    public class DatabaseConstraintDeleteException : Exception
    {
        public DatabaseConstraintDeleteException() : base("The entity you are trying to delete is currently in use.") { }
    }
}
