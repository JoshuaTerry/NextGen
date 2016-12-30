using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Data.Attributes
{
    /// <summary>
    /// Attribute for specifying an entity type string value which is used for entity linkage.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EntityTypeAttribute : Attribute
    {
        public EntityTypeAttribute(string entityTypeName)
        {
            EntityTypeName = entityTypeName;
        }
        [Required]
        public string EntityTypeName { get; private set; }
    }
}
