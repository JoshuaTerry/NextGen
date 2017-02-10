using System;
using System.ComponentModel;

namespace DDI.Shared.Enums.CP
{
    public enum PaymentMethodCategory
    {
        [Description("Other")]
        Other = 0,

        [Description("EFT")]
        EFT = 1,

        [Description("Card")]
        Card = 2

    }

}
