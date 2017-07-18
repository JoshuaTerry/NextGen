using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("Attachment")]
    public class Attachment : LinkedEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        public Guid? FileId { get; set; }
        [ForeignKey(nameof(FileId))]
        public FileStorage File { get; set; }

        public Guid? NoteId { get; set; }
        [ForeignKey(nameof(NoteId))]
        public Note Note { get; set; }

        public override string DisplayName => File?.Name ?? string.Empty;
    }
}
