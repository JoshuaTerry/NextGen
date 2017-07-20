namespace DDI.Shared.Statics.GL
{
    public static class UserMessagesGL
    {
        public static string BusinessUnitTypeNotEditable => "The Business Unit Type is not editable.";
        public static string GLAcctDescrBlank => "GL account description cannot be blank.";
        public static string GLAcctNumBlank => "GL account number cannot be blank.";
        public static string GLAcctCategoryNone => "GL account category cannot be set to \"None\".";
        public static string GLAcctBeginBalRandE => "Revenue and expense accounts cannot have a beginning balance.";
        public static string GLAcctDuplicate => "GL account number is already assigned for this fiscal year.";
        public static string AcctMergeFundMismatch = "Accounts being muerged must be in the same fund.";

        public static string AccountGroupLevelsRange => $"The number of account groups must be between 1 and {ConstantsGL.MaxAccountGroups}.";
        public static string AccountGroupLevelsChanged => "The number of account groups cannot be modified if any GL accounts have been defined for this ledger.";
        public static string AccountGroupCodeBlank => "Group code cannot be blank.";
        public static string AccountGroupNameBlank => "Group name cannot be blank.";

        public static string AccountSegmentsRange => $"The number of GL account segments must be between 1 and {ConstantsGL.MaxAccountSegments}.";
        public static string SegmentLevelsChanged => "The number of segment levels cannot be modified if any GL account segments have been defined for this ledger.";
        public static string BadBusinessUnitCode => "Invalid {0} \"{1}\".";
        public static string AccountMustBeInBusinessUnit => "GL account must be in {0} \"{1}\".";
        public static string BadFiscalYearForBusinessUnit => "Fiscal year {0} not defined for {1} \"{2}\".";
        public static string BadFiscalYear => "Fiscal year not defined.";
        public static string GLSegmentAlpha => "Segment code must contain only letters.";
        public static string GLSegmentAlphaNumeric => "Segment code must contain only letters and numbers.";
        public static string GLSegmentLength => "Segment code must be {0} characters long.";
        public static string GLSegmentNumeric => "Segment code must contain only numbers.";
        public static string GLSegmentNameBlank => "Segment name cannot be blank.";
        public static string GLAccountNumberInvalid => "Invalid GL account \"{0}\".";
        public static string GLAccountSegmentInvalid => "Account segment is not specified or invalid.";
        public static string GLAccountNoSegments => "Account has no account segments.";
        public static string GLAccountNotInFiscalYear => "Account {0} not defined in fiscal year {1}.";

        public static string SegmentLevelsNotEditable => "Segment level settings cannot be modified once accounts have been defined.";
        public static string SegmentLevelMissing => "Segment level {0} is not defined.";
        public static string SegmentLevelDuplicate => "Segment level {0} appears more than once.";
        public static string SegmentLevelOneLinked => "The first segment level cannot be linked.";
        public static string SegmentLevelFundOne => "When fund accounting is enabled, there must be exactly one segment level of type \"Fund\".";
        public static string SegmentLevelFundZero => "A segment level can not be of type \"Fund\" unless fund accounting is enabled.";

        public static string FiscalYearNotEditable => "Fiscal year settings cannot be modified once transactions have been posted.";
        public static string FiscalPeriodClosed => "Fiscal period {0} in year {0} is closed.";
        public static string FiscalPeriodOpen => "Fiscal period {0} in year {0} is open.";
        public static string FiscalYearClosed => "Fiscal year {0} has been closed.";
        public static string FiscalYearOpen => "Fiscal year {0} is open.";
        public static string FiscalPeriodsRange => $"The number of fiscal periods must be between 1 and {ConstantsGL.MaxFiscalPeriods}.";
        public static string FiscalYearCantReopen => "Fiscal year {0} has no fiscal period that can be re-opened.";
        public static string FiscalYearCantReclose => "Fiscal year {0} cannot be re-closed:  No fiscal years defined after fiscal year {0}.";
        public static string CurrentPeriodRange => "The current period number must be between 1 and {0}.";
        public static string FiscalYearDatesInvalid => "Fiscal year date range is invalid.";
        public static string FiscalYearDuplicateName => "Fiscal year {0} already exists.";
        public static string FiscalYearDuplicateStartDate => "A fiscal year with a start date of {0} already exists.";
        public static string BadFiscalPeriod => "Invalid fiscal period.";
        public static string FiscalYearNoName => "Fiscal year name is required.";
        public static string FiscalYearPeriodsChanged => "The number of periods cannot be changed unless the fiscal year status is Empty.";
        public static string FiscalYearAdjustPeriodChanged => "The adjustment period cannot be changed unless the fiscal year status is Empty.";
        public static string FiscalYearDatesChanged => "Cannot change the start or end date of an existing fiscal year.";

        public static string FiscalPeriodMissing => "Fiscal period {0} not defined for fiscal year {1}.";
        public static string FiscalPeriodDuplicate => "Fiscal period {0} appears more than once in fiscal year {1}.";
        public static string FiscalPeriodDatesInvalid => "Fiscal period {0} in fiscal year {1} has an invalid date range.";
        public static string AdjustmentPeriodNotLast => "Adjustment period for fiscal year {0} must be the last period.";
        public static string AdjustmentPeriodDates => "Adjustment period for fiscal year {0} must start and end on {1}.";
        public static string FiscalPeriodStartDate => "Period {0} in fiscal year {1} must start on {2}.";
        public static string FiscalPeriodEndDate => "Period {0} in fiscal year {1} must end on {2}.";
        public static string NoFiscalPeriodForDate => "No fiscal period defined for {0} in fiscal year {1}.";
        public static string NoFiscalYearForDate => "No fiscal year defined for {0}.";
        public static string TranDateClosedPeriod => "Transaction date of {0} is for a closed fiscal period.";
        public static string FiscalPeriodNumberChanged => "Cannot change the period number of a fiscal year.";
        public static string FiscalPeriodNumberInvalid => "Fiscal period number must be between 1 and {0}.";

        public static string FundNoFiscalYear => "Fund has no fiscal year.";
        public static string FundNoFundSegment => "No fund segment specified for fund.";
        public static string FundSegmentWrongFiscalYear => "Fund {0} is not in the same fiscal year as its fund segment.";
        public static string FundFBAccountWrongLedger => "Fund's fund balance account is not in the same ledger as the fund.";
        public static string FundFBAccountWrongFund => "The fund balance account for Fund {0} belongs to a different fund.";
        public static string FundCRAccountWrongLedger => "Fund's closing revenue account is not in the same ledger as the fund.";
        public static string FundCRAccountWrongFund => "The closing revenue account for Fund {0} belongs to a different fund.";
        public static string FundCEAccountWrongLedger => "Fund's closing expense account is not in the same ledger as the fund.";
        public static string FundCEAccountWrongFund => "The closing expense account for Fund {0} belongs to a different fund.";

        public static string UnitFromToNoFiscalYear => "Business unit from/to has no fiscal year.";
        public static string UnitFromToWrongUnit => "Business unit from/to has a mismatch between its business unit and fiscal year.";
        public static string UnitFromToDFAccountWrongLedger => "Business unit from/to has a \"due from\" account in the wrong ledger.";
        public static string UnitFromToDTAccountWrongLedger => "Business unit from/to has a \"due to\" account in the wrong ledger.";
        public static string NoUnitFromTo => "No due to / due from accounts defined for business unit {0} and offsetting business unit {1}.";

        public static string FundFromToNoFundAccounting => "Fund from/to cannot be validated if fund accounting is disabled.";
        public static string FundFromToNoFiscalYear => "Fund unit from/to has no fiscal year.";
        public static string FundFromToWrongFiscalYear => "Fund unit from/to is not in the same fiscal year as its fund segment.";
        public static string FundFromToDFAccountWrongFund => "Fund from/to has a \"due from\" account in the wrong fund.";
        public static string FundFromToDTAccountWrongFund => "Fund from/to has a \"due to\" account in the wrong fund.";
        public static string NoFundFromTo => "No due to / due from accounts defined for fund {0} and offsetting fund {1}.";

        public static string NewJournalNoFiscalYear => "A valid fiscal year must be specified when creating a new one-time journal.";
        public static string NewJournalNoBusinessUnit => "A valid business unit must be specified when creating a recurring or template journal.";




    }
}
