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
        public static string EducationNoConstituent => "Constitent for education is not defined.";
        public static string ContactTypeCantDelete => "This contact type cannot be deleted.";
        public static string BirthDateBadMonth => "Birth date month must be between 1 and 12.";
        public static string BirthDateBadDay => "Birth date day must be between 1 and 31.";
        public static string BirthDateBadYear => "Birth date year is not valid.";
        public static string ConstituentNumberExists => "A constituent with constituent number {0} already exists.";
    }
}
