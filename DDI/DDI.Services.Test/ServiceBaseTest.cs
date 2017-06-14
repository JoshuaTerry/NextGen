using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business;
using DDI.Business.Core;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Services.Test
{
    /// <summary>
    /// DI Container tests for creating Services.
    /// </summary>
    [TestClass]
    public class ServiceBaseTest : TestBase
    {

        private const string TESTDESCR = "Service | Core";

        [TestInitialize]
        public void Initialize()
        {
            Factory.ConfigureForTesting();
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void FieldListHasProperties_Expressions()
        {
            var service = new ServiceBase<Constituent>(null);

            Assert.IsFalse(service.FieldListHasProperties(null, p => p.AgeFrom), "Null field list should return false.");
            Assert.IsFalse(service.FieldListHasProperties(string.Empty, p => p.AgeFrom), "Empty field list should return false.");
            Assert.IsTrue(service.FieldListHasProperties(FieldLists.AllFields, p => p.AgeFrom), "'All' should return true.");

            Assert.IsTrue(service.FieldListHasProperties("AgeFrom,AgeTo", p => p.Id, p => p.AgeFrom), "Property in field list.");
            Assert.IsFalse(service.FieldListHasProperties("BirthYear,AgeTo", p => p.Id, p => p.AgeFrom), "Property not in field list.");
            Assert.IsTrue(service.FieldListHasProperties("id", p => p.Id), "Case is ignored.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void FieldListHasProperties_Strings()
        {
            var service = new ServiceBase<Constituent>(null);
            string[] properties = { "Id", "AgeFrom" };

            Assert.IsFalse(service.FieldListHasProperties(null, properties), "Null field list should return false.");
            Assert.IsFalse(service.FieldListHasProperties(string.Empty, properties), "Empty field list should return false.");
            Assert.IsTrue(service.FieldListHasProperties(FieldLists.AllFields, properties), "'All' should return true.");

            Assert.IsTrue(service.FieldListHasProperties("AgeFrom,AgeTo", properties), "Property in field list.");
            Assert.IsFalse(service.FieldListHasProperties("BirthYear,AgeTo", properties), "Property not in field list.");
            Assert.IsTrue(service.FieldListHasProperties("id", properties), "Case is ignored.");
        }

    }
}
