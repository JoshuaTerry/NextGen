using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Business.Tests.Helpers
{
    public static class GuidHelper
    {
        private static uint _counter = 0;
        private static byte[] _guidBytes = null;

        /// <summary>
        /// Return a new sequential Guid for test data.
        /// </summary>
        /// <returns></returns>
        public static Guid NextGuid()
        {
            if (_guidBytes == null)
            {
                _guidBytes = Guid.NewGuid().ToByteArray();

            }

            return new Guid(++_counter, (ushort)(_guidBytes[4] | (_guidBytes[5] << 8)), (ushort)(_guidBytes[6] | (_guidBytes[7] << 8)),
                _guidBytes[8], _guidBytes[9], _guidBytes[10], _guidBytes[11], _guidBytes[12], _guidBytes[13], _guidBytes[14], _guidBytes[15]);
        }
    }
}
