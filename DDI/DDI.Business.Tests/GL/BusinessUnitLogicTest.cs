using System;
using DDI.Business.GL;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.GL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.GL
{
    [TestClass]
    public class BusinessUnitLogicTest
    {
        private const string TESTDESCR = "Business | GL";
        private UnitOfWorkNoDb _uow;
        private BusinessUnitLogic _bl;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();
            _bl = new BusinessUnitLogic(_uow);

        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
