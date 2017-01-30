using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Conversion.Statics
{
    /// <summary>
    /// Directory names for conversion files.
    /// </summary>
    internal static class DirectoryName
    {
        public static string DataDirectory => @"\\ddifs2\ddi\DDI\Dept 00 - Common\Projects\NextGen\Conversion\Data";
        public static string OutputDirectory => @"\\ddifs2\ddi\DDI\Dept 00 - Common\Projects\NextGen\Conversion\IS_Conversion_Payload";

        #region Module Subdirectories

        public static string CRM => "CRM";
        public static string GL => "GL";
        public static string CP => "CP";

        #endregion
    }
}
