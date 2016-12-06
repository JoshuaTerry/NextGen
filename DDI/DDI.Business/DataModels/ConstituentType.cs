using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Business.DataModels
{
    [Table("ConstituentType")]
    public class ConstituentType :IConstituentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        //Question - JLT Is the code still important or was it the Id in the old system?
        public string Code { get; set; }

        public string Description { get; set; }

        public string BaseType { get; set; }

        public bool IsActive { get; set; }

        public bool IsRequired { get; set; }
    }
}