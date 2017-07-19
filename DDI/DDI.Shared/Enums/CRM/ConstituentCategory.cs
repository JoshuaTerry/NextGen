using System.ComponentModel;

namespace DDI.Shared.Enums.CRM
{
    public enum ConstituentCategory
    {
        [Description("Individual")]
        Individual = 0,

        [Description("Organization")]
        Organization = 1,

        [Description("Both")]
        Both = 2
    }
}
