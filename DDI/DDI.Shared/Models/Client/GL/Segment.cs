using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.GL
{
    [Table("GL_Segment")]
    public class Segment : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        public Guid SegmentLevelId { get; set; }
        [ForeignKey(nameof(SegmentLevelId))]
        public SegmentLevel SegmentLevel { get; set; }

        public int Level { get; set; }

        [MaxLength(30)]
        public string Code { get; set; }         

        [MaxLength(255)]
        public string Description { get; set; } 

        public Guid? ParentSegmentId { get; set; }
        [ForeignKey(nameof(ParentSegmentId))]
        public Segment ParentSegment { get; set; }         

        [InverseProperty(nameof(ParentSegment))]
        public ICollection<Segment> ChildSegments { get; set; }

        public ICollection<AccountSegment> AccountSegments { get; set; }
    }
}
