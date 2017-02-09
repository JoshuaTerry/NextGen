
using System.ComponentModel;

namespace DDI.Shared.Enums.CRM
{
    public enum PaymentMethodType
    {
        [Description("None")]
        None = 0,

        [Description("Check")]
        Check = 1,

        [Description("ACH Transfer")]
        ACH = 2,

        [Description("Wire Transfer")]
        Wire = 3,

        [Description("SWIFT Transfer")]
        SWIFT = 4
    }

}
    