
using System.ComponentModel;

namespace DDI.Shared.Enums.INV
{
    public enum InvestorStatus
    {
        [Description("None")]
        None = 0,

        [Description("Active Investor")]
        ActiveInvestor = 1,

        [Description("Dormant Investor")]
        DormantInvestor = 2,

        [Description("Holder of Extended Investment - No Recent Activity")]
        ExtendedInvestor = 3,

        [Description("Former Investor = All Investments Paid")]
        FormerInvestor = 4,
        
    }
}
    