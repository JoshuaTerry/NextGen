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

        [MaxLength(256)]
        public string Title { get; set; }

        public string Text { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AlertStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AlertEndDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ContactDate { get; set; }

        public Guid? CategoryId { get; set; }
        public NoteCategory Category { get; set; }

        [MaxLength(32)]
        public string NoteCode { get; set; }

        public Guid? PrimaryContactId { get; set; }
        public Constituent PrimaryContact { get; set; }

        public Guid? ContactMethodId { get; set; }
        public NoteContactMethod ContactMethod { get; set; }

        public ICollection<NoteTopic> NoteTopics { get; set; } 
    }
}
