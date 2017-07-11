namespace DDI.Shared.Statics
{
    public static class UserMessages
    {
        public static string NameIsRequired => "The {0} name is required";
        public static string CodeIsRequired => "The {0} code is required.";
        public static string MustBeNonBlank => "{0} cannot be blank.";
        public static string MustBeUnique => "{0} must be unique.";
        public static string IsRequired => "{0} is required.";
        public static string CodeMaxLengthError => "The {0} code cannot exceed than {1} characters.";
        public static string CodeAlphaNumericRequired => "The {0} code can only contain letters and numbers";
        public static string CodeIsNotUnique => "The {0} code must be unique.";
        public static string InvalidCode => "Invalid {0} code {1}";
        public static string TranDateInvalid => "A valid transaction date is required.";
        public static string TranDateMissingForEntity => "{0} has no transaction date.";
        public static string EntityAlreadyPosted => "{0} has already been posted and cannot be updated.";
        public static string EntityAlreadyReversed => "{0} has already been reversed.";
        public static string TranNoFiscalYear => "Transaction {0} has no fiscal year defined.";
        public static string TranInvalidDate => "Transaction {0} has a missing or invalid transaction date.";

        public static string TranNoTranDate => "Transaction #{0} has no transaction date.";
        public static string TranCantGetFund => "Cannot determine fund for transaction #{0}.";
        public static string TranImbalance => "{0} is out of balance by {1:C2}.";
        public static string TranImbalanceForDate => "{0} is out of balance by {1:C2} on {2:d}.";
        public static string TranImbalanceForBU => "{0} is out of balance by {1:C2} for {2} on {3:d}.";
        public static string TranImbalanceForFund => "{0} is out of balance by {1:C2} for fund {2} on {3:d}.";
    }
}
