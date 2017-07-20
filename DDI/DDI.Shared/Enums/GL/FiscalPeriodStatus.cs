
using System.ComponentModel;

namespace DDI.Shared.Enums.GL
{
    public enum FiscalPeriodStatus
    {
        [Description("Open")]
        Open = 0,

        [Description("Closed")]
        Closed = 1,

        [Description("Reopened")]
        Reopened = 2
    }
}
