using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Business.Tests.Common.DataSources;
using DDI.Business.Tests.CRM.DataSources;
using DDI.Data;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.CRM
{
    [TestClass]
    public class AddressLogicTest
    {

        private const string TESTDESCR = "Business | CRM";
        private UnitOfWorkNoDb _uow;
        private IList<Country> _countries;
        private IList<State> _states;
        private AddressLogic _logic;
        private IList<Address> _addresses;
        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

            _countries = CountryDataSource.GetDataSource(_uow);
            _states = StateDataSource.GetDataSource(_uow);
            _addresses = AddressDataSource.GetDataSource(_uow);

            AbbreviationDataSource.GetDataSource(_uow);
            CRMConfigurationDataSource.GetDataSource(_uow);

            _logic = new AddressLogic(_uow);

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AddressLogic_ValidatePostalCode()
        {
            string rawResult, formattedResult;

            // US formatting

            Country country = _countries.FirstOrDefault(p => p.ISOCode == "US");

            Assert.IsTrue(_logic.ValidatePostalCode("46204", country, out rawResult, out formattedResult), "46204 is a valid US postal code");
            Assert.AreEqual("46204", rawResult);
            Assert.AreEqual("46204", formattedResult);

            Assert.IsTrue(_logic.ValidatePostalCode("462041234", country, out rawResult, out formattedResult), "462041234 is a valid US postal code");
            Assert.AreEqual("462041234", rawResult);
            Assert.AreEqual("46204-1234", formattedResult);

            Assert.IsTrue(_logic.ValidatePostalCode("46204-1234", country, out rawResult, out formattedResult), "46204-1234 is a valid US postal code");
            Assert.AreEqual("462041234", rawResult);
            Assert.AreEqual("46204-1234", formattedResult);

            Assert.IsFalse(_logic.ValidatePostalCode("1234", country, out rawResult, out formattedResult), "1234 is not a valid US postal code");
            Assert.IsFalse(_logic.ValidatePostalCode("4A204", country, out rawResult, out formattedResult), "4A204 is not a valid US postal code");

            // Canada formatting

            country = _countries.FirstOrDefault(p => p.ISOCode == "CA");

            Assert.IsTrue(_logic.ValidatePostalCode("C1D2E3", country, out rawResult, out formattedResult), "C1D2E3 is a valid Canadian postal code");
            Assert.AreEqual("C1D2E3", rawResult);
            Assert.AreEqual("C1D 2E3", formattedResult);

            Assert.IsTrue(_logic.ValidatePostalCode("C1D 2E3", country, out rawResult, out formattedResult), "C1D 2E3 is valid Canadian postal code");
            Assert.AreEqual("C1D2E3", rawResult);
            Assert.AreEqual("C1D 2E3", formattedResult);

            Assert.IsFalse(_logic.ValidatePostalCode("1C2D5X", country, out rawResult, out formattedResult), "1C2D5X is not a valid Canadian postal code");

            // Benin formatting

            country = _countries.FirstOrDefault(p => p.ISOCode == "BJ");

            Assert.IsTrue(_logic.ValidatePostalCode("123456", country, out rawResult, out formattedResult), "123456 is a valid Benin postal code");
            Assert.AreEqual("123456", rawResult);
            Assert.AreEqual("12 BP 3456", formattedResult);

            Assert.IsTrue(_logic.ValidatePostalCode("12 BP 3456", country, out rawResult, out formattedResult), "12 BP 3456 is a valid Benin postal code");
            Assert.AreEqual("123456", rawResult);
            Assert.AreEqual("12 BP 3456", formattedResult);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AddressLogic_FormatPostalCode()
        {
            // US formatting

            Country country = _countries.FirstOrDefault(p => p.ISOCode == "US");

            Assert.AreEqual("46204", _logic.FormatPostalCode("46204", country));
            Assert.AreEqual("46204-1234", _logic.FormatPostalCode("462041234", country));
            Assert.AreEqual("46204-1234", _logic.FormatPostalCode("46204-1234", country));
            Assert.AreEqual("46204-1234", _logic.FormatPostalCode("46204 1234", country));

            // Canadian Formatting
            country = _countries.FirstOrDefault(p => p.ISOCode == "CA");

            Assert.AreEqual("C1D 2E3", _logic.FormatPostalCode("C1D2E3", country));
            Assert.AreEqual("C1D 2E3", _logic.FormatPostalCode("C1D 2E3", country));

            // Benin Formatting
            country = _countries.FirstOrDefault(p => p.ISOCode == "BJ");

            Assert.AreEqual("12 BP 3456", _logic.FormatPostalCode("123456", country));
            Assert.AreEqual("12 BP 3456", _logic.FormatPostalCode("12 BP 3456", country));
            Assert.AreEqual("12 BP 3456", _logic.FormatPostalCode("12BP3456", country));

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AddressLogic_FormatCityStatePostalCode()
        {
            Country country = _countries.FirstOrDefault(p => p.ISOCode == "US");

            Assert.AreEqual("Kokomo, IN 46901", _logic.FormatCityStatePostalCode("Kokomo", "IN", "46901", country));

            country = _countries.FirstOrDefault(p => p.ISOCode == "CA");

            Assert.AreEqual("Toronto ON C1D 2E3\nCANADA", _logic.FormatCityStatePostalCode("Toronto", "on", "C1D2E3", country));

            country = _countries.FirstOrDefault(p => p.ISOCode == "BJ");

            Assert.AreEqual("12 BP 3456\nParakou\nBENIN", _logic.FormatCityStatePostalCode("Parakou", "", "123456", country));

            country = _countries.FirstOrDefault(p => p.ISOCode == "FR");

            Assert.AreEqual("FR-69005 Lyon\nFRANCE", _logic.FormatCityStatePostalCode("Lyon", "", "69005", country));

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AddressLogic_FormatAddress()
        {

            Assert.AreEqual("101 W. Ohio St.\nSuite 1650\nIndianapolis, IN 46204", _logic.FormatAddress(_addresses[0], false, false, 0), "Default formatting.");
            Assert.AreEqual("101 W OHIO ST\nSTE 1650\nINDIANAPOLIS, IN 46204", _logic.FormatAddress(_addresses[0], true, false, 0), "Format with caps option.");
            Assert.AreEqual("101 West Ohio Street\nSuite 1650\nIndianapolis, IN 46204", _logic.FormatAddress(_addresses[0], false, true, 0), "Format with expand option.");
            Assert.AreEqual("101 W Ohio St\nSte 1650\nIndianapolis, IN 46204", _logic.FormatAddress(_addresses[0], false, false, 8), "Format with max length option.");

        }
    }
}