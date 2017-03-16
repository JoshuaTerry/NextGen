using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Core
{
    [Table("NoteCategory")]
    public class NoteCategory : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(64)]
        public string Label { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Note> Notes { get; set; }

        public override string DisplayName => Name;
    }
}
