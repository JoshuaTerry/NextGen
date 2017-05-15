namespace DDI.Shared.Statics.CRM
{
    public static class UserMessagesCRM
    {
        public static string NextSequenceValueTooManyTries => "Exceeded maximum number of tries to retreive NextSequenceValue";

        public static string ContactInfoBlank => "Contact information cannot be blank.";
        public static string ContactTypeMissing => "Contact type is not specified.";
        public static string ContactInfoNoParent => "Contact information has no parent.";
        public static string PhoneFormatNotValid => "Phone number format is not valid for constituent's country.";
        public static string EmailFormatNotValid => "Email address is not valid.";
    }
}
