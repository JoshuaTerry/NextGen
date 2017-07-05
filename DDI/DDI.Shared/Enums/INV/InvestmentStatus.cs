
using System.ComponentModel;

namespace DDI.Shared.Enums.INV
{
    public enum InvestmentStatus
    {
        [Description("Setup")]
        Setup = 0,

        [Description("Current")]
        Current = 1,

        [Description("Redeemed")]
        Redeemed = 2
    }
}
    