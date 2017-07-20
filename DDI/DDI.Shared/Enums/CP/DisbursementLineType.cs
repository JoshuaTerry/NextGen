using System.ComponentModel;

namespace DDI.Shared.Enums.CP
{
    public enum DisbursementLineType
    {
        [Description("None")]
        None = 0,

        [Description("Amount")]
        Amount = 1,

        [Description("Memo")]
        Memo = 2
    }
}
