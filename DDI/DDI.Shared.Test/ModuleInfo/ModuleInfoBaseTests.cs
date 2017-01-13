using System;
using DDI.Shared.Enums;
using DDI.Shared.Helpers;
using DDI.Shared.ModuleInfo;
using DDI.Shared.ModuleInfo.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test.ModuleInfo
{
    [TestClass]
    public class ModuleInfoBaseTests
    {
        private const string TESTDESCR = "Shared | ModuleInfo";

        [TestMethod, TestCategory(TESTDESCR)]
        public void ModuleHelper_GetModuleInfo_ModuleType()
        {
            ModuleInfoBase acctModule = ModuleHelper.GetModuleInfo(Enums.ModuleType.Accounting);
            ModuleInfoBase glModule = ModuleHelper.GetModuleInfo(Enums.ModuleType.GeneralLedger);

            Assert.AreEqual(ModuleType.Accounting, acctModule.ModuleType, "Accounting module has module type of Accounting.");
            Assert.AreEqual(ModuleType.Accounting, glModule.ParentModuleType, "GL module has parent module type of Accounting.");
        }


    }
}
