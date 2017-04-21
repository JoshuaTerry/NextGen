using System.ComponentModel;

namespace DDI.Shared.Enums.INV
{
    public enum InterestPaymentMethod
    {
        [Description("Compound")]
        Compound = 0,

        [Description("EFT")]
        EFT = 1,

        [Description("Check")]
        Check = 2,

        [Description("Investment Deposit")]
        InvestmentDeposit = 3,

        
    }

}
