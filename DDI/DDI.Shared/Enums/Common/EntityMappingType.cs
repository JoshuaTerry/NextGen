using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Enums.Common
{
    public enum EntityMappingType
    {
        [Description("Budget")]
        Budget = 0
    }

    public enum EntityMappingPropertyType
    {
        [Description("String")]
        String = 0,

        [Description("DateTime")]
        DateTime = 1,

        [Description("Numeric")]
        Numeric = 2
    }
}
