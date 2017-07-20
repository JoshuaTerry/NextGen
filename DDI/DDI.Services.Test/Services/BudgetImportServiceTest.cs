using DDI.Services.GL;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DDI.Services.Test.Services
{    
    [TestClass]
    public class BudgetImportServiceTest
    {
        private const string TESTDESCR = "Services | BudgetImport";
        private Guid badImportFileId = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6921");
        private Guid importFileId = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6922");
        private Guid fiscalYearId = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6923");
        private Guid ledgerAccountId = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6924");
        private Guid ledgerId = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6925");
        private Guid accountId = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6926");

        private Guid fixedBudgetId = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6927");
        private Guid workingBudgetId = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6928");
        private Guid whatIfBudgetId = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6929");

        private MappableEntityField accountColumn = new MappableEntityField() { ColumnName = "Account", PropertyName = "AccountNumber" };
        private MappableEntityField budgetColumn = new MappableEntityField() { ColumnName = "Budget", PropertyName = "Budget" };
        private MappableEntityField period1Column = new MappableEntityField() { ColumnName = "Period 1", PropertyName = "Amount01" };
        private string accountNumber = "01-305-45-60";


        [TestMethod, TestCategory(TESTDESCR)]
        public void ImportBudgets_OneBudgetImported()
        {
            var uow = Setup();
            var fields = new[] { accountColumn, budgetColumn, period1Column };
            var service = new BudgetImportService(uow);


            var response = service.ImportBudgets(importFileId, fiscalYearId, fields);

            Assert.AreEqual(true, response.IsSuccessful);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ImportBudgets_1Successful2Failures()
        {
            var uow = Setup();
            var fields = new[] { accountColumn, budgetColumn, period1Column };
            var service = new BudgetImportService(uow);

            var response = service.ImportBudgets(badImportFileId, fiscalYearId, fields);
            List<string> successes = (List<string>)response.Data.GetType().GetProperty("Imported").GetValue(response.Data);
            List<string> failures = (List<string>)response.Data.GetType().GetProperty("Failed").GetValue(response.Data); 
          
            Assert.AreEqual(2, failures.Count);
            Assert.AreEqual(1, successes.Count);
        }
        
        [TestMethod, TestCategory(TESTDESCR)]
        public void GetBugetNames_Returns3BudgetNames()
        {
            var uow = Setup();

            var service = new BudgetImportService(uow);
            (string fix, string working, string whatif) = service.GetBudgetNames(GetAccount());

            Assert.AreEqual("Eeny", fix);
            Assert.AreEqual("Meeny", working);
            Assert.AreEqual("Miny", whatif);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetAccountBudget_ReturnsValidBudgetForAlias()
        {
            var uow = Setup();

            var service = new BudgetImportService(uow);
            var result = service.GetAccountBudget(accountNumber, fiscalYearId, "Miny");
            Assert.IsNotNull(result);
        }

        private IUnitOfWork Setup()
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetById<ImportFile>(importFileId, It.IsAny<Expression<Func<ImportFile, object>>[]>())).Returns(GetImportFile());
            uow.Setup(m => m.GetById<ImportFile>(badImportFileId, It.IsAny<Expression<Func<ImportFile, object>>[]>())).Returns(GetBadImportFile());
            uow.Setup(m => m.GetEntities<Account>(It.IsAny<Expression<Func<Account, object>>[]>())).Returns(GetAccountQueryable());
            uow.Setup(m => m.GetEntities<AccountBudget>()).Returns(GetAccountBudgets().AsQueryable());
            uow.Setup(m => m.GetEntities<LedgerAccount>()).Returns(new List<LedgerAccount>(new[] { GetLedgerAccount() }).AsQueryable());
            uow.Setup(m => m.GetEntities<Ledger>()).Returns(new List<Ledger>(new[] { GetLedger() }).AsQueryable());

            return uow.Object;
        }
        private ImportFile GetImportFile()
        {
            var importFile = new ImportFile();
            importFile.Id = importFileId;
            var file = new FileStorage();

            var contents = new StringBuilder();
            contents.AppendLine("Business Unit,Fiscal Year,Account,Budget,Period 1,Period 2,Period 3,Period 4,Period 5,Period 6,Period 7,Period 8,Period 9,Period 10,Period 11,Period 12,Period 13");
            contents.AppendLine("DCEF,2016,01-305-45-60,Miny,12.54,45.78,-65.78,0.78,0,0,0,0,0,0,0,0,0");
            file.Data = Encoding.ASCII.GetBytes(contents.ToString());
            file.Extension = "csv";
            file.FileType = "csv";
            file.Name = "TestFile";

            importFile.File = file;

            return importFile;
        }

        private ImportFile GetBadImportFile()
        {
            var importFile = new ImportFile();
            importFile.Id = importFileId;
            var file = new FileStorage();

            var contents = new StringBuilder();
            contents.AppendLine("Business Unit,Fiscal Year,Account,Budget,Period 1,Period 2,Period 3,Period 4,Period 5,Period 6,Period 7,Period 8,Period 9,Period 10,Period 11,Period 12,Period 13");
            contents.AppendLine("DCEF,2016,01-305-45-60,Miny,12.54,45.78,-65.78,0.78,0,0,0,0,0,0,0,0,0");
            contents.AppendLine("DCEF,2016,01-305-45-61,Miny,12.54,45.78,-65.78,0.78,0,0,0,0,0,0,0,0,0");
            contents.AppendLine("DCEF,2016,01-305-45-60,Bad Name,12.54,45.78,-65.78,0.78,0,0,0,0,0,0,0,0,0");
            file.Data = Encoding.ASCII.GetBytes(contents.ToString());
            file.Extension = "csv";
            file.FileType = "csv";
            file.Name = "TestFile";

            importFile.File = file;

            return importFile;
        }
        private IQueryable<Account> GetAccountQueryable()
        {
            var account = GetAccount();
            var list = new List<Account>();
            list.Add(account);
            return list.AsQueryable();

        }
        private Account GetAccount()
        {
            var account = new Account();
            account.Id = accountId;
            account.AccountNumber = accountNumber;
            account.FiscalYearId = fiscalYearId;
            account.Budgets = GetAccountBudgets();

            return account;
        }

        private LedgerAccount GetLedgerAccount()
        {
            var ledgerAccount = new LedgerAccount();
            ledgerAccount.Id = ledgerAccountId;
            ledgerAccount.LedgerId = ledgerId;
            ledgerAccount.AccountNumber = accountNumber;

            return ledgerAccount;
        }

        private Ledger GetLedger()
        {
            var ledger = new Ledger();
            ledger.Id = ledgerId;
            ledger.FixedBudgetName = "Eeny";
            ledger.WorkingBudgetName = "Meeny";
            ledger.WhatIfBudgetName = "Miny";

            return ledger;
        }

        private List<AccountBudget> GetAccountBudgets()
        {
            var list = new List<AccountBudget>();
            list.Add(new AccountBudget() { Id = fixedBudgetId, AccountId = accountId, BudgetType = BudgetType.Fixed, Budget = new PeriodAmountList() });
            list.Add(new AccountBudget() { Id = workingBudgetId, AccountId = accountId, BudgetType = BudgetType.Working, Budget = new PeriodAmountList() });
            list.Add(new AccountBudget() { Id = whatIfBudgetId, AccountId = accountId, BudgetType = BudgetType.WhatIf, Budget = new PeriodAmountList() });

            return list;
        }
    }
}
