namespace DDI.Shared.Statics.GL
{
    public static class UserMessagesGL
    {
        public static string NameIsRequired => "The Business Unit Name is required";
        public static string CodeIsRequired => "The Business Unit Code is required.";
        public static string CodeMaxLengthError => "The Business Unit Code is greater than 8 characters.";
        public static string CodeAlphaNumericRequired => "The Business Unit Code can only contain Letters and Numbers";
        public static string CodeIsNotUnique => "The Business Unit Code must be unique.";
        public static string BusinessUnitTypeNotEditable => "The Business Unit Type is not editable.";
        public static string FiscalPeriodClosed => "Fiscal year {0} has been closed.";
        public static string FiscalYearClosed => "Fiscal year {0} has been closed.";
        public static string TranDateInvalid => "A valid transaction date is required.";
        public static string GLAcctDescrBlank => "G/L account description cannot be blank.";
        public static string GLAcctNumBlank => "G/L account number cannot be blank.";
        public static string GLAcctCategoryNone => "G/L account category cannot be set to \"None\".";
        public static string GLAcctBeginBalRandE => "Revenue and expense accounts cannot have a beginning balance.";


    }
}
