using DDI.Business.CRM;
using DDI.Business.CRM.ModuleInfo.Base;
using DDI.Shared.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Business.Tests.ModuleInfo
{
    [TestClass]
    class ModuleCreatorTests
    {
        // TODO: Add tests from ModuleHelperTests

        // TODO: Test functionality that ModuleHelperTests doesn't cover

        private const string TESTDESCR = "Business | CRM | ModuleInfo";
        // I believe this will change to "Business | ModuleInfo"

        [TestMethod, TestCategory(TESTDESCR)]
        public void ModuleInfoCreator_GetModuleInfo_ModuleType()
        {
            // This tests a bunch of functionality, split it up
            ModuleInfoConcrete accountingModule = ModuleInfoCreator.GetModuleInfo(ModuleType.Accounting); // make sure this works properly
            ModuleInfoConcrete generalLedgerModule = ModuleInfoCreator.GetModuleInfo(ModuleType.GeneralLedger);

            Assert.AreEqual("ACCT", accountingModule.Code, "Ensure Accounting module is present and Code property is valid.");

            Assert.AreEqual(accountingModule, generalLedgerModule.ParentModule, "GL module has parent of Accounting.");
        }
        
    }
}
