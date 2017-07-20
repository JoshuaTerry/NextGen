using System.ComponentModel;

namespace DDI.Shared.Enums.GL
{
    public enum AccountCategory
    {
        [Description("None")]
        None = 0,

        [Description("Asset")]
        Asset = 1,

        [Description("Liability")]
        Liability = 2,

        [Description("Fund")]
        Fund = 3,

        [Description("Revenue")]
        Revenue = 4,

        [Description("Expense")]
        Expense = 5
    }
}
