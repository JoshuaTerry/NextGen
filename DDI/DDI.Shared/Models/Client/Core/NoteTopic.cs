﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Core
{
    public class NoteTopic : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        [MaxLength(64)]
        public string Code { get; set; }
        [MaxLength(128)]
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Note> Notes { get; set; }
    }
}
