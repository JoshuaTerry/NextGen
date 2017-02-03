using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Statics.CP
{
    public static class UserMessagesCP
    {
        public static string EFTFormatRequired => "EFT Format is required.";
        public static string EFTRoutingNumberDigits => "Routing number must have nine digits.";
        public static string EFTRoutingNumberZero => "Routing number cannot be all zeros.";
        public static string EFTRoutingNumberNotValid => "Invalid routing number (based on check digit.)";

    }
}
