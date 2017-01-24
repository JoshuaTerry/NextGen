using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Statics.CRM
{
    /// <summary>
    /// Address formatting macros, used in Country.AddressFormat
    /// </summary>
    public static class ContactInfoDefaults
    {
        /// <summary>
        /// Standard North American Numbering Plan format: "XXX-XXX-XXXX"
        /// </summary>
        public static string NANPFormat => "XXX-XXX-XXXX";

        /// <summary>
        /// Country calling code for NANP countries.
        /// </summary>
        public static string NANPCallingCode => "1";

        /// <summary>
        /// Trunk prefix for NANP country phone numbers.
        /// </summary>
        public static string NANPTrunkPrefix = "1-";

        /// <summary>
        /// International dialing prefix for US: "011"
        /// </summary>
        public static string DefaultInternationalPrefix => "011";

        /// <summary>
        /// '+' - Char. often added to beginning of international phone numbers before the country calling code.
        /// </summary>
        public static char CallingCodeIndicator => '+';
    }
}
