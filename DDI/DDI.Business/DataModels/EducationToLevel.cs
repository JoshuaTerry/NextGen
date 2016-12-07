using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDI.Business.DataModels
{
    [Table("EducationToLevel")]
    public class EducationToLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public virtual EducationLevel EducationLevel { get; set; }
        public Guid? EducationLevelId { get; set; }
        public virtual Education Education { get; set; }
        public Guid? EducationId { get; set; }

    }
}