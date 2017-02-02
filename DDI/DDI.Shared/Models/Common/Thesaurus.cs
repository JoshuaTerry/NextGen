using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Text;

namespace DDI.Shared.Models.Common
{
    [Table("Thesaurus")]
    public class Thesaurus
    {
        [Key,MaxLength(50)]
        public string Word { get; set; }
       
        public string Expansion { get; set; }

    }
}
