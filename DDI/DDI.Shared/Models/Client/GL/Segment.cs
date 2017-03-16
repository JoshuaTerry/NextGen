using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.GL
{
    public class Segment : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }
        public Guid? SegmentLevelId { get; set; }
        [ForeignKey(nameof(SegmentLevelId))]
        public SegmentLevel SegmentLevel { get; set; }
        public byte Level { get; set; }
        public string Code { get; set; }         
        public string Description { get; set; } 
        public Segment ParentSegment { get; set; }         
        public string LegacyKey { get; set; }
        public ICollection<Segment> ChildSegments { get; set; }
        public ICollection<AccountSegment> AccountSegments { get; set; }
    }
}
