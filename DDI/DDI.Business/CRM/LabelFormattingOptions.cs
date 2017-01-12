using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Business.CRM
{
    /// <summary>
    /// Class for specifying label formatting options for NameFormatter.
    /// </summary>
    public class LabelFormattingOptions : NameFormattingOptions
    {
        public bool IsSpouse { get; set; }
        public bool IncludeInactive { get; set; }
        public string AddressType { get; set; }
        public string ContactName { get; set; }
        public int MaxLines { get; set; }
        public bool Caps { get; set; }
        public bool ExpandName { get; set; }
        public bool ExpandAddress { get; set; }
        public bool allowVacationAddress { get; set; }
        public AddressCategory AddressCategory { get; set; }
        public Guid AddressOid { get; set; }

        public LabelFormattingOptions() : base()
        {
            AddressCategory = AddressCategory.Mailing;
        }
    }
}
