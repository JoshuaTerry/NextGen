using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Enums.Core
{
    public enum RecurringType
    {
        [Description("None")]
        None = 0,

        [Description("On Hold")]
        OnHold = 1,

        [Description("Daily")]
        Daily = 2,

        [Description("Weekly")]
        Weekly = 3,

        [Description("Bi-Weekly")]
        BiWeekly = 4,

        [Description("Semi-Monthly")]
        SemiMonthly = 5,

        [Description("Monthly")]
        Monthly = 6,

        [Description("Bi-Monthly")]
        BiMonthly = 7,

        [Description("Quarterly")]
        Quarterly = 8,

        [Description("Semi-Annually")]
        SemiAnnually = 9,

        [Description("Yearly")]
        Yearly = 10,

        [Description("Period")]
        Period = 11
    }
}
