using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Search
{
    public class ContactInfo
    {
        public Guid Id { get; set; }

        public Guid ContactCategoryId { get; set; }
        public string Info { get; set; }
        public bool IsPrefrerred { get; set; }
        public string Comment { get; set; }

    }
}
