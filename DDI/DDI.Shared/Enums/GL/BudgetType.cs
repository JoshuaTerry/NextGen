using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Enums.GL
{
    public enum BudgetType
    {
        [Description("Fixed")]
        Fixed = 0,

        [Description("Working")]
        Working = 1,

        [Description("What If")]
        WhatIf = 2
    }
}
