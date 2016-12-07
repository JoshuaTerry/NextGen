using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Business.DataModels
{
    [Table("Prefix")]
    public class Prefix  
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Abbreviation { get; set; }
        public string Descriptin { get; set; }
        public Gender Gender { get; set; }
        public Guid? GenderId { get; set; }
    }
}