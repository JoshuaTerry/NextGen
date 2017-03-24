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
    [Table("SegmentLevel")]
    public class SegmentLevel : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_Name", IsUnique = true, Order = 1)]
        [Index("IX_Level", IsUnique = true, Order = 1)]
        public Guid? LedgerId { get; set; }
        [ForeignKey(nameof(LedgerId))]
        public Ledger Ledger { get; set; }

        [Index("IX_Level", IsUnique = true, Order = 2)]
        public int Level { get; set; }         

        public SegmentType Type { get; set; }         

        public SegmentFormat Format { get; set; }         

        public int Length { get; set; }         

        public bool IsLinked { get; set; }

        public bool IsCommon { get; set; }         
        
        [Index("IX_Name", IsUnique = true, Order = 2), MaxLength(40)]
        public string Name { get; set; }         

        [MaxLength(16)]
        public string Abbreviation { get; set; }         

        [MaxLength(1)]
        public string Separator { get; set; }         

        public int SortOrder { get; set; }

        public ICollection<Segment> Segments { get; set; }
    }
}
