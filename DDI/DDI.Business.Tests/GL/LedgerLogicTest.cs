using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Business.Tests.GL.DataSources;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.GL
{    
    [TestClass]
    public class LedgerLogicTest : TestBase
    {
        private const string TESTDESCR = "Business | GL";
        private const string MODIFIED = "Modified";
        private IUnitOfWork _uow;
        private LedgerLogic _bl;
        private IList<BusinessUnit> _units;
        private IList<Ledger> _ledgers;
        private IList<FiscalYear> _fiscalYears;
        private ITestRepository<Ledger> _ledgerRepo;
        private ITestRepository<SegmentLevel> _segmentLevelRepo;

        [TestInitialize]
        public void Initialize()
        {
            Factory.ConfigureForTesting();

            _uow = Factory.CreateUnitOfWork();

            _units = BusinessUnitDataSource.GetDataSource(_uow);
            _ledgers = LedgerDataSource.GetDataSource(_uow);
            _fiscalYears = FiscalYearDataSource.GetDataSource(_uow);

            _bl = new LedgerLogic(_uow);

            _ledgerRepo = _uow.GetRepository<Ledger>() as ITestRepository<Ledger>;
            _segmentLevelRepo = _uow.GetRepository<SegmentLevel>() as ITestRepository<SegmentLevel>;
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void LedgerLogic_GetCachedLedger()
        {
            Guid id = _ledgers.First(p => p.Code == BusinessUnitDataSource.UNIT_CODE1).Id;

            Assert.AreEqual(id, _bl.GetCachedLedger(id)?.Id, "GetCachedLedger retreives ledger");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void LedgerLogic_GetBudgetName()
        {
            Ledger ledger = _ledgers.First(p => p.Code == BusinessUnitDataSource.UNIT_CODE1);

            Assert.AreEqual(ledger.FixedBudgetName, _bl.GetBudgetName(ledger, BudgetType.Fixed), "Fixed budget name is returned.");
            Assert.AreEqual(ledger.WorkingBudgetName, _bl.GetBudgetName(ledger, BudgetType.Working), "Fixed budget name is returned.");
            Assert.AreEqual(ledger.WhatIfBudgetName, _bl.GetBudgetName(ledger, BudgetType.WhatIf), "Fixed budget name is returned.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void LedgerLogic_GetCurrentLedger()
        {
            // Test using date
            DateTime targetDate = DateTime.Parse($"6/1/{FiscalYearDataSource.OPEN_YEAR}");

            BusinessUnit unit = _units.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE1);
            Assert.AreEqual(unit.Code, _bl.GetCurrentLedger(unit.Id, targetDate)?.Code, $"Current ledger for {BusinessUnitDataSource.UNIT_CODE1}");

            unit = _units.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE_SEPARATE);
            Assert.AreEqual(unit.Code, _bl.GetCurrentLedger(unit.Id, targetDate)?.Code, $"Current ledger for {BusinessUnitDataSource.UNIT_CODE_SEPARATE}");

            targetDate = targetDate.AddYears(1);
            Assert.AreEqual(LedgerDataSource.NEW_LEDGER_CODE, _bl.GetCurrentLedger(unit.Id, targetDate)?.Code, $"Current ledger for {BusinessUnitDataSource.UNIT_CODE_SEPARATE} in next year");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void LedgerLogic_GetMostRecentLedger()
        {
            BusinessUnit unit = _units.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE_SEPARATE);
            Assert.AreEqual(LedgerDataSource.NEW_LEDGER_CODE, _bl.GetMostRecentLedger(unit.Id)?.Code, $"Most recent ledger for {BusinessUnitDataSource.UNIT_CODE_SEPARATE}");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void LedgerLogic_GetSegmentLevels()
        {
            Ledger ledger = _ledgers.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE1);
            SegmentLevel[] levels = _bl.GetSegmentLevels(ledger);
            Assert.IsNotNull(levels, "Result via ledger is not null");
            Assert.AreEqual(5, levels.Length, $"Result via ledger is length 5");

            levels = _bl.GetSegmentLevels(ledger.Id);
            Assert.IsNotNull(levels, "Result via ledger Id is not null");
            Assert.AreEqual(5, levels.Length, $"Result via ledger Id is length 5");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void LedgerLogic_GetFundSegmentLevel()
        {
            Ledger ledger = _ledgers.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE1);
            Assert.AreEqual(1, _bl.GetFundSegmentLevel(ledger), "For common ledger, fund segment level is 1");

            ledger = _ledgers.FirstOrDefault(p => p.Code == LedgerDataSource.NEW_LEDGER_CODE);
            Assert.AreEqual(0, _bl.GetFundSegmentLevel(ledger), "For new ledger, fund segment level is 0");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void LedgerLogic_GetFundLabel()
        {
            Ledger ledger = _ledgers.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE1);
            Assert.AreEqual("Fund", _bl.GetFundLabel(ledger), "For common ledger, fund label is Fund");

            ledger = _ledgers.FirstOrDefault(p => p.Code == LedgerDataSource.NEW_LEDGER_CODE);
            Assert.AreEqual("", _bl.GetFundLabel(ledger), "For new ledger, fund label is blank");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void LedgerLogic_Validate()
        {
            AccountDataSource.GetDataSource(_uow); // ensure GL accounts are available.

            Ledger ledger = _ledgers.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE1);
            AssertNoException(() => _bl.Validate(ledger), "Valid ledger example");

            // Budget name blank exceptions

            string temp = ledger.FixedBudgetName;
            ledger.FixedBudgetName = string.Empty;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeNonBlank, "Blank fixed budget name.");
            ledger.FixedBudgetName = temp;

            temp = ledger.WorkingBudgetName;
            ledger.WorkingBudgetName = string.Empty;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeNonBlank, "Blank working budget name.");
            ledger.WorkingBudgetName = temp;

            temp = ledger.WhatIfBudgetName;
            ledger.WhatIfBudgetName = string.Empty;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeNonBlank, "Blank what if budget name.");
            ledger.WhatIfBudgetName = temp;

            // Budget name unique exceptions

            temp = ledger.FixedBudgetName;
            ledger.FixedBudgetName = ledger.WorkingBudgetName;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeUnique, "Fixed/Working budgets same.");
            ledger.FixedBudgetName = temp;

            temp = ledger.WorkingBudgetName;
            ledger.WorkingBudgetName = ledger.WhatIfBudgetName;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeUnique, "Working/What If budgets same.");
            ledger.WorkingBudgetName = temp;

            // Account group title blank exceptions

            int tempInt = ledger.AccountGroupLevels;
            ledger.AccountGroupLevels = 4;

            temp = ledger.AccountGroup1Title;
            ledger.AccountGroup1Title = string.Empty;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeNonBlank);
            ledger.AccountGroup1Title = temp;

            temp = ledger.AccountGroup2Title;
            ledger.AccountGroup2Title = string.Empty;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeNonBlank);
            ledger.AccountGroup2Title = temp;

            temp = ledger.AccountGroup3Title;
            ledger.AccountGroup3Title = string.Empty;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeNonBlank);
            ledger.AccountGroup3Title = temp;

            temp = ledger.AccountGroup4Title;
            ledger.AccountGroup4Title = string.Empty;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeNonBlank);
            ledger.AccountGroup4Title = temp;

            // Account group title unique exceptions

            temp = ledger.AccountGroup1Title;
            ledger.AccountGroup1Title = ledger.AccountGroup4Title;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeUnique);
            ledger.AccountGroup1Title = temp;

            temp = ledger.AccountGroup2Title;
            ledger.AccountGroup2Title = ledger.AccountGroup3Title;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeUnique);
            ledger.AccountGroup2Title = temp;

            ledger.AccountGroupLevels = tempInt;

            // Test modified propertites
            _ledgerRepo.ModifiedPropertyList = nameof(ledger.AccountGroupLevels);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.AccountGroupLevelsChanged);

            _ledgerRepo.ModifiedPropertyList = nameof(ledger.SegmentLevels);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelsChanged);

            // Test copying a property to other common ledgers.            
            temp = ledger.AccountGroup4Title;
            ledger.AccountGroup4Title = MODIFIED;
            _ledgerRepo.ModifiedPropertyList = nameof(ledger.AccountGroup4Title);
            _bl.Validate(ledger);
            foreach (var entry in _ledgers)
            {
                if (entry.BusinessUnit.BusinessUnitType == BusinessUnitType.Separate)
                {
                    Assert.AreNotEqual(MODIFIED, entry.AccountGroup4Title, "Separate ledger was not copied.");
                }
                else
                {
                    Assert.AreEqual(MODIFIED, entry.AccountGroup4Title, "Common Ledger data was copied.");
                }                
            }

            // Changing it back...
            ledger.AccountGroup4Title = temp;
            _bl.Validate(ledger);

            _ledgerRepo.ModifiedPropertyList = null;

            var allLevels = _ledgers.Where(p => p.BusinessUnit.BusinessUnitType != BusinessUnitType.Separate).SelectMany(p => p.SegmentLevels);

            // Test copying a segment level to other common ledgers
            SegmentLevel level = ledger.SegmentLevels.FirstOrDefault(p => p.Level == 2);
            temp = level.Abbreviation;
            level.Abbreviation = MODIFIED;
            _bl.Validate(ledger);

            foreach (var entry in allLevels.Where(p => p.Level == 2))
            {
                Assert.AreEqual(MODIFIED, entry.Abbreviation, "Segment level abbreviation modified for all common ledgers.");
            }

            // Changing it back...
            level.Abbreviation = temp;
            _bl.Validate(ledger);

            // Segment level properties modified for non-empty ledger.            
            _segmentLevelRepo.ModifiedPropertyList = nameof(level.Format);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelsNotEditable);

            _segmentLevelRepo.ModifiedPropertyList = nameof(level.IsCommon);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelsNotEditable);

            _segmentLevelRepo.ModifiedPropertyList = nameof(level.IsLinked);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelsNotEditable);

            _segmentLevelRepo.ModifiedPropertyList = nameof(level.Length);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelsNotEditable);

            _segmentLevelRepo.ModifiedPropertyList = nameof(level.Separator);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelsNotEditable);

            _segmentLevelRepo.ModifiedPropertyList = nameof(level.Type);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelsNotEditable);

            _segmentLevelRepo.ModifiedPropertyList = null;

            // Segment level missing / duplicate
            ledger.SegmentLevels.Remove(level);
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelMissing);
            ledger.SegmentLevels.Add(level);

            level.Level--;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelDuplicate);
            level.Level++;

            // First segment level linked
            level = ledger.SegmentLevels.FirstOrDefault(p => p.Level == 1);
            level.IsLinked = true;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelOneLinked);
            level.IsLinked = false;

            // Segment type = fund
            ledger.FundAccounting = false;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelFundZero);
            ledger.FundAccounting = true;

            level.Type = SegmentType.None;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelFundOne);
            level.Type = SegmentType.Fund;

            level = ledger.SegmentLevels.FirstOrDefault(p => p.Level == 2);
            level.Type = SegmentType.Fund;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessagesGL.SegmentLevelFundOne);
            level.Type = SegmentType.None;

            // Validate abbreviation
            temp = level.Abbreviation;
            level.Abbreviation = string.Empty;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeNonBlank);
            level.Abbreviation = ledger.SegmentLevels.First(p => p.Level == 1).Abbreviation;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeUnique);
            level.Abbreviation = temp;

            // Validate name
            temp = level.Name;
            level.Name = string.Empty;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeNonBlank);
            level.Name = ledger.SegmentLevels.First(p => p.Level == 1).Name;
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(ledger), UserMessages.MustBeUnique);
            level.Name = temp;

            // Tests for an empty ledger
            ledger = _ledgers.FirstOrDefault(p => p.Code == LedgerDataSource.NEW_LEDGER_CODE);

            // Segment level properties modified for non-empty ledger.            
            _segmentLevelRepo.ModifiedPropertyList = string.Join(",",
                new string[] { nameof(level.Format), nameof(level.IsCommon), nameof(level.IsLinked), nameof(level.Length),
                nameof(level.Separator), nameof(level.Type) });

            AssertNoException(() => _bl.Validate(ledger));

            _segmentLevelRepo.ModifiedPropertyList = null;

        }
    }
}