using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums.GL;

namespace DDI.Services.Search
{
    public class NoteSearch : PageableSearch
    {
        public Guid? ParentEntityId { get; set; }
        public string EntityType { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? NoteCodeId { get; set; }
        public Guid? PrimaryContactId { get; set; }
        public Guid? ContactMethodId { get; set; }
        public Guid? UserResponsibleId { get; set; }
        public DateTime? ContactDateFrom { get; set; }
        public DateTime? ContactDateTo { get; set; }
        public DateTime? AlertDate { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

    }
}
