using System;
using System.Linq;
using DDI.Business.GL;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared;
using DDI.Business.Tests.GL.DataSources;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.GL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DDI.Shared.Helpers;
using System.Collections.Generic;
using DDI.Shared.Caching;
using DDI.Shared.Enums.GL;

namespace DDI.Business.Tests.GL
{
    [TestClass]
    public class LedgerLogicTest : TestBase
    {
        private const string TESTDESCR = "Business | GL";
        private UnitOfWorkNoDb _uow;
        private LedgerLogic _bl;
        private IList<Ledger> _ledgers;
        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

            _ledgers = LedgerDataSource.GetDataSource(_uow);
           
            _bl = new LedgerLogic(_uow);
        }

       
        [TestMethod, TestCategory(TESTDESCR)]
        public void LedgerDataSource_GetCachedLedger()
        {
            Guid id = _ledgers.First(p => p.Code == "DCEF").Id;

            Assert.AreEqual(id, _bl.GetCachedLedger(id)?.Id, "GetCachedLedger retreives ledger");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void LedgerDataSource_GetBudgetName()
        {
            Ledger ledger =_ledgers.First(p => p.Code == "DCEF");

            Assert.AreEqual(ledger.FixedBudgetName, _bl.GetBudgetName(ledger, BudgetType.Fixed), "Fixed budget name is returned.");
            Assert.AreEqual(ledger.WorkingBudgetName, _bl.GetBudgetName(ledger, BudgetType.Working), "Fixed budget name is returned.");
            Assert.AreEqual(ledger.WhatIfBudgetName, _bl.GetBudgetName(ledger, BudgetType.WhatIf), "Fixed budget name is returned.");
        }
    }
}
