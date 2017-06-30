using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDI.Business.Core;
using DDI.Business.GL;
using DDI.Business.Tests.GL.DataSources;
using DDI.Shared;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;
using DDI.Shared.Extensions;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.GL
{
    [TestClass]
    public class JournalLogicTest : TestBase
    {
        private const string TESTDESCR = "Business | GL";
        private IUnitOfWork _uow;
        private JournalLogic _bl;
        private IList<BusinessUnit> _businessUnits;
        private IList<FiscalYear> _fiscalYears;
        private IList<Fund> _funds;
        private IList<Transaction> _trans;
        private IList<EntityTransaction> _entityTrans;
        private IQueryable<LedgerAccount> _ledgerAccounts;

        [TestInitialize]
        public void Initialize()
        {
            Factory.ConfigureForTesting();

            _uow = Factory.CreateUnitOfWork();

            _businessUnits = BusinessUnitDataSource.GetDataSource(_uow);
            LedgerDataSource.GetDataSource(_uow);
            _fiscalYears = FiscalYearDataSource.GetDataSource(_uow);
            AccountDataSource.GetDataSource(_uow);
            _funds = FundDataSource.GetDataSource(_uow);

            _trans = new List<Transaction>();
            _uow.CreateRepositoryForDataSource(_trans);

            _entityTrans = new List<EntityTransaction>();
            _uow.CreateRepositoryForDataSource(_entityTrans);

            _bl = _uow.GetBusinessLogic<JournalLogic>();
            _ledgerAccounts = _uow.GetEntities<LedgerAccount>();
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void JournalLogic_CreateTransactions()
        {
            FiscalYear year = _fiscalYears.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Name == FiscalYearDataSource.OPEN_YEAR);
            var journalLines = new List<JournalLine>();

            Journal journal = new Journal()
            {
                Id = GuidHelper.NewSequentialGuid(),
                FiscalYear = year,
                Amount = 100.0m,
                BusinessUnit = year.Ledger.BusinessUnit,
                JournalNumber = 4,
                JournalType = JournalType.Normal,
                TransactionDate = DateTime.Parse("12/31/" + FiscalYearDataSource.OPEN_YEAR),
                JournalLines = journalLines                
            };

            JournalLine line = new JournalLine()
            {
                Journal = journal,
                LineNumber = 1,
                Amount = 100.0m,
                LedgerAccount = _ledgerAccounts.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.AccountNumber == "02-100-10-10"),
                DueToMode = DueToMode.DueFrom,
                SourceBusinessUnit = _businessUnits.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE2),
                SourceFund = _funds.FirstOrDefault(p => p.FiscalYear.Name == year.Name && p.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE2 && p.FundSegment.Code == "01"),
            };

            journalLines.Add(line);

            line = new JournalLine()
            {
                Journal = journal,
                LineNumber = 2,
                Amount = -100.0m,
                LedgerAccount = _ledgerAccounts.FirstOrDefault(p => p.Ledger.Code == BusinessUnitDataSource.UNIT_CODE2 && p.AccountNumber == "01-100-10-10"),
                DueToMode = DueToMode.DueTo,
                SourceBusinessUnit = _businessUnits.FirstOrDefault(p => p.Code == BusinessUnitDataSource.UNIT_CODE1),
                SourceFund = _funds.FirstOrDefault(p => p.FiscalYear.Name == year.Name && p.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.FundSegment.Code == "02"),
            };

            journalLines.Add(line);

            _uow.CreateRepositoryForDataSource(journalLines);
            _uow.CreateRepositoryForDataSource(new List<Journal>() { journal });
            _uow.CreateRepositoryForDataSource(new List<EntityApproval>());
            IList<Transaction> trans = _bl.CreateTransactions(journal);

            Assert.IsTrue(trans.Any(p => p.DebitAccount.LedgerAccount.AccountNumber == "02-100-10-10" &&
                                         p.DebitAccount.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 &&
                                         p.Amount == 100m), "Tran for line 1 was created.");

            Assert.IsTrue(trans.Any(p => p.DebitAccount.LedgerAccount.AccountNumber == "01-100-10-10" &&
                                         p.DebitAccount.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE2 &&
                                         p.Amount == -100m), "Tran for line 2 was created.");

            Assert.IsTrue(trans.Any(p => p.DebitAccount.LedgerAccount.AccountNumber == "01-150-50-50" &&
                                         p.DebitAccount.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 &&
                                         p.Amount == -100m), "Tran for line 1 due from other unit was created.");

            Assert.IsTrue(trans.Any(p => p.DebitAccount.LedgerAccount.AccountNumber == "02-150-50-40" &&
                                         p.DebitAccount.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 &&
                                         p.Amount == -100m), "Tran for line 1 due from fund 01 was created.");

            Assert.IsTrue(trans.Any(p => p.DebitAccount.LedgerAccount.AccountNumber == "01-150-50-43" &&
                                         p.DebitAccount.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 &&
                                         p.Amount == 100m), "Tran for line 1 due to fund 02 was created.");

            Assert.IsTrue(trans.Any(p => p.DebitAccount.LedgerAccount.AccountNumber == "01-150-50-51" &&
                                         p.DebitAccount.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE2 &&
                                         p.Amount == 100m), "Tran for line 2 due to other unit.");

            var tranLogic = _uow.GetBusinessLogic<TransactionLogic>();
            string result = tranLogic.ValidateTransactions(trans);
            Assert.IsNull(result, $"Transactions validation error: {result}");
        }


    }
}