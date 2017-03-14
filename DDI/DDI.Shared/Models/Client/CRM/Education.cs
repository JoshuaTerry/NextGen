using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("Education")]
    public class Education : EntityBase
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(128)]
        public string Major { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public Guid? SchoolId { get; set; }
        [ForeignKey("SchoolId")]
        public School School { get; set;
        }

        [MaxLength(128)]
        public string SchoolOther { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        public Guid? DegreeId { get; set; }

        public Degree Degree { get; set; }
        
        [MaxLength(128)]
        public string DegreeOther { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }
        [ForeignKey("ConstituentId")]
        public Constituent Constituent { get; set; }

        public Guid? ConstituentId { get; set; }
        #endregion Public Properties
    }
}
