
using System.ComponentModel;

namespace DDI.Shared.Enums.INV
{
    public enum InvestorStatus
    {
        [Description("None")]
        None = 0,

        [Description("Active Investor")]
        INVA = 1,

        [Description("Dormant Investor")]
        INVD = 2,

        [Description("Holder of Extended Investment - No Recent Activity")]
        INVI = 3,

        [Description("Former Investor = All Investments Paid")]
        INVR = 4,
        
    }

}
    