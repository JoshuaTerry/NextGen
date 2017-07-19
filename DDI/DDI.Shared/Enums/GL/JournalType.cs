
using System.ComponentModel;

namespace DDI.Shared.Enums.GL
{
    public enum JournalType
    {
        [Description("Normal")]
        Normal = 0,

        [Description("Recurring")]
        Recurring = 1,

        [Description("Template")]
        Template = 2,      
    }
}
