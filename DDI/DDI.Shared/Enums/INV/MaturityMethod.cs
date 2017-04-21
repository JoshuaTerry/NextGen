
using System.ComponentModel;

namespace DDI.Shared.Enums.INV
{
    public enum MaturityMethod
    {
        [Description("Redeem Investment, No Action")]
        Redeem = 0,

        [Description("Redeem Investment via Check")]
        Check = 1,

        [Description("Redeem Investment via EFT")]
        EFT = 2,

        [Description("Redeem Investment into Same Investment Type")]
        SameType = 3,

        [Description("Redeem Investment into Other Investment Type")]
        OtherType = 4

    }

}
