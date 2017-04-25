using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;

namespace DDI.Shared.Attributes.Transactions
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class TransactionTypeAttribute : Attribute
    {
        public string Description { get; private set; }
        public ModuleType ModuleType { get; set; }
        public string EntityType { get; set; }

        public TransactionTypeAttribute(string description)
        {
            Description = description;
        }
    }
}
