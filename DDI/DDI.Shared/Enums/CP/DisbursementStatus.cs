using System.ComponentModel;

namespace DDI.Shared.Enums.CP
{
    public enum DisbursementStatus
    {
        [Description("None")]
        None = 0,

        [Description("Unprinted")]
        Unprinted = 1,

        [Description("Printed")]
        Printed = 2,

        [Description("Void")]
        Void = 3,

        [Description("Unsent")]
        Unsent = 4,

        [Description("Sent")]
        Sent = 5,

        [Description("Accepted")]
        Accepted = 6,

        [Description("Denied")]
        Denied = 7
    }
}
