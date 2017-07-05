
using System.ComponentModel;

namespace DDI.Shared.Enums.GL
{
    public enum PriorPeriodPostingMode
    {
        [Description("Allow")]
        Allow = 0,

        [Description("Security")]
        Security = 1,

        [Description("Prohibit")]
        Prohibit = 2
    }
}
