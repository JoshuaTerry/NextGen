using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IContactInfo
    {
        Guid Id { get; set; }
        string Name { get; set; }
        IConstituent Constituent { get; set; }
        string ContactType { get; set; }
        string Info { get; set; }
        string Comment { get; set; }
        bool IsPreferred { get; set; }
    }
}
