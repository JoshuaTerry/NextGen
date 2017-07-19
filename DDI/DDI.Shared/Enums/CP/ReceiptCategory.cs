using System.ComponentModel;

namespace DDI.Shared.Enums.CP
{
    public enum ReceiptCategory
    {
        [Description("Cash")]
        Cash = 0,

        [Description("Check")]
        Check = 1,

        [Description("Credit Card")]
        CreditCard = 2,

        [Description("In Kind")]
        InKind = 3,

        [Description("EFT")]
        EFT = 4,

        [Description("Security")]
        Security = 5,

        [Description("Fee")]
        Fee = 6,

        [Description("Other")]
        Other = 7
    }
}
