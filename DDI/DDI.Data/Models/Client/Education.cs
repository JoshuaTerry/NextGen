using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
    [Table("Education")]
    public class Education : BaseEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        [MaxLength(128)]
        public string Major { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string School { get; set; }

        [MaxLength(128)]
        public string SchoolCode { get; set; }

        [MaxLength(128)]
        public string SchoolOther { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [MaxLength(128)]
        public string DegreeCode { get; set; }

        [MaxLength(128)]
        public string DegreeOther { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public ICollection<EducationLevel> EducationLevels { get; set; }
        #endregion Public Properties
    }
}
