using DDI.Business.CRM;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using DDI.Shared;

namespace DDI.Business.Tests
{
    [TestClass]
    public class RegionLogicTests
    {
        private const string TESTDESCR = "Business | Logic";

        [TestMethod, TestCategory(TESTDESCR)]
        public void CompareRegionAreaAgainstZipCode_ReturnsTrue()
        {
            /*  Tests can be added when recent Business Logic tests have been merged to develop.
             *  These tests already have builders to create countries, states, etc. for UnitOfWorkNoDb.
             
            var uow = new Mock<IUnitOfWork>();
            var logic = new RegionLogic(uow.Object);

            RegionArea ra = new RegionArea();
            ra.PostalCodeHigh = "33333";
            ra.PostalCodeLow = "11111";

            var result = logic.CompareRegionAreaAgainstZipCode(ra, "22222");
            
            Assert.AreEqual(result, true);
            */
        }
    }
}
