using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DDI.Business.ModuleInfo;
using DDI.Business;
using DDI.Shared.Enums;
using DDI.Business.Tests;

namespace DDI.Shared.Test.ModuleInfo
{
    class ModuleInformationTests : TestBase
    {
        private const string TESTDESCR = "Business | ModuleInformation";

        [TestMethod, TestCategory(TESTDESCR)]
        public void ModuleInfoCreator_GetModuleInfo_Moduletype()
        {
            ModuleInformation acctModule = ModuleInfoCreator.GetModuleInfo(ModuleType.Accounting);
            ModuleInformation glModule = ModuleInfoCreator.GetModuleInfo(ModuleType.GeneralLedger);

            Assert.AreEqual(ModuleType.Accounting, acctModule.ModuleType, "Accounting module has module type of Accounting.");
            Assert.AreEqual(ModuleType.GeneralLedger, glModule.ParentModuleType, "GL module has parent module type of Accounting.");
        }
    }
}