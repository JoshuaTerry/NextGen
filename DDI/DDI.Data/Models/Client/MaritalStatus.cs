﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Data.Models.Client
{
    [Table("MaritialStatus")]
    public class MaritalStatus : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }         
        [MaxLength(128)]
        public string Name { get; set; }
    }
}
