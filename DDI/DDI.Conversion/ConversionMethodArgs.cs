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
        public string[] Filenames { get; set; }
        public bool AddOnly { get; private set; }
        public bool Skip { get; private set; }
        public int FileNum { get; set; }

        public ConversionMethodArgs(object methodNum) : this(methodNum, string.Empty, false, false) { }

        public ConversionMethodArgs(object methodNum, string filename) : this(methodNum, filename, false, false) { }

        public ConversionMethodArgs(object methodNum, string filename, bool addOnly) : this(methodNum, filename, addOnly, false) { }

        public ConversionMethodArgs(object methodNum, string filename, bool addOnly, bool skip)
        {
            MethodNum = (int)methodNum;
            AddOnly = addOnly;
            Skip = skip;

            if (filename == null)
            {
                filename = string.Empty;
            }

            Filenames = new string[] { filename };
            FileNum = 0;            
        }

    }
}
