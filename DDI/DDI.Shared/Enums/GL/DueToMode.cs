
using System.ComponentModel;

namespace DDI.Shared.Enums.GL
{
    public enum DueToMode
    {
        [Description("None")]
        None = 0,

        [Description("Due From")]
        DueFrom = 1,

        [Description("Due To")]
        DueTo = 2
    }
}
