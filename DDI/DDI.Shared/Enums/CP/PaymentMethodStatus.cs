using System;
using System.ComponentModel;

namespace DDI.Shared.Enums.CP
{
    public enum PaymentMethodStatus
    {
        [Description("Active")]
        Active = 0,

        [Description("Inactive")]
        Inactive = 1,

        [Description("Pre-note Required")]
        PrenoteRequired = 2,

        [Description("Pre-note Sent")]
        PrenoteSent = 3,

        [Description("Expired")]
        Expired = 4,

        [Description("Not Valid")]
        NotValid = 5
    }

}
