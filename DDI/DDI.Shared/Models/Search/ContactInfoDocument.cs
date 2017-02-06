using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Search
{
    /// <summary>
    /// Elasticsearch ContactInfo document (CRM)
    /// </summary>
    public class ContactInfoDocument : ISearchDocument
    {
        public Guid Id { get; set; }

        public Guid ContactCategoryId { get; set; }
        public string Info { get; set; }
        public bool IsPrefrerred { get; set; }
        public string Comment { get; set; }

    }
}
