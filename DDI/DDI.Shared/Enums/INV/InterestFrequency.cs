
using System.ComponentModel;

namespace DDI.Shared.Enums.INV
{
    public enum InterestFrequency
    {
        [Description("None")]
        None = 0,

        [Description("Monthly")]
        Monthly = 1,

        [Description("Quarterly")]
        Quarterly = 2,

        [Description("Semi-Annual")]
        SemiAnnual = 3,

        [Description("Annual")]
        Annual = 4,

        [Description("Maturity Only")]
        Maturity = 5

    }

}
