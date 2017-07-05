
using System.ComponentModel;

namespace DDI.Shared.Enums.INV
{
    public enum InvestmentAutomatedTransactionMethod
    {
        [Description("Deposit")]
        Deposit = 0,

        [Description("Withdrawal")]
        Withdrawal = 1,

        [Description("Transfer")]
        Transfer = 2       
    }
}
