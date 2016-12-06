using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IAlternateId
    {
        Guid Id { get; set; }
        string Name { get; set; }
        IConstituent Constituent { get; set; }
    }
}
