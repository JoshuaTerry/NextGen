using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Common
{
    [Table("Thesaurus")]
    public class Thesaurus
    {
        [Key, MaxLength(50)]
        public string Word { get; set; }

        [MaxLength(50)]
        public string Expansion { get; set; }

    }
}
