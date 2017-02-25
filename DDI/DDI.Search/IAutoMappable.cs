using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace DDI.Search
{
    public interface IAutoMappable
    {
        void AutoMap(MappingsDescriptor mappings);
    }
}
