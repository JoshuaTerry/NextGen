using System.ComponentModel;

namespace DDI.Shared.Enums.Common
{
    public enum CustomFieldType
    {
        [Description("Number")]
        Number = 0,

        [Description("TextBox")]
        TextBox = 1,

        [Description("TextArea")]
        TextArea = 2,

        [Description("DropDown")]
        DropDown = 3,

        [Description("Radio")]
        Radio = 4,

        [Description("CheckBox")]
        CheckBox = 5,

        [Description("Date")]
        Date = 6,

        [Description("DateTime")]
        DateTime = 7
    }
}
