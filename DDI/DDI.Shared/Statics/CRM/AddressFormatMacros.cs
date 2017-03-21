namespace DDI.Shared.Statics.CRM
{
    /// <summary>
    /// Address formatting macros, used in Country.AddressFormat
    /// </summary>
    public static class AddressFormatMacros
    {
        public const string City = "$CITY";
        public const string State = "$STATE";
        public const string StateCode = "$ST";
        public const string PostalCode = "$ZIP";
        public const string Country = "$COUNTRY";
        public const string ISOCode = "$CC";
        public const string Newline = "|";

        public const char AlphaSpecifier = 'A';
        public const char NumericSpecifier = '9';
        public const char AlphanumericSpecifier = 'X';
    }
}
