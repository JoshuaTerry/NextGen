
using System.ComponentModel;

namespace DDI.Shared.Enums.CRM
{
    public enum SalutationType
    {
        [Description("Default")]
        Default = 0,

        [Description("Formal")]
        Formal = 1,

        [Description("Informal")]
        Informal = 2,

        [Description("Formal Separate")]
        FormalSeparate = 3,

        [Description("Informal Separate")]
        InformalSeparate = 4,

        [Description("Custom")]
        Custom = 5
	}

}
