using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Common;
using DDI.Business.Core;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DDI.Shared.Enums;
using DDI.Business.Tests.Core.DataSources;
using DDI.Shared.Enums.Core;
using DDI.Shared.Helpers;
using DDI.Business.Tests.GL.DataSources;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Enums.GL;
using DDI.Business.GL;

namespace DDI.Business.Tests.Core
{
    [TestClass]
    public class TransactionLogicTest : TestBase
    {

        private const string TESTDESCR = "Business | Core";
        private TransactionLogic _bl;
        private IList<Account> _accounts;
        private IList<Transaction> _transactions;
        private UnitOfWorkNoDb _uow;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

            _accounts = AccountDataSource.GetDataSource(_uow);
            _transactions = new List<Transaction>();
            _uow.SetRepository(new RepositoryNoDb<Transaction>(_transactions.AsQueryable()));

            _bl = _uow.GetBusinessLogic<TransactionLogic>();

        }



        [TestMethod, TestCategory(TESTDESCR)]
        public void TransactionLogic_SetAccount()
        {
            var tran = new Transaction();
            var assetAccount = _accounts.First(p => p.FiscalYear.Name == FiscalYearDataSource.OPEN_YEAR && p.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Category == AccountCategory.Asset);
            var fundAccount = _accounts.First(p => p.FiscalYear.Name == FiscalYearDataSource.OPEN_YEAR && p.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Category == AccountCategory.Fund);

            _bl.SetDebitAccount(tran, assetAccount);
            Assert.AreEqual(assetAccount.LedgerAccountYears.First(), tran.DebitAccount, "SetDebitAccount with account");
            Assert.AreEqual(assetAccount.LedgerAccountYears.First().Id, tran.DebitAccountId, "SetDebitAccount with account for Id");

            _bl.SetCreditAccount(tran, fundAccount);
            Assert.AreEqual(fundAccount.LedgerAccountYears.First(), tran.CreditAccount, "SetCreditAccount with account");
            Assert.AreEqual(fundAccount.LedgerAccountYears.First().Id, tran.CreditAccountId, "SetCreditAccount with account for Id");

            var accountLogic = _uow.GetBusinessLogic<AccountLogic>();

            tran.DebitAccount = null;
            tran.CreditAccount = null;

            AssertThrowsExceptionMessageContains<InvalidOperationException>(() => _bl.SetDebitAccount(tran, accountLogic.GetLedgerAccount(assetAccount)), "no fiscal year", 
                "SetDebitAccount with ledger account requires transaction to have a fiscal year.");

            tran.FiscalYear = assetAccount.FiscalYear;
            tran.FiscalYearId = tran.FiscalYear.Id;

            _bl.SetDebitAccount(tran, accountLogic.GetLedgerAccount(assetAccount));
            Assert.AreEqual(assetAccount.LedgerAccountYears.First(), tran.DebitAccount, "SetDebitAccount with ledger account");
            Assert.AreEqual(assetAccount.LedgerAccountYears.First().Id, tran.DebitAccountId, "SetDebitAccount with ledger account for Id");

            _bl.SetCreditAccount(tran, accountLogic.GetLedgerAccount(fundAccount));
            Assert.AreEqual(fundAccount.LedgerAccountYears.First(), tran.CreditAccount, "SetCreditAccount with ledger account");
            Assert.AreEqual(fundAccount.LedgerAccountYears.First().Id, tran.CreditAccountId, "SetCreditAccount with ledger account for Id");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void TransactionLogic_Swap_and_Negate()
        {
            // NOTE:  Testing Negate Also tests SwapGLAccounts

            var tran = new Transaction();
            var assetAccount = _accounts.First(p => p.FiscalYear.Name == FiscalYearDataSource.OPEN_YEAR && p.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Category == AccountCategory.Asset);
            var fundAccount = _accounts.First(p => p.FiscalYear.Name == FiscalYearDataSource.OPEN_YEAR && p.FiscalYear.Ledger.Code == BusinessUnitDataSource.UNIT_CODE1 && p.Category == AccountCategory.Fund);

            _bl.SetDebitAccount(tran, assetAccount);
            _bl.SetCreditAccount(tran, fundAccount);
            tran.Amount = 123.45m;

            _bl.Negate(tran);

            Assert.AreEqual(assetAccount.LedgerAccountYears.First(), tran.CreditAccount, "Negate set debit account to credit account.");
            Assert.AreEqual(assetAccount.LedgerAccountYears.First().Id, tran.CreditAccountId, "Negate set debit account id to credit account id.");
            Assert.AreEqual(fundAccount.LedgerAccountYears.First(), tran.DebitAccount, "Negate set credit account to debit account.");
            Assert.AreEqual(fundAccount.LedgerAccountYears.First().Id, tran.DebitAccountId, "Negate set credit account id to debit account id.");
            Assert.AreEqual(-123.45m, tran.Amount, "Negate reversed sign of transaction amount.");
        }
    }
}
