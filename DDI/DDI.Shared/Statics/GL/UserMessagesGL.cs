namespace DDI.Shared.Statics.GL
{
    public static class UserMessagesGL
    {
        public static string NameIsRequired => "The {0} name is required";
        public static string CodeIsRequired => "The {0} code is required.";
        public static string IsRequired => "{0} is required.";
        public static string CodeMaxLengthError => "The {0} code cannot exceed than {1} characters.";
        public static string CodeAlphaNumericRequired => "The {0} code can only contain letters and numbers";
        public static string CodeIsNotUnique => "The {0} code must be unique.";
        public static string BusinessUnitTypeNotEditable => "The Business Unit Type is not editable.";
        public static string FiscalPeriodClosed => "Fiscal year {0} has been closed.";
        public static string FiscalYearClosed => "Fiscal year {0} has been closed.";
        public static string TranDateInvalid => "A valid transaction date is required.";
        public static string GLAcctDescrBlank => "G/L account description cannot be blank.";
        public static string GLAcctNumBlank => "G/L account number cannot be blank.";
        public static string GLAcctCategoryNone => "G/L account category cannot be set to \"None\".";
        public static string GLAcctBeginBalRandE => "Revenue and expense accounts cannot have a beginning balance.";
        public static string AccountGroupLevelsRange => "The number of account groups must be between 1 and 4.";
        public static string AccountGroupLevelsChanged => "The number of account groups cannot be modified if any G/L accounts have been defined for this ledger.";
        public static string AccountGroupLevelsUnique => "Account group titles must be unique.";
        public static string BudgetNamesUnique => "Budget names cannot be blank and must be unique.";

    }
}
