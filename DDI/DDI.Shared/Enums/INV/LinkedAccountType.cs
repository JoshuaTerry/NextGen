
using System.ComponentModel;

namespace DDI.Shared.Enums.INV
{
    public enum LinkedAccountType
    {
        [Description("Loan Support")]
        LoanSupport = 0,

        [Description("Pool")]
        Pool = 1,

        [Description("Down Payment")]
        DownPayment = 2,

        [Description("Grant")]
        Grant = 3
    }
}
