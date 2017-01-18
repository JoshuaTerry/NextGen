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
    public class ModuleInfoCreatorTests
    {


        private const string TESTDESCR = "Business | CRM | ModuleInfo";
        // I believe this will change to "Business | ModuleInfo"

        [TestMethod, TestCategory(TESTDESCR)]
        public void ModuleInfoCreator_GetModuleInfo_ModuleType()
        {
            ModuleInfoConcrete acctModule = ModuleInfoCreator.GetModuleInfo(ModuleType.Accounting); 
            ModuleInfoConcrete glModule = ModuleInfoCreator.GetModuleInfo(ModuleType.GeneralLedger);

            var generalLedgerParentModule = acctModule.ChildModules[3].ParentModuleType;
            // general ledger module in acctmodule child modules collection

            Assert.AreEqual(generalLedgerParentModule, glModule.ParentModuleType, "Accounting has child module of GL.");

            Assert.AreEqual("ACCT", acctModule.Code, "Ensure Accounting module is present and Code property is valid.");

            Assert.AreEqual(acctModule.ModuleType, glModule.ParentModule.ModuleType, "GL module has parent of Accounting.");
        }
        
    }
}
