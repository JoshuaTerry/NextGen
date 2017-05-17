using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;
using Nest;

namespace DDI.Search.Models
{
    /// <summary>
    /// Elasticsearch note document (CORE).
    /// </summary>
    [ElasticsearchType(Name = "note")]
    public class NoteDocument : ISearchDocument, IAutoMappable
    {
        [Keyword(IncludeInAll = false)]
        public Guid Id { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? CategoryId { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? NoteCodeId { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? PrimaryContactId { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? ContactMethodId { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? UserResponsibleId { get; set; }

        [Keyword(IncludeInAll = false)]
        public Guid? ParentEntityId { get; set; }

        [Keyword(IncludeInAll = false)]
        public string EntityType { get; set; }
        
        [Date]
        public DateTime? ContactDate { get; set; }

        [Date]
        public DateTime? AlertStartDate { get; set; }

        [Date]
        public DateTime? AlertEndDate { get; set; }

        [Text]
        public string Title { get; set; }

        [Text]
        public string Text { get; set; }

        
     
        
        public void AutoMap(MappingsDescriptor mappings)
        {
            mappings.Map<NoteDocument>(p => p.AutoMap());            
        }

    }
}
