using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Common;
using DDI.Business.Core;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DDI.Shared.Enums;
using DDI.Business.Tests.Core.DataSources;
using DDI.Shared.Enums.Core;
using DDI.Shared.Helpers;

namespace DDI.Business.Tests.Core
{
    [TestClass]
    public class EntityNumberLogicTest : TestBase
    {

        private const string TESTDESCR = "Business | Core";
        private IList<EntityNumber> _entityNumbers;
        private EntityNumberLogic _bl;

        private UnitOfWorkNoDb _uow;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();
            _entityNumbers = EntityNumberDataSource.GetDataSource(_uow);
            _bl = _uow.GetBusinessLogic<EntityNumberLogic>();
        }
       
        [TestMethod, TestCategory(TESTDESCR)]
        public void EntityNumberLogic_GetNextEntityNumber()
        {
            Assert.AreEqual(EntityNumberDataSource.STARTING_NEXT_NUMBER, _bl.GetNextEntityNumber(EntityNumberType.Constituent), "Starting number is correct.");
            Assert.AreEqual(EntityNumberDataSource.STARTING_NEXT_NUMBER + 1, _bl.GetNextEntityNumber(EntityNumberType.Constituent), "Nubmer increments correctly.");

            Guid id = EntityNumberDataSource.RangeIdForTest;
            Assert.AreEqual(EntityNumberDataSource.STARTING_NEXT_NUMBER, _bl.GetNextEntityNumber(EntityNumberType.Journal, id), "Starting number is correct for range id.");
            Assert.AreEqual(EntityNumberDataSource.STARTING_NEXT_NUMBER + 1, _bl.GetNextEntityNumber(EntityNumberType.Journal, id), "Number increments correctly for range.");
            
            id = GuidHelper.NewSequentialGuid();
            Assert.AreEqual(1, _bl.GetNextEntityNumber(EntityNumberType.Journal, id), "Starting number is 1 for different range id.");
            Assert.AreEqual(2, _bl.GetNextEntityNumber(EntityNumberType.Journal, id), "Number increments correctly for different range.");
            
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void EntityNumberLogic_SetNextEntityNumber()
        {
            _bl.SetNextEntityNumber(EntityNumberType.Constituent, 42);
            Assert.AreEqual(42, _bl.GetNextEntityNumber(EntityNumberType.Constituent), "Starting number was set correctly.");
            Assert.AreEqual(43, _bl.GetNextEntityNumber(EntityNumberType.Constituent), "Nubmer increments correctly.");

            Guid id = EntityNumberDataSource.RangeIdForTest;
            _bl.SetNextEntityNumber(EntityNumberType.Journal, 722, id);
            Assert.AreEqual(722, _bl.GetNextEntityNumber(EntityNumberType.Journal, id), "Starting number was set correctly for range id.");
            Assert.AreEqual(723, _bl.GetNextEntityNumber(EntityNumberType.Journal, id), "Number increments correctly for range.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void EntityNumberLogic_ReturnEntityNumber()
        {
            int number = _bl.GetNextEntityNumber(EntityNumberType.Constituent);
            _bl.ReturnEntityNumber(EntityNumberType.Constituent, number);
            Assert.AreEqual(number, _bl.GetNextEntityNumber(EntityNumberType.Constituent), "Starting number was returned correctly.");
            Assert.AreEqual(number + 1, _bl.GetNextEntityNumber(EntityNumberType.Constituent), "Nubmer increments correctly.");
        }

    }
}
