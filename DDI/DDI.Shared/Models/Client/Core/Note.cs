using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Client.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("Note")]
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
        [ForeignKey("CategoryId")]
        public NoteCategory Category { get; set; }

        public Guid? NoteCodeId { get; set; }
        public NoteCode NoteCode { get; set; }

        public Guid? PrimaryContactId { get; set; }
        [ForeignKey("PrimaryContactId")]
        public Constituent PrimaryContact { get; set; }

        public Guid? ContactMethodId { get; set; }
        [ForeignKey("ContactMethodId")]
        public NoteContactMethod ContactMethod { get; set; }

        public Guid? UserResponsibleId { get; set; }
        public User UserResponsible { get; set; }

        public ICollection<NoteTopic> NoteTopics { get; set; } 
    }
}
