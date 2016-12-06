using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IGender
    {
        Guid Id { get; set; }
        bool? IsMasculine { get; set; }
        string Name { get; set; }
    }
}
