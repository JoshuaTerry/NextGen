using System.ComponentModel.DataAnnotations;

namespace DDI.Shared.Models.Client.Core
{
    public class MappableEntityField
    {
        [MaxLength(128)]
        public string PropertyName { get; set; }
        [MaxLength(128)]
        public string ColumnName { get; set; }
    }
}
