
using System.ComponentModel;

namespace DDI.Shared.Enums.CRM
{
    public enum CorrespondencePreference
    {
        [Description("None")]
        None = 0,

        [Description("Paper")]
        Paper = 1,

        [Description("Email")]
        Email = 2,

        [Description("Both")]
        Both = 3
	}
}
