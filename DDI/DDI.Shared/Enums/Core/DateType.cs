
using System.ComponentModel;

namespace DDI.Shared.Enums.Core
{
    public enum DateType
    {
        [Description("Month Day Year")]
        MonthDayYear = 0,

        [Description("Month Year")]
        MonthYear = 1,

        [Description("Year")]
        Year = 2,

        [Description("Month Day")]
        MonthDay = 3
	}

}
