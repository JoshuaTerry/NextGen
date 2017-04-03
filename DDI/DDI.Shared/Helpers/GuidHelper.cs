using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Helpers
{
    public static class GuidHelper
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern int UuidCreateSequential(out Guid guid);

        private static byte[] _macAddress = { 0x00, 0xC0, 0xDD, 0x01, 0x02, 0x5C }; // Actual MAC address of a discarded QLogic card.

        /// <summary>
        /// Determines whether NewGuid() will return random or sequential Guids.  This should be set to false in normal runtime environments.
        /// </summary>
        public static bool GenerateSequentialGuids { get; set; } = false;

        /// <summary>
        /// Generate a new sequential Guid for testing or data conversion.
        /// </summary>
        public static Guid NewSequentialGuid()
        {
            // NOTE:  This method should not be called from different nodes or workstations at the same time, as the MAC address part of the GUID is overwritten for
            // security reasons.
            //
            Guid result;
            UuidCreateSequential(out result);
            byte[] bytes = result.ToByteArray();
            Array.ConstrainedCopy(_macAddress, 0, bytes, 10, 6);
            return new Guid(bytes);
        }

        /// <summary>
        /// Generate a new Guid based on the GenerateSequentialGuids setting.
        /// </summary>
        public static Guid NewGuid()
        {
            return GenerateSequentialGuids ? NewSequentialGuid() : Guid.NewGuid();
        }

    }
}
