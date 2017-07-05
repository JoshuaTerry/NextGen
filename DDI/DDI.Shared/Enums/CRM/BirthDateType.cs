
using System.ComponentModel;

namespace DDI.Shared.Enums.CRM
{
    public enum BirthDateType
    {
        [Description("None")]
        None = 0,

        [Description("Age Range")]
        AgeRange = 1,

        [Description("Full Date")]
        FullDate = 2,

        [Description("Month Day")]
        MonthDay = 3
	}

}
