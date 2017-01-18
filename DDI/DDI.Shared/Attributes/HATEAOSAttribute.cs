using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Attributes
{
    /// <summary>
    /// Attribute for specifying an entity type string value which is used for entity linkage.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HateoasAttribute : Attribute
    {
        public HateoasAttribute(string routeName)
        {
            RouteName = routeName;
        }
        [Required]
        public string RouteName { get; private set; }
    }
}
