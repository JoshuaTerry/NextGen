using System;
using DDI.Business.GL;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Business.Tests.GL.DataSources;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.GL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.GL
{
    [TestClass]
    public class BusinessUnitLogicTest : TestBase
    {
        private const string TESTDESCR = "Business | GL";
        private UnitOfWorkNoDb _uow;
        private BusinessUnitLogic _bl;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

            BusinessUnitDataSource.GetDataSource(_uow);
           
            _bl = new BusinessUnitLogic(_uow);

        }

        [TestMethod]
        public void BusinessUnitLogic_ValidatingCodeAndNameTest()
        {
            BusinessUnit unit = new BusinessUnit() { Code = "MKM", Name = "Methodist Kare Ministries ", BusinessUnitType = Shared.Enums.GL.BusinessUnitType.Common };

            AssertNoException(() => _bl.Validate(unit), "Valid Business Unit Code example.");

            unit.Code = "M&KMinstries";
            AssertThrowsExceptionMessageContains<Exception>(() => _bl.Validate(unit), "Invalid Business Unit Code", UserMessagesGL.CodeAlphaNumericRequired);

            unit.Code = "";
            AssertThrowsExceptionMessageContains<Exception>(() => _bl.Validate(unit), "Invalid Business Unit Code", UserMessagesGL.CodeIsRequired);

            unit.Code = "123456789";
            AssertThrowsExceptionMessageContains<Exception>(() => _bl.Validate(unit), "Invalid Business Unit Code", UserMessagesGL.CodeMaxLengthError);

            unit.Name = "";
            unit.Code = "MKM";
            AssertThrowsExceptionMessageContains<Exception>(() => _bl.Validate(unit), "Invalid Business Unit Name", UserMessagesGL.NameIsRequired);     
        }
    }
}
