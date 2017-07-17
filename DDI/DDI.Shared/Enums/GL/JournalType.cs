using System.ComponentModel;

namespace DDI.Shared.Enums.GL
{
    public enum JournalType
    {
        [Description("One-Time")]
        OneTime = 0,

        [Description("Recurring")]
        Recurring = 1,

        [Description("Template")]
        Template = 2,      
    }
}
