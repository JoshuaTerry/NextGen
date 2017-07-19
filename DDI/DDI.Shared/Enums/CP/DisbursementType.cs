using System.ComponentModel;

namespace DDI.Shared.Enums.CP
{
    public enum DisbursementType
    {
        [Description("None")]
        None = 0,

        [Description("Check")]
        Check = 1,

        [Description("EFT")]
        EFT = 2,

        [Description("Other")]
        Other = 3
    }
}
