using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Attributes.Models
{
    /// <summary>
    /// Attribute for specifying a human readable name for an entity model class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EntityNameAttribute : Attribute
    {
        public EntityNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
