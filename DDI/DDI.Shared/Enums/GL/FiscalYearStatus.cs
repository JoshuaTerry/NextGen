
using System.ComponentModel;

namespace DDI.Shared.Enums.GL
{
    public enum FiscalYearStatus
    {
        [Description("Empty")]
        Empty = 0,

        [Description("Open")]
        Open = 1,

        [Description("Closed")]
        Closed = 2,

        [Description("Reopened")]
        Reopened = 3,

        [Description("Locked")]
        Locked = 4
    }
}
