using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class AccountSegment : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public int Level { get; set; } 
        public Guid AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }
        public Guid SegmentId { get; set; }
        [ForeignKey(nameof(SegmentId))]
        public Segment Segment { get; set; }
        public Guid SegmentLevelId { get; set; }
        [ForeignKey(nameof(SegmentLevelId))]
        public SegmentLevel SegmentLevel { get; set; }         
    }
}
