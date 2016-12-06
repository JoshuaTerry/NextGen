using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Business.DataModels
{
    [Table("ContactInfo")]
    public class ContactInfo : IContactInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IConstituent Constituent { get; set; }
        public string ContactType { get; set; }
        public string Info { get; set; }
        public string Comment { get; set; }
        public bool IsPreferred { get; set; }
    }
}