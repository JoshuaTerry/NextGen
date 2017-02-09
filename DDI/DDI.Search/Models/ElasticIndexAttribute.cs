using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Search.Attributes
{
    /// <summary>
    /// Attribute for specifying an elasticsearch index
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ElasticIndexAttribute : Attribute
    {
        public ElasticIndexAttribute(string indexSuffix)
        {
            IndexSuffix = indexSuffix;
        }

        [Required]
        public string IndexSuffix { get; private set; }
    }
}
