﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Text;

namespace DDI.Shared.Models.Common
{
    [Table("Abbreviation")]
    public class Abbreviation : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(32)]
        public string Word { get; set; }

        [MaxLength(32)]
        public string USPSAbbreviation { get; set; }

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