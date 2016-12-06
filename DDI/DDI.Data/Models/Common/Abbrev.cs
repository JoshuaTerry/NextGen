using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DDI.Data.Models.Common
{
    [Table("Abbrev")]
    public class Abbrev
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(32)]
        public string Word { get; set; }

        [MaxLength(32)]
        public string USPSAbbrev { get; set; }

        [MaxLength(32)]
        public string AddressWord { get; set; }

        [MaxLength(32)]
        public string NameWord { get; set; }

        public bool IsSuffix { get; set; }

        public bool IsCaps { get; set; }

        public bool IsSecondary { get; set; }

        public int Priority { get; set; }

    }
}
