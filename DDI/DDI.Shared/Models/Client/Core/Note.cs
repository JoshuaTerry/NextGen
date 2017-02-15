using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Shared.Models.Client.Core
{
    public class Note : LinkedEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public int LegacyId { get; set; }
        [MaxLength(64)]
        public string Title { get; set; }
        [MaxLength(4000)]
        public string Text { get; set; }
        public DateTime? AlertStartDate { get; set; }
        public DateTime? AlertEndDate { get; set; }
        public DateTime? ContactDate { get; set; }
        public NoteCategory NoteCategory { get; set; }
        public string NoteCode { get; set; }
        public Constituent PrimaryContact { get; set; }
        public NoteContactCode ContactCode { get; set; }

        public ICollection<NoteTopic> NoteTopics { get; set; } 
    }
}
