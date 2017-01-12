using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Business.CRM
{
    /// <summary>
    /// Class for specifying name formatting options for NameFormatter.
    /// </summary>
    public class NameFormattingOptions
    {
        public bool KeepSeparate { get; set; }
        public int MaxChars { get; set; }
        public bool OmitPrefix { get; set; }
        public bool AddFirstNames { get; set; }
        public LabelRecipient Recipient { get; set; }
        public NameFormattingOptions()
        {
            Recipient = LabelRecipient.Primary;
        }
    }
}
