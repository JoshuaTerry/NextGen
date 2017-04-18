using System.ComponentModel;

namespace DDI.Shared.Enums.INV
{
    public enum InterestPaymentMethod
    {
        [Description("Compound")]
        Compound = 0,

        [Description("ACH")]
        ACH = 1,

        [Description("Check")]
        Check = 2,

        [Description("Investment Deposit")]
        InvestmentDeposit = 3,

        [Description("Wire")]
        Wire = 4

    }

}
