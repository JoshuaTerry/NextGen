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
        public Account Account { get; set; }

        public Guid SegmentId { get; set; }
        public Segment Segment { get; set; }

    }
}
