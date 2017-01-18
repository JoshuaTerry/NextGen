using System;
using DDI.Shared.Enums;
using DDI.Shared.Helpers;
using DDI.Shared.ModuleInfo;
using DDI.Shared.ModuleInfo.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test.Helpers
{
    [TestClass]
    public class ModuleHelperTests
    {
        private const string TESTDESCR = "Shared | Helpers";

        [TestMethod, TestCategory(TESTDESCR)]
        public void ModuleHelper_GetModuleInfo_ModuleType()
        {
            ModuleInfoBase acctModule = ModuleHelper.GetModuleInfo(Enums.ModuleType.Accounting);
            ModuleInfoBase glModule = ModuleHelper.GetModuleInfo(Enums.ModuleType.GeneralLedger);

            Assert.AreEqual("ACCT", acctModule.Code, "Ensure Accounting module is present and Code property is valid.");

           CollectionAssert.Contains(acctModule.ChildModules, glModule, "Accounting has child module of GL.");

           Assert.AreEqual(acctModule, glModule.ParentModule, "GL module has parent of Accounting.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ModuleHelper_GetModuleInfo_ClassType()
        {
            ModuleInfoBase crmModule = ModuleHelper.GetModuleInfo<CRM>();

            Assert.AreEqual(ModuleType.CRM, crmModule.ModuleType, "Ensure CRM module can be accessed via class type.");
        }
    }
}
