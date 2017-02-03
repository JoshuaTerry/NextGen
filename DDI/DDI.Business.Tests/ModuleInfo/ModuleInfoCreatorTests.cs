using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.ModuleInfo;
using DDI.Shared.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.ModuleInfo
{
    [TestClass]
    public class ModuleInfoCreatorTests : TestBase
    {
        #region Private Fields

        private const string TESTDESCR = "Business | ModuleInfo";
        private ModuleInformation acctModule;
        private ModuleInformation glModule;

        #endregion Private Fields

        #region Public Methods

        [TestInitialize()]
        public void InitializeTest()
        {
            this.acctModule = ModuleInfoCreator.GetModuleInfo(ModuleType.Accounting);
            this.glModule = ModuleInfoCreator.GetModuleInfo(ModuleType.GeneralLedger);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ModuleInfoCreator_GetModuleInfo_ModuleType_ChildHasCorrectParent()
        {
            Assert.AreEqual(acctModule.ModuleType, glModule.ParentModule.ModuleType, "GL module has parent of Accounting.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ModuleInfoCreator_GetModuleInfo_ModuleType_ParentContainsCorrectChildModules()
        {
            var generalLedgerParentModule = acctModule.ChildModules[3].ParentModuleType;
            // general ledger module in acctmodule child modules collection

            Assert.AreEqual(generalLedgerParentModule, glModule.ParentModuleType, "Accounting has child module of GL.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ModuleInfoCreator_GetModuleInfo_ModuleType_ReturnsCorrectCode()
        {
            Assert.AreEqual("ACCT", acctModule.Code, "Ensure Accounting module is present and Code property is valid.");
        }

        #endregion Public Methods
    }
}
