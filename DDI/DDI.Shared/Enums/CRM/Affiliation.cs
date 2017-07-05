using System.ComponentModel;

namespace DDI.Shared.Enums.CRM
{
    public enum Affiliation
    {
        [Description("None")]
        None = 0,

        [Description("Affiliated")]
        Affiliated = 1,

        [Description("Unaffiliated")]
        Unaffiliated = 2,

        [Description("Other")]
        Other = 3
    }

}
