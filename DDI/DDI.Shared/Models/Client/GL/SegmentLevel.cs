using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.GL
{
    public class SegmentLevel : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Ledger Ledger { get; set; }         
        public byte Level { get; set; }         
        public SegmentType Type { get; set; }         
        public SegmentFormat Format { get; set; }         
        public byte Length { get; set; }         
        public bool IsLinked { get; set; }         
        public bool IsCommon { get; set; }         
        public string Description { get; set; }         
        public string Abbreviation { get; set; }         
        public string Separator { get; set; }         
        public byte SortOrder { get; set; }
        public ICollection<Segment> Segments { get; set; }
    }
}
