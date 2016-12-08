﻿using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Business.DataModels
{
    [Table("Address")]
    public class Address 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public Guid? StateId { get; set; }
        public State State { get; set; }
        public string Zip { get; set; }
    }
}