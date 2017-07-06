using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Extensions
{
    public static class IoExtensions
    {
        /// <summary>
        /// Read all bytes from a stream and return a byte array.
        /// </summary>
        public static byte[] ReadAllBytes(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
