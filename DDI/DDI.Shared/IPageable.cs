using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IPageable
    {
        int? Offset { get; set; }
        int? Limit { get; set; }
        string OrderBy { get; set; }
    }
}
