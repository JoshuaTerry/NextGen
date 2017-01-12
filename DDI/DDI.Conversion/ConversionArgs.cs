using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Conversion
{
    /// <summary>
    /// Arguments for data conversion classes.
    /// </summary>
    public class ConversionArgs
    {
        public string Organization { get; private set; }
        public int MinCount { get; private set; }
        public int MaxCount { get; private set; }
        public string BaseDirectory { get; private set; }
        public bool AddOnly { get; private set; }

        public ConversionArgs(string organization, string baseDirectory) : this(organization, baseDirectory, 0, 0, false) { }

        public ConversionArgs(string organization, string baseDirectory, int minCount, int maxCount, bool addOnly)
        {
            Organization = organization;
            BaseDirectory = baseDirectory;
            MinCount = minCount;
            MaxCount = maxCount;
            AddOnly = addOnly;
        }
    }
}
