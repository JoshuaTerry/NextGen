﻿
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("GLAccountSelection")]
    public class GLAccountSelection :  EntityBase
    {
        [Key]
        public override Guid Id { get; set; }

        public string AccountNumber { get; set; }
        public string Description { get; set; }
        public string Level1 { get; set; }
        public string Level2 { get; set; }
        public string Level3 { get; set; }
        public string Level4 { get; set; }
        public int? LevelSequence1 { get; set; }
        public int? LevelSequence2 { get; set; }
        public int? LevelSequence3 { get; set; }
        public int? LevelSequence4 { get; set; }

        public Guid LedgerId { get; set; }
        public Guid FiscalYearId { get; set; }



    }

}
