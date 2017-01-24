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
        public static string NAMPAFormat => "XXX-XXX-XXXX";
        public static string NAMPACountryCode => "1";
        public static string DefaultInternationalPrefix => "00";
        public static string InternationalDialingSymbol => "+";
    }
}
