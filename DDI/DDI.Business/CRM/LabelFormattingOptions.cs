﻿namespace DDI.Business.CRM
{
    /// <summary>
    /// Class for specifying label formatting options for NameFormatter.
    /// </summary>
    public class LabelFormattingOptions : NameFormattingOptions
    {
        public string AddressTypeCode { get; set; }
        public string ContactName { get; set; }
        public int MaxLines { get; set; }
        public bool Caps { get; set; }
        public bool ExpandName { get; set; }
        public bool ExpandAddress { get; set; }
        public bool AllowVacationAddress { get; set; }
        public AddressCategory AddressCategory { get; set; }        

        public LabelFormattingOptions() : base()
        {
            AddressCategory = AddressCategory.Mailing;
        }
    }
}
