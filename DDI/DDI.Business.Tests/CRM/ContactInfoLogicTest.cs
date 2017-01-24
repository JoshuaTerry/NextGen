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
    public class ContactInfoLogicTest
    {
        private const string TESTDESCR = "Business | CRM";
        private UnitOfWorkNoDb _uow;
        private IList<Country> _countries;
        private ContactInfoLogic _bl;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

            _countries = CountryDataSource.GetDataSource(_uow);
            _bl = new ContactInfoLogic(_uow);

        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void ContactInfoLogic_ValidatePhoneNumber()
        {
            string phone;
            Country country = _countries.FirstOrDefault(p => p.ISOCode == "US");

            phone = "317-555-1234";
            Assert.IsTrue(_bl.ValidatePhoneNumber(ref phone, country), "10-digit US format valid");
            Assert.AreEqual("3175551234", phone, "10-digit US format returns raw digits");

            phone = "(317) 555-1234 x5678";
            Assert.IsTrue(_bl.ValidatePhoneNumber(ref phone, country), "10-digit with extension valid");
            Assert.AreEqual("3175551234 x5678", phone, "First 10 digits returned raw");

            phone = "555-1234";
            Assert.IsFalse(_bl.ValidatePhoneNumber(ref phone, country), "7 digit format not valid");
            Assert.AreEqual("555-1234", phone, "Invalid format returned as-is");

        }
    }
}
