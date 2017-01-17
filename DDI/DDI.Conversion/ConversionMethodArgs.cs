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
    public class ConversionMethodArgs
    {
        public int MethodNum { get; set; }
        public int MinCount { get; private set; }
        public int MaxCount { get; private set; }
        public bool AddOnly { get; private set; }
        public bool Skip { get; private set; }

        public ConversionMethodArgs(object methodNum) : this(methodNum, 0, int.MaxValue, false, false) { }

        public ConversionMethodArgs(object methodNum, int minCount, int maxCount) : this(methodNum, minCount, maxCount, false, false) { }

        public ConversionMethodArgs(object methodNum, int minCount, int maxCount, bool addOnly) : this(methodNum, minCount, maxCount, addOnly, false) { }

        public ConversionMethodArgs(object methodNum, int minCount, int maxCount, bool addOnly, bool skip)
        {
            if (maxCount == 0)
            {
                maxCount = int.MaxValue;
            }

            MethodNum = (int)methodNum;
            MinCount = minCount;
            MaxCount = maxCount;
            AddOnly = addOnly;
            Skip = skip;       
            
        }

    }
}
