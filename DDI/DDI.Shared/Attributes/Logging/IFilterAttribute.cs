using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDI.Shared.Attributes.Logging
{
    public interface IFilterAttribute
    {
        bool ShouldLog();
    }
}
