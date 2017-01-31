using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Business.Tests.Common.DataSources;
using DDI.Business.Tests.CRM.DataSources;
using DDI.Data;
using DDI.Shared.Models.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.CRM
{
    [TestClass]
    public class AddressLogicTest : TestBase
    {

        private const string TESTDESCR = "Business | CRM";
        private UnitOfWorkNoDb _uow;
        private IList<Country> _countries;
        private IList<State> _states;
        private AddressLogic _bl;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

            _countries = CountryDataSource.GetDataSource(_uow);
            _states = StateDataSource.GetDataSource(_uow);

            CRMConfigurationDataSource.GetDataSource(_uow);

            _bl = new AddressLogic(_uow);

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AddressLogic_ValidatePostalCode()
        {
            string rawResult, formattedResult;

            // US formatting

            Country country = _countries.FirstOrDefault(p => p.ISOCode == "US");

            Assert.IsTrue(_bl.ValidatePostalCode("46204", country, out rawResult, out formattedResult), "46204 is a valid US postal code");
            Assert.AreEqual("46204", rawResult);
            Assert.AreEqual("46204", formattedResult);

            Assert.IsTrue(_bl.ValidatePostalCode("462041234", country, out rawResult, out formattedResult), "462041234 is a valid US postal code");
            Assert.AreEqual("462041234", rawResult);
            Assert.AreEqual("46204-1234", formattedResult);

            Assert.IsTrue(_bl.ValidatePostalCode("46204-1234", country, out rawResult, out formattedResult), "46204-1234 is a valid US postal code");
            Assert.AreEqual("462041234", rawResult);
            Assert.AreEqual("46204-1234", formattedResult);

            Assert.IsFalse(_bl.ValidatePostalCode("1234", country, out rawResult, out formattedResult), "1234 is not a valid US postal code");
            Assert.IsFalse(_bl.ValidatePostalCode("4A204", country, out rawResult, out formattedResult), "4A204 is not a valid US postal code");

            // Canada formatting

            country = _countries.FirstOrDefault(p => p.ISOCode == "CA");

            Assert.IsTrue(_bl.ValidatePostalCode("C1D2E3", country, out rawResult, out formattedResult), "C1D2E3 is a valid Canadian postal code");
            Assert.AreEqual("C1D2E3", rawResult);
            Assert.AreEqual("C1D 2E3", formattedResult);

            Assert.IsTrue(_bl.ValidatePostalCode("C1D 2E3", country, out rawResult, out formattedResult), "C1D 2E3 is valid Canadian postal code");
            Assert.AreEqual("C1D2E3", rawResult);
            Assert.AreEqual("C1D 2E3", formattedResult);

            Assert.IsFalse(_bl.ValidatePostalCode("1C2D5X", country, out rawResult, out formattedResult), "1C2D5X is not a valid Canadian postal code");

            // Benin formatting

            country = _countries.FirstOrDefault(p => p.ISOCode == "BJ");

            Assert.IsTrue(_bl.ValidatePostalCode("123456", country, out rawResult, out formattedResult), "123456 is a valid Benin postal code");
            Assert.AreEqual("123456", rawResult);
            Assert.AreEqual("12 BP 3456", formattedResult);

            Assert.IsTrue(_bl.ValidatePostalCode("12 BP 3456", country, out rawResult, out formattedResult), "12 BP 3456 is a valid Benin postal code");
            Assert.AreEqual("123456", rawResult);
            Assert.AreEqual("12 BP 3456", formattedResult);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AddressLogic_FormatPostalCode()
        {
            // US formatting

            Country country = _countries.FirstOrDefault(p => p.ISOCode == "US");

            Assert.AreEqual("46204", _bl.FormatPostalCode("46204", country));
            Assert.AreEqual("46204-1234", _bl.FormatPostalCode("462041234", country));
            Assert.AreEqual("46204-1234", _bl.FormatPostalCode("46204-1234", country));
            Assert.AreEqual("46204-1234", _bl.FormatPostalCode("46204 1234", country));

            // Canadian Formatting
            country = _countries.FirstOrDefault(p => p.ISOCode == "CA");

            Assert.AreEqual("C1D 2E3", _bl.FormatPostalCode("C1D2E3", country));
            Assert.AreEqual("C1D 2E3", _bl.FormatPostalCode("C1D 2E3", country));

            // Benin Formatting
            country = _countries.FirstOrDefault(p => p.ISOCode == "BJ");

            Assert.AreEqual("12 BP 3456", _bl.FormatPostalCode("123456", country));
            Assert.AreEqual("12 BP 3456", _bl.FormatPostalCode("12 BP 3456", country));
            Assert.AreEqual("12 BP 3456", _bl.FormatPostalCode("12BP3456", country));

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AddressLogic_FormatCityStatePostalCode()
        {
            Country country = _countries.FirstOrDefault(p => p.ISOCode == "US");

            Assert.AreEqual("Kokomo, IN 46901", _bl.FormatCityStatePostalCode("Kokomo", "IN", "46901", country));

            country = _countries.FirstOrDefault(p => p.ISOCode == "CA");

            Assert.AreEqual("Toronto ON C1D 2E3\nCANADA", _bl.FormatCityStatePostalCode("Toronto", "on", "C1D2E3", country));

            country = _countries.FirstOrDefault(p => p.ISOCode == "BJ");

            Assert.AreEqual("12 BP 3456\nParakou\nBENIN", _bl.FormatCityStatePostalCode("Parakou", "", "123456", country));

            country = _countries.FirstOrDefault(p => p.ISOCode == "FR");

            Assert.AreEqual("FR-69005 Lyon\nFRANCE", _bl.FormatCityStatePostalCode("Lyon", "", "69005", country));

        }
    }
}