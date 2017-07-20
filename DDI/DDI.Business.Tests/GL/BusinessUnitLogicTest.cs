using System.Collections.Generic;
using DDI.Business.GL;
using DDI.Business.Tests.Core.DataSources;
using DDI.Business.Tests.GL.DataSources;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.GL
{
    [TestClass]
    public class BusinessUnitLogicTest : TestBase
    {
        private const string TESTDESCR = "Business | GL";
        private IUnitOfWork _uow;
        private BusinessUnitLogic _bl;

        [TestInitialize]
        public void Initialize()
        {
            Factory.ConfigureForTesting();
            _uow = Factory.CreateUnitOfWork();

            BusinessUnitDataSource.GetDataSource(_uow);
           
            _bl = new BusinessUnitLogic(_uow);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void BusinessUnitLogic_ValidatingCodeAndNameTest()
        {
            LedgerDataSource.GetDataSource(_uow);
            ConfigurationDataSource.GetDataSource(_uow);

            BusinessUnit unit = new BusinessUnit() { Code = "MKM", Name = "Methodist Kare Ministries ", BusinessUnitType = Shared.Enums.GL.BusinessUnitType.Separate };

            AssertNoException(() => _bl.Validate(unit), "Valid Business Unit Code example.");

            unit.Code = "M&KM";

            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(unit), UserMessages.CodeAlphaNumericRequired, UserMessages.CodeAlphaNumericRequired);

            unit.Code = "";
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(unit), UserMessages.CodeIsRequired, UserMessages.CodeIsRequired);

            unit.Code = "123456789";
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(unit), UserMessages.CodeMaxLengthError, UserMessages.CodeMaxLengthError);

            unit.Name = "";
            unit.Code = "MKM";
            AssertThrowsExceptionMessageContains<ValidationException>(() => _bl.Validate(unit), UserMessages.NameIsRequired, UserMessages.NameIsRequired);     
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void BusinessUnitLogic_IsMultiple()
        {
            Assert.AreEqual(true, _bl.IsMultiple, "Multiple business objects defined.");

            // Set up tests for a single business unit.
            using (var uow2 = Factory.CreateUnitOfWork())
            {
                var businessUnits = new List<BusinessUnit>();
                businessUnits.Add(new BusinessUnit() { Code = "*", Name = "Organizational Business Unit", BusinessUnitType = Shared.Enums.GL.BusinessUnitType.Organization, Id = GuidHelper.NewSequentialGuid() });
                uow2.CreateRepositoryForDataSource(businessUnits);

                var bl2 = new BusinessUnitLogic(uow2);
                Assert.AreEqual(true, bl2.IsMultiple, "IsMultiple uses cached value");
                CacheHelper.RemoveAllEntries();
                Assert.AreEqual(false, bl2.IsMultiple, "IsMultiple revaluated returns false for single business unit.");
                CacheHelper.RemoveAllEntries();
            }
        }
    }
}
