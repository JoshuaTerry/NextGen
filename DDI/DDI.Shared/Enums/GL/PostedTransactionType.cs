
using System.ComponentModel;

namespace DDI.Shared.Enums.GL
{
    public enum PostedTransactionType
    {
        [Description("Actual")]
        Actual = 0,

        [Description("Close From")]
        CloseFrom = 1,

        [Description("Close To")]
        CloseTo = 2,

        [Description("Begin Balance")]
        BeginBal = 3,

        [Description("End Balance")]
        EndBal = 4
    }
}
