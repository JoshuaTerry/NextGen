using System.ComponentModel;

namespace DDI.Shared.Enums.CRM
{
    public enum ConstituentBaseStatus
    {
        [Description("Active")]
        Active = 0,

        [Description("Inactive")]
        Inactive = 1,

        [Description("Blocked")]
        Blocked = 2
    }
}
