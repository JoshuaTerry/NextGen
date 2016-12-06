﻿using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Business.DataModels
{
    [Table("Education")]
    public class Education : IEducation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string School { get; set; }
        public IEducationLevel Degree { get; set; }
        public string Major { get; set; }
    }
}