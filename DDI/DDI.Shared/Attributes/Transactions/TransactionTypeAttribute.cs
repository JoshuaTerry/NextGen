using DDI.Shared.Enums;
using System;

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
