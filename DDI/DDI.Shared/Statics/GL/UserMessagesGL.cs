namespace DDI.Shared.Statics.GL
{
    public static class UserMessagesGL
    {
        public static string BusinessUnitTypeNotEditable => "The Business Unit Type is not editable.";
        public static string GLAcctDescrBlank => "G/L account description cannot be blank.";
        public static string GLAcctNumBlank => "G/L account number cannot be blank.";
        public static string GLAcctCategoryNone => "G/L account category cannot be set to \"None\".";
        public static string GLAcctBeginBalRandE => "Revenue and expense accounts cannot have a beginning balance.";
        public static string AccountGroupLevelsRange => $"The number of account groups must be between 1 and {ConstantsGL.MaxAccountGroups}.";
        public static string AccountGroupLevelsChanged => "The number of account groups cannot be modified if any G/L accounts have been defined for this ledger.";
        public static string AccountSegmentsRange => $"The number of G/L account segments must be between 1 and {ConstantsGL.MaxAccountSegments}.";
        public static string SegmentLevelsChanged => "The number of segment levels cannot be modified if any G/L account segments have been defined for this ledger.";
        public static string BadBusinessUnitCode => "Invalid {0} \"{1}\".";
        public static string AccountMustBeInBusinessUnit => "G/L account must be in {0} \"{1}\".";
        public static string BadFiscalYearForBusinessUnit => "Fiscal year {0} not defined for {1} \"{2}\".";
        public static string GLSegmentAlpha => "Segment code must contain only letters.";
        public static string GLSegmentAlphaNumeric => "Segment code must contain only letters and numbers.";
        public static string GLSegmentLength => "Segment code must be {0} characters long.";
        public static string GLSegmentNumeric => "Segment code must contain only numbers.";
        public static string GLAccountNumberInvalid => "Invalid G/L account \"{0}\".";

        public static string SegmentLevelsNotEditable => "Segment level settings cannot be modified once accounts have been defined.";
        public static string SegmentLevelMissing => "Segment level {0} is not defined.";
        public static string SegmentLevelDuplicate => "Segment level {0} appears more than once.";
        public static string SegmentLevelOneLinked => "The first segment level cannot be linked.";
        public static string SegmentLevelFundOne => "When fund accounting is enabled, there must be exactly one segment level of type \"Fund\".";
        public static string SegmentLevelFundZero => "A segment level can not be of type \"Fund\" unless fund accounting is enabled.";

        public static string FiscalYearNotEditable => "Fiscal year settings cannot be modified once transactions have been posted.";
        public static string FiscalPeriodClosed => "Fiscal year {0} has been closed.";
        public static string FiscalYearClosed => "Fiscal year {0} has been closed.";
        public static string FiscalPeriodsRange => $"The number of fiscal periods must be between 1 and {ConstantsGL.MaxFiscalPeriods}.";
        public static string CurrentPeriodRange => "The current period number must be between 1 and {0}.";
        public static string FiscalYearDatesInvalid => "Fiscal year date range is invalid.";

        public static string FiscalPeriodMissing => "Fiscal period {0} not defined for fiscal year {1}.";
        public static string FiscalPeriodDuplicate => "Fiscal period {0} appears more than once in fiscal year {1}.";
        public static string FiscalPeriodDatesInvalid => "Fiscal period {0} in fiscal year {1} has an invalid date range.";
        public static string AdjustmentPeriodNotLast => "Adjustment period for fiscal year {0} must be the last period.";
        public static string AdjustmentPeriodDates => "Adjustment period for fiscal year {0} must start and end on {1}.";
        public static string FiscalPeriodStartDate => "Period {0} in fiscal year {1} must start on {2}.";

        public static string NewJournalNoFiscalYear => "A valid fiscal year must be specified when creating a new one-time journal.";
        public static string NewJournalNoBusinessUnit => "A valid business unit must be specified when creating a recurring or template journal.";
    }
}
