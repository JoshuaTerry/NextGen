using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Business.Tests.CRM.DataSources;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.CRM
{
    [TestClass]
    public class ConstituentAddressLogicTest
    {

        private const string TESTDESCR = "Business | CRM";
        private UnitOfWorkNoDb _uow;
        private IList<Constituent> _constituents;
        private IList<Address> _addresses;
        private IList<AddressType> _addressTypes;
        private IList<ConstituentAddress> _constituentAddresses;
        private ConstituentAddressLogic _logic;
        private ConstituentAddress _homeAddress, _workAddress, _mailingAddress, _vacationAddress;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

            _addresses = AddressDataSource.GetDataSource(_uow);
            _addressTypes = AddressTypeDataSource.GetDataSource(_uow);
            CRMConfigurationDataSource.GetDataSource(_uow);

            // Constituents

            _constituents = new List<Constituent>();
            _constituents.Add(new Constituent()
            {
                ConstituentNumber = 1,
                FirstName = "John",
                LastName = "Public",
                Id = GuidHelper.NextGuid()
            });

            _uow.CreateRepositoryForDataSource(_constituents);

            _constituentAddresses = new List<ConstituentAddress>();

            // First address is home
            _constituentAddresses.Add(_homeAddress = new ConstituentAddress()
            {
                Address = _addresses[0],
                Constituent = _constituents[0],
                AddressType = _addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Home),
                IsPrimary = true,
                ResidentType = ResidentType.Primary,
                Id = GuidHelper.NextGuid()
            });

            // Second address is work
            _constituentAddresses.Add(_workAddress = new ConstituentAddress()
            {
                Address = _addresses[1],
                Constituent = _constituents[0],
                AddressType = _addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Work),
                IsPrimary = false,
                ResidentType = ResidentType.Primary,
                Id = GuidHelper.NextGuid()
            });

            // Third address is mailing
            _constituentAddresses.Add(_mailingAddress = new ConstituentAddress()
            {
                Address = _addresses[2],
                Constituent = _constituents[0],
                AddressType = _addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Mailing),
                IsPrimary = false,
                ResidentType = ResidentType.Primary,
                Id = GuidHelper.NextGuid()
            });

            // Fourth address is vacation : 10/1 thru 3/31
            _constituentAddresses.Add(_vacationAddress = new ConstituentAddress()
            {
                Address = _addresses[3],
                Constituent = _constituents[0],
                AddressType = _addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Vacation),
                IsPrimary = false,
                StartDay = new DateTime(2015, 10, 1).DayOfYear,
                EndDay = new DateTime(2015, 3, 31).DayOfYear,
                ResidentType = ResidentType.Primary,
                Id = GuidHelper.NextGuid()
            });

            _constituents[0].ConstituentAddresses = _constituentAddresses.Where(p => p.Constituent == _constituents[0]).ToList();

            _logic = new ConstituentAddressLogic(_uow);

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ConstituentAddressLogic_IsCurrentAddress()
        {
            var testRow = _constituentAddresses[0];
            var testDate = new DateTime(2016, 6, 15);

            // No dates
            testRow.StartDate = testRow.EndDate = null;
            testRow.StartDay = testRow.EndDay = 0;
            Assert.IsTrue(_logic.IsCurrentAddress(testRow, testDate), "Current if no start or end dates specified.");

            // Start date
            testRow.StartDate = new DateTime(2016, 6, 1);
            testRow.EndDate = null;
            testRow.StartDay = testRow.EndDay = 0;
            Assert.IsTrue(_logic.IsCurrentAddress(testRow, testDate), "Current if no end date specified.");
            Assert.IsFalse(_logic.IsCurrentAddress(testRow, new DateTime(2016, 4, 1)), "Not current if prior to start date.");

            // End date
            testRow.StartDate = null;
            testRow.EndDate = new DateTime(2016, 7, 1);
            testRow.StartDay = testRow.EndDay = 0;
            Assert.IsTrue(_logic.IsCurrentAddress(testRow, testDate), "Current if no start date specified.");
            Assert.IsFalse(_logic.IsCurrentAddress(testRow, new DateTime(2016, 8, 1)), "Not current if after end date.");

            // Vacation dates: 3/1 thru 9/30
            testRow.StartDate = testRow.EndDate = null;
            testRow.StartDay = new DateTime(2015, 3, 1).DayOfYear;
            testRow.EndDay = new DateTime(2015, 9, 30).DayOfYear;
            Assert.IsTrue(_logic.IsCurrentAddress(testRow, testDate), "Current if between 3/1 and 9/30.");
            Assert.IsTrue(_logic.IsCurrentAddress(testRow, new DateTime(2015, 9, 30)), "9/30 current between 3/1 and 9/30.");
            Assert.IsTrue(_logic.IsCurrentAddress(testRow, new DateTime(2015, 3, 1)), "3/1 current between 3/1 and 9/30.");
            Assert.IsFalse(_logic.IsCurrentAddress(testRow, new DateTime(2015, 10, 1)), "10/1 not current between 3/1 and 9/30.");
            Assert.IsFalse(_logic.IsCurrentAddress(testRow, new DateTime(2015, 2, 28)), "2/29 not current between 3/1 and 9/30.");

            // Vacation dates: 9/1 thru 3/31
            testRow.StartDate = testRow.EndDate = null;
            testRow.StartDay = new DateTime(2015, 9, 1).DayOfYear;
            testRow.EndDay = new DateTime(2015, 3, 31).DayOfYear;
            Assert.IsFalse(_logic.IsCurrentAddress(testRow, testDate), "Not current if outside 9/1 and 3/31.");
            Assert.IsTrue(_logic.IsCurrentAddress(testRow, new DateTime(2015, 12, 31)), "Current if inside 9/1 and 3/31.");
            Assert.IsTrue(_logic.IsCurrentAddress(testRow, new DateTime(2015, 3, 31)), "3/1 current between 9/1 and 3/31.");
            Assert.IsFalse(_logic.IsCurrentAddress(testRow, new DateTime(2015, 4, 1)), "4/1 not current between 9/1 and 3/31.");
            Assert.IsTrue(_logic.IsCurrentAddress(testRow, new DateTime(2015, 9, 1)), "9/1 current between 9/1 and 3/31.");
            Assert.IsFalse(_logic.IsCurrentAddress(testRow, new DateTime(2015, 8, 31)), "8/31 not current between 9/1 and 3/31.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ConstituentAddressLogic_GetAddress()
        {
            var testRow = _constituentAddresses[0];
            var testDate = new DateTime(2016, 6, 15);

            Assert.AreEqual(_homeAddress, _logic.GetAddress(_constituents[0], AddressCategory.Primary, "", false, false, null, null), "Get primary address.");

            Assert.AreEqual(_mailingAddress, _logic.GetAddress(_constituents[0], AddressCategory.Mailing, "", false, false, null, null), "Get mailing address.");

            Assert.AreEqual(_homeAddress, _logic.GetAddress(_constituents[0], AddressCategory.Location, "", false, false, null, null), "Get location address.");

            Assert.AreEqual(_workAddress, _logic.GetAddress(_constituents[0], AddressCategory.None, "A,W", false, false, null, null), "Get alternate or work address.");

            Assert.AreEqual(_vacationAddress, _logic.GetAddress(_constituents[0], AddressCategory.Mailing, "", true, true, new DateTime(2015, 12, 31), null), "Get vacation address.");

            Assert.AreEqual(_mailingAddress, _logic.GetAddress(_constituents[0], AddressCategory.Mailing, "", true, true, new DateTime(2015, 5, 31), null), "Get vacation address (non-current).");

            Assert.AreEqual(_workAddress, _logic.GetAddress(_constituents[0], AddressCategory.Primary, "", false, false, null, _homeAddress.Id), "Get primary address, exclude home address.");

        }
    }
}