﻿using DDI.Shared.Enums.CRM;

namespace DDI.Business.CRM
{
    /// <summary>
    /// Class for specifying name formatting options for NameFormatter.
    /// </summary>
    public class SalutationFormattingOptions : NameFormattingOptions
    {
        public SalutationType PreferredType { get; set; }
        public string CustomSalutation { get; set; }
        public bool ForcePreferredtype { get; set; }
        
        public SalutationFormattingOptions()
        {
            Recipient = LabelRecipient.Both;
            PreferredType = SalutationType.Default;
        }
    }
}
