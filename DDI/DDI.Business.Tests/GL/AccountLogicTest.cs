﻿using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Business.Tests.GL.DataSources;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.GL
{
    [TestClass]
    public class AccountLogicTest : TestBase
    {
        private const string TESTDESCR = "Business | GL";
        private UnitOfWorkNoDb _uow;
        private AccountLogic _bl;
        private IList<BusinessUnit> _units;
        private IList<Ledger> _ledgers;
        private IList<FiscalYear> _fiscalYears;
        private IList<Segment> _segments;
        private IList<Account> _accounts;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

            _units = BusinessUnitDataSource.GetDataSource(_uow);
            _ledgers = LedgerDataSource.GetDataSource(_uow);
            _fiscalYears = FiscalYearDataSource.GetDataSource(_uow);
            _segments = SegmentDataSource.GetDataSource(_uow);
            _accounts = AccountDataSource.GetDataSource(_uow);

            _bl = new AccountLogic(_uow);
            //bool _ = _uow.GetBusinessLogic<BusinessUnitLogic>().IsMultiple; // Need to call this to establish the cached value via a UnitOfWork.
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void AccountLogic_GetPrefixedAccountNumber()
        {
            string unit = BusinessUnitDataSource.UNIT_CODE1;

            var year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == unit && p.Name == FiscalYearDataSource.OPEN_YEAR);
            var account = _accounts.First(p => p.FiscalYear == year && p.Category == AccountCategory.Asset);

            Assert.AreEqual($"{unit}:{account.AccountNumber}", _bl.GetPrefixedAccountNumber(account), "Add business unit prefix.");

            Assert.AreEqual($"{account.AccountNumber}", _bl.GetPrefixedAccountNumber(account, null), "Account: No prefix if unit ID is null.");

            Assert.AreEqual($"{account.AccountNumber}", _bl.GetPrefixedAccountNumber(account, year.LedgerId), "Account: No prefix if unit ID matches account ledger.");

            Assert.AreEqual($"{account.AccountNumber}", _bl.GetPrefixedAccountNumber(account, year.Ledger.BusinessUnitId), "Account: No prefix if unit ID matches account business unit.");

            Assert.AreEqual($"{unit}:{account.AccountNumber}", _bl.GetPrefixedAccountNumber(account,
                _units.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE2).Id), "Account: Add business unit prefix for other unit.");

            var ledgerAccount = account.LedgerAccountYears.FirstOrDefault().LedgerAccount;

            Assert.AreEqual($"{ledgerAccount.AccountNumber}", _bl.GetPrefixedAccountNumber(ledgerAccount, null), "Ledger Account: No prefix if unit ID is null.");

            Assert.AreEqual($"{ledgerAccount.AccountNumber}", _bl.GetPrefixedAccountNumber(ledgerAccount, year.LedgerId), "Ledger Account: No prefix if unit ID matches account ledger.");

            Assert.AreEqual($"{ledgerAccount.AccountNumber}", _bl.GetPrefixedAccountNumber(ledgerAccount, year.Ledger.BusinessUnitId), "Ledger Account: No prefix if unit ID matches account business unit.");

            Assert.AreEqual($"{unit}:{ledgerAccount.AccountNumber}", _bl.GetPrefixedAccountNumber(ledgerAccount,
                _units.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE2).Id), "Ledger Account: Add business unit prefix for other unit.");


        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AccountLogic_GetLedgerAccount()
        {
            string unit = BusinessUnitDataSource.UNIT_CODE1;

            var year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == unit && p.Name == FiscalYearDataSource.OPEN_YEAR);
            var ledger = year.Ledger;
            var account = _accounts.First(p => p.FiscalYear == year && p.AccountNumber == "01-100-10-10");
            var ledgerAccount = ledger.LedgerAccounts.FirstOrDefault(p => p.AccountNumber == account.AccountNumber);

            Assert.AreEqual(ledgerAccount, _bl.GetLedgerAccount(account));            
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AccountLogic_GetAccount()
        {
            string unit = BusinessUnitDataSource.UNIT_CODE1;

            var year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == unit && p.Name == FiscalYearDataSource.OPEN_YEAR);
            var ledger = year.Ledger;
            var account = _accounts.First(p => p.FiscalYear == year && p.AccountNumber == "01-100-10-10");
            var ledgerAccount = ledger.LedgerAccounts.FirstOrDefault(p => p.AccountNumber == account.AccountNumber);

            Assert.AreEqual(account, _bl.GetAccount(ledgerAccount, year.StartDate.Value), "Get account via ledger account and date");

            // Test using separate business unit
            unit = BusinessUnitDataSource.UNIT_CODE_SEPARATE;
            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == unit && p.Name == FiscalYearDataSource.OPEN_YEAR);
            ledger = year.Ledger;
            account = _accounts.First(p => p.FiscalYear == year && p.AccountNumber == "01-100-10-10");
            ledgerAccount = ledger.LedgerAccounts.FirstOrDefault(p => p.AccountNumber == account.AccountNumber);

            Assert.IsNull(_bl.GetAccount(ledgerAccount, null), "Returns null if year is null.");
            Assert.IsNull(_bl.GetAccount(null, year), "Returns null if ledger account is null.");

            Assert.AreEqual(account, _bl.GetAccount(ledgerAccount, year), "Account is in fiscal year provided.");
            var newYear = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == LedgerDataSource.NEW_LEDGER_CODE);
            account = _accounts.FirstOrDefault(p => p.FiscalYear == newYear && p.AccountNumber == "1001.CORP");
            ledgerAccount = newYear.Ledger.LedgerAccounts.FirstOrDefault(p => p.AccountNumber == account.AccountNumber);

            Assert.AreEqual(account, _bl.GetAccount(ledgerAccount, newYear, false), "Account in year provided");
            Assert.IsNull(_bl.GetAccount(ledgerAccount, year, false), "Account not in year provided, anyBusinessUnit = false");

            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.EMPTY_YEAR);
            Assert.AreEqual(account, _bl.GetAccount(ledgerAccount, year, true), "Account not in year provided, anyBusinessUnit = true");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AccountLogic_GetAccountNumber()
        {
            var year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.EMPTY_YEAR);
            var newYear = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == LedgerDataSource.NEW_LEDGER_CODE);
            var account = _accounts.FirstOrDefault(p => p.FiscalYear == newYear && p.AccountNumber == "1001.CORP");
            var ledgerAccount = newYear.Ledger.LedgerAccounts.FirstOrDefault(p => p.AccountNumber == account.AccountNumber);

            Assert.AreEqual("1001.CORP", _bl.GetAccountNumber(ledgerAccount, newYear), "Account in year provided.");
            Assert.AreEqual("XYZ:1001.CORP", _bl.GetAccountNumber(ledgerAccount, year), "Account not in year provided.");
            Assert.AreEqual("", _bl.GetAccountNumber(null, year), "Null ledger account returns empty string.");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AccountLogic_GetAccountDescription()
        {
            var year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.EMPTY_YEAR);
            var newYear = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == LedgerDataSource.NEW_LEDGER_CODE);
            var account = _accounts.FirstOrDefault(p => p.FiscalYear == newYear && p.AccountNumber == "1001.CORP");
            var ledgerAccount = newYear.Ledger.LedgerAccounts.FirstOrDefault(p => p.AccountNumber == account.AccountNumber);

            Assert.AreEqual(account.Name, _bl.GetAccountDescription(ledgerAccount, newYear), "Account in year provided.");
            Assert.AreEqual(account.Name, _bl.GetAccountDescription(ledgerAccount, year), "Account not in year provided.");
            Assert.AreEqual("", _bl.GetAccountDescription(null, year), "Null ledger account returns empty string.");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AccountLogic_CalculateAccountNumber()
        {
            string unit = BusinessUnitDataSource.UNIT_CODE1;

            var year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == unit && p.Name == FiscalYearDataSource.OPEN_YEAR);
            var ledger = year.Ledger;
            var account = _accounts.First(p => p.FiscalYear == year && p.AccountNumber == "01-100-10-10");
            var segmentLevel = ledger.SegmentLevels.FirstOrDefault(p => p.Level == 1);

            // Calculate account number for account
            string temp = segmentLevel.Separator;
            segmentLevel.Separator = "~";

            Assert.AreEqual("01~100-10-10", _bl.CalculateAccountNumber(account), "Uses updated segment separator.");

            segmentLevel.Separator = temp;

            var segment = account.AccountSegments.First(p => p.Level == 2).Segment;
            temp = segment.Code;
            segment.Code = "999";

            Assert.AreEqual("01-999-10-10", _bl.CalculateAccountNumber(account), "Uses updated segment code.");
            segment.Code = temp;

            // Calculate account number for list of segments
            IList<Segment> segments = account.AccountSegments.Select(p => p.Segment).OrderByDescending(p => p.Level).ToList();

            Assert.AreEqual("01-100-10-10", _bl.CalculateAccountNumber(ledger, segments), "Using list of segments in wrong order.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AccountLogic_CalculateSortKey()
        {
            string unit = BusinessUnitDataSource.UNIT_CODE1;

            var year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == unit && p.Name == FiscalYearDataSource.OPEN_YEAR);
            var ledger = year.Ledger;
            var account = _accounts.First(p => p.FiscalYear == year && p.AccountNumber == "01-150-50-42");

            var segmentLevels = _uow.GetBusinessLogic<LedgerLogic>().GetSegmentLevels(ledger.Id);
            foreach (var entry in segmentLevels)
            {
                entry.SortOrder = 0;
            }

            Assert.AreEqual("01 150 50 42", _bl.CalculateSortKey(account), "Using default order");

            segmentLevels[0].SortOrder = 1;
            segmentLevels[1].SortOrder = 4;
            segmentLevels[2].SortOrder = 3;
            segmentLevels[3].SortOrder = 2;

            Assert.AreEqual("01 42 50 150", _bl.CalculateSortKey(account), "Using default order");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AccountLogic_ValidateAccountNumber_FiscalYear()
        {
            string unit = BusinessUnitDataSource.UNIT_CODE1;
            var year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == unit && p.Name == FiscalYearDataSource.OPEN_YEAR);
            var ledger = year.Ledger;
            var account = _accounts.First(p => p.FiscalYear == year && p.AccountNumber == "01-150-50-42");

            // Basic tests for a valid account

            ValidatedAccount result = _bl.ValidateAccountNumber(year, "01-150-50-42", false, false, true);
            Assert.AreEqual(account, result.Account, "Returns correct account.");
            Assert.AreEqual(account.AccountNumber, result.AccountNumber, "Returns correct account number.");
            Assert.AreEqual(_bl.GetLedgerAccount(account), result.LedgerAccount, "Returns correct ledger account.");
            CollectionAssert.AreEquivalent(new string[] { "01", "150", "50", "42" }, result.SegmentCodes, "Returns each segment code correctly.");
            CollectionAssert.AreEquivalent(new string[] { "01", "150", "50", "42" }, result.Segments.Select(p => p.Code).ToList(), "Returns each segment correctly.");

            // Allow omission of separators
            result = _bl.ValidateAccountNumber(year, "011505042", false, false, true);
            Assert.AreEqual(account, result.Account, "Returns correct account when separators omitted.");
            Assert.AreEqual(account.AccountNumber, result.AccountNumber, "Returns correct account number when separators omitted.");

            // Business unit override
            result = _bl.ValidateAccountNumber(year, unit + ":01-150-50-42", false, false, true);
            Assert.AreEqual(account, result.Account, "Unit prefix where unit matches fiscal year.");

            string unit2 = BusinessUnitDataSource.UNIT_CODE2;
            AssertThrowsExceptionMessageContains<ValidationException>(() => result = _bl.ValidateAccountNumber(year, unit2 + ":01-150-50-42", false, false, true),
                UserMessagesGL.AccountMustBeInBusinessUnit, "Don't allow business unit override.");

            AssertNoException(() => result = _bl.ValidateAccountNumber(year, unit2 + ":01-150-50-42", true, false, true), "Allow business unit override.");
            Assert.IsNotNull(result.Account, "Business unit override returned account.");
            Assert.IsNotNull(result.ExplicitBusinessUnit, "Business unit override returned non-null explicit business unit.");
            Assert.AreEqual(unit2, result.ExplicitBusinessUnit.Code, "Business unit override returned correct explicit business unit.");

            // Validation of account in COA

            AssertThrowsExceptionMessageContains<ValidationException>(() => result = _bl.ValidateAccountNumber(year, "01-150-50-42-01", false, false, true),
                UserMessagesGL.GLAccountNumberInvalid, "Invalid account, with account validation enabled.");

            AssertNoException(() => _bl.ValidateAccountNumber(year, "01-150-50-42-01", false, false, false),
                "Invalid account, with account validation disabled.");

            // Allowing new G/L segments
            AssertThrowsExceptionMessageContains<ValidationException>(() => result = _bl.ValidateAccountNumber(year, "01-159-50-42", false, false, false),
                UserMessages.InvalidCode, "Invalid segment, with new segments disabled.");

            AssertNoException(() => result = _bl.ValidateAccountNumber(year, "01-159-50-42", false, true, false),
                 "Invalid segment, with new segments enabled.");

            Assert.AreEqual("159", result.SegmentCodes[1], "New segment code is returned.");

            // Segment code validations -- using new ledger.

            year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == LedgerDataSource.NEW_LEDGER_CODE);

            AssertThrowsExceptionMessageContains<ValidationException>(() => result = _bl.ValidateAccountNumber(year, "100X.CORP", false, false, false),
                UserMessagesGL.GLSegmentNumeric, "Numeric segment has letters.");

            AssertThrowsExceptionMessageContains<ValidationException>(() => result = _bl.ValidateAccountNumber(year, "1001.C0RP", false, false, false),
                UserMessagesGL.GLSegmentAlpha, "Alpha segment has digits.");

            AssertThrowsExceptionMessageContains<ValidationException>(() => result = _bl.ValidateAccountNumber(year, "100.CORP", false, false, false),
                UserMessagesGL.GLSegmentLength, "Segment length.");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AccountLogic_ValidateAccountNumber_Ledger()
        {
            string unit = BusinessUnitDataSource.UNIT_CODE1;
            var year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == unit && p.Name == FiscalYearDataSource.OPEN_YEAR);
            var ledger = year.Ledger;
            var account = _accounts.First(p => p.FiscalYear == year && p.AccountNumber == "01-150-50-42");

            // Basic tests for a valid account

            ValidatedAccount result = _bl.ValidateAccountNumber(ledger, "01-150-50-42", false, false, true);
            Assert.IsNull(result.Account, "Account returned as null.");
            Assert.AreEqual(account.AccountNumber, result.AccountNumber, "Returns correct account number.");
            Assert.AreEqual(_bl.GetLedgerAccount(account), result.LedgerAccount, "Returns correct ledger account.");
            CollectionAssert.AreEquivalent(new string[] { "01", "150", "50", "42" }, result.SegmentCodes, "Returns each segment code correctly.");
            Assert.AreEqual(0, result.Segments.Count, "Returns no segments.");

            // Business unit override
            result = _bl.ValidateAccountNumber(ledger, unit + ":01-150-50-42", false, false, true);
            Assert.AreEqual(_bl.GetLedgerAccount(account), result.LedgerAccount, "Unit prefix where unit matches fiscal year.");

            // G/L segments shouldn't be validated when passing a ledger.
            AssertNoException(() => result = _bl.ValidateAccountNumber(ledger, "01-159-50-42", false, false, false),
                "No validation of segments.");
        }

    }
}