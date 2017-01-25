using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Business.Tests.Common.DataSources;
using DDI.Business.Tests.CRM.DataSources;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.CRM
{
    [TestClass]
    public class ContactInfoLogicTest
    {
        private const string TESTDESCR = "Business | CRM";
        private UnitOfWorkNoDb _uow;
        private IList<Country> _countries;
        private List<Constituent> _constituents;
        private List<ConstituentAddress> _constituentAddresses;
        private IList<ConstituentType> _constituentTypes;
        private IList<Gender> _genders;
        private IList<Prefix> _prefixes;
        private IList<ConstituentStatus> _statuses;
        private IList<RelationshipType> _relationshipTypes;
        private IList<Address> _addresses;
        private IList<AddressType> _addressTypes;
        private IList<ContactInfo> _contactInfo;
        private IList<ContactCategory> _contactCategories;
        private IList<ContactType> _contactTypes;
        private ContactInfoLogic _bl;
        private ContactInfo _emailContactInfo, _usPhoneContactInfo, _foreignPhoneContactInfo;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

            _countries = CountryDataSource.GetDataSource(_uow);
            _contactCategories = ContactCategoryDataSource.GetDataSource(_uow);
            _contactTypes = ContactTypeDataSource.GetDataSource(_uow);
            _bl = new ContactInfoLogic(_uow);

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ContactInfoLogic_GetContactCategoryCode()
        {
            // Getting the contact category code for a ContactType...
            ContactType type = _contactTypes.FirstOrDefault(p => p.Code == "H" && p.ContactCategory.Code == ContactCategoryCodes.Email); // Email - Home
            Assert.AreEqual(ContactCategoryCodes.Email, _bl.GetContactCategoryCode(type), "Get category code for ContactType");

            // Getting the contact category code for a ContactInfo...
            ContactInfo info = new ContactInfo()
            {
                ContactType = type,
                Info = ""
            };

            Assert.AreEqual(ContactCategoryCodes.Email, _bl.GetContactCategoryCode(info), "Get category code for ContactInfo");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ContactInfoLogic_ValidatePhoneNumber_Entity()
        {
            BuildConstituentDataSource();

            _usPhoneContactInfo.Info = "317-555-1234";
            Assert.IsTrue(_bl.ValidatePhoneNumber(_usPhoneContactInfo), "10-digit US format valid");
            Assert.AreEqual("3175551234", _usPhoneContactInfo.Info, "10-digit US format returns raw digits");

            Assert.IsTrue(_bl.ValidatePhoneNumber(_foreignPhoneContactInfo), "9-digit French format valid");

            FailIfNotException<ArgumentNullException>(() => _bl.ValidatePhoneNumber(null), "Null argument should throw exception.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ContactInfoLogic_ValidatePhoneNumber_String()
        {
            string phone;
            Country country = _countries.FirstOrDefault(p => p.ISOCode == AddressDefaults.DefaultCountryCode);

            phone = "317-555-1234";
            Assert.IsTrue(_bl.ValidatePhoneNumber(ref phone, country), "10-digit US format valid");
            Assert.AreEqual("3175551234", phone, "10-digit US format returns raw digits");

            phone = "(317) 555-1234 x5678";
            Assert.IsTrue(_bl.ValidatePhoneNumber(ref phone, country), "10-digit with extension valid");
            Assert.AreEqual("3175551234 x5678", phone, "First 10 digits returned raw");

            phone = "1-317-555-1234";
            Assert.IsTrue(_bl.ValidatePhoneNumber(ref phone, country), "10-digit US format with trunk prefix valid");
            Assert.AreEqual("3175551234", phone, "10-digit US format with trunk prefix returns raw digits");

            phone = "555-1234";
            Assert.IsFalse(_bl.ValidatePhoneNumber(ref phone, country), "7 digit format not valid");
            Assert.AreEqual("555-1234", phone, "Invalid format returned as-is");

            country = _countries.FirstOrDefault(p => p.ISOCode == "CA");
            phone = "317-555-1234";
            Assert.IsTrue(_bl.ValidatePhoneNumber(ref phone, country), "10-digit Canadian format valid");
            Assert.AreEqual("3175551234", phone, "10-digit Canadian format returns raw digits");

            phone = "+011-1-317-555-1234";
            Assert.IsTrue(_bl.ValidatePhoneNumber(ref phone, country), "Full international Canadian format valid");
            Assert.AreEqual("3175551234", phone, "Full international Canadian format returns raw digits");

            country = _countries.FirstOrDefault(p => p.ISOCode == "FR");
            phone = "5 23 45 67 89";
            Assert.IsTrue(_bl.ValidatePhoneNumber(ref phone, country), "9-digit French format valid");
            Assert.AreEqual("523456789", phone, "9-digit French format returns raw digits");

            phone = "+011-33-523-456789";
            Assert.IsTrue(_bl.ValidatePhoneNumber(ref phone, country), "9-digit French format with US dialing prefix valid");
            Assert.AreEqual("523456789", phone, "9-digit French format with US dialing prefix returns raw digits");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ContactInfoLogic_FormatContactInformation()
        {
            BuildConstituentDataSource();

            string formattedInfo = _bl.FormatContactInformation(_usPhoneContactInfo);
            Assert.AreEqual("(317) 555-1234", formattedInfo, "US phone # formatted correctly");

            formattedInfo = _bl.FormatContactInformation(_foreignPhoneContactInfo);
            Assert.AreEqual("+33 5 61 29 81 96", formattedInfo, "French phone # formatted correctly");

            formattedInfo = _bl.FormatContactInformation(_emailContactInfo);
            Assert.AreEqual(_emailContactInfo.Info, formattedInfo, "Email returned as-is.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ContactInfoLogic_FormatPhoneNumber_String()
        {
            string rawPhone = "3175551234";
            Country country = _countries.FirstOrDefault(p => p.ISOCode == AddressDefaults.DefaultCountryCode);

            string formattedPhone = _bl.FormatPhoneNumber(rawPhone, country, false, false, false);
            Assert.AreEqual("(317) 555-1234", formattedPhone, "Default US formatting");

            formattedPhone = _bl.FormatPhoneNumber(rawPhone, country, false, true, false);
            Assert.AreEqual("1-317-555-1234", formattedPhone, "NANP formatting");

            formattedPhone = _bl.FormatPhoneNumber(rawPhone, country, false, true, true);
            Assert.AreEqual("1-317-555-1234", formattedPhone, "NANP formatting, from local country");

            rawPhone = "561298196";
            country = _countries.FirstOrDefault(p => p.ISOCode == "FR");
            formattedPhone = _bl.FormatPhoneNumber(rawPhone, country, false, false, false);
            Assert.AreEqual("+33 5 61 29 81 96", formattedPhone, "Default FR formatting");

            formattedPhone = _bl.FormatPhoneNumber(rawPhone, country, true, false, false);
            Assert.AreEqual("011 33 5 61 29 81 96", formattedPhone, "FR formatting with international prefix");

            formattedPhone = _bl.FormatPhoneNumber(rawPhone, country, false, false, true);
            Assert.AreEqual("05 61 29 81 96", formattedPhone, "FR formatting from FR");

            formattedPhone = _bl.FormatPhoneNumber(null, country, false, false, true);
            Assert.AreEqual(string.Empty, formattedPhone, "Null phone returns empty.");

            formattedPhone = _bl.FormatPhoneNumber(string.Empty, country, false, false, true);
            Assert.AreEqual(string.Empty, formattedPhone, "Empty phone returns empty.");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ContactInfoLogic_FormatPhoneNumber_Entity()
        {
            BuildConstituentDataSource();

            string formattedPhone = _bl.FormatPhoneNumber(_usPhoneContactInfo, false, false, false);
            Assert.AreEqual("(317) 555-1234", formattedPhone, "Default US formatting");

            formattedPhone = _bl.FormatPhoneNumber(_foreignPhoneContactInfo, false, false, false);
            Assert.AreEqual("+33 5 61 29 81 96", formattedPhone, "Default FR formatting");

            formattedPhone = _bl.FormatPhoneNumber(null, false, false, false);
            Assert.AreEqual(string.Empty, formattedPhone, "Null contact info returns empty.");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ContactInfoLogic_Validate()
        {
            BuildConstituentDataSource();

            FailIfException(() => _bl.Validate(_usPhoneContactInfo), "Validate for valid ContactInfo should pass.");

            _usPhoneContactInfo.Info = string.Empty;
            FailIfNotException<Exception>(() => _bl.Validate(_usPhoneContactInfo), "Validate should fail on blank contact info.");

            _usPhoneContactInfo.Info = "1234";
            FailIfNotException<Exception>(() => _bl.Validate(_usPhoneContactInfo), "Validate should fail on invalid phone #.");

            _usPhoneContactInfo.Info = "3175551234";
            _usPhoneContactInfo.ContactTypeId = null;
            FailIfNotException<Exception>(() => _bl.Validate(_usPhoneContactInfo), "Validate should fail if no contact type ID.");

            _usPhoneContactInfo.ContactTypeId = _usPhoneContactInfo.ContactType.Id;
            _usPhoneContactInfo.ConstituentId = null;
            FailIfNotException<Exception>(() => _bl.Validate(_usPhoneContactInfo), "Validate should fail if no parent.");

        }

        private void FailIfException(Action action, string message)
        {
            try
            {
                action.Invoke();
            }
            catch
            {
                Assert.Fail(message);
            }
        }

        private void FailIfNotException<T>(Action action, string message) where T : Exception
        {
            try
            {
                action.Invoke();
            }
            catch (T)
            {
                return;
            }
            Assert.Fail(message);
        }

        private void BuildConstituentDataSource()
        {
            _genders = GenderDataSource.GetDataSource(_uow);
            _constituentTypes = ConstituentTypeDataSource.GetDataSource(_uow);
            _prefixes = PrefixDataSource.GetDataSource(_uow);
            _statuses = ConstituentStatusDataSource.GetDataSource(_uow);
            RelationshipCategoryDataSource.GetDataSource(_uow);
            _relationshipTypes = RelationshipTypeDataSource.GetDataSource(_uow);
            _addresses = AddressDataSource.GetDataSource(_uow);
            _addressTypes = AddressTypeDataSource.GetDataSource(_uow);

            CRMConfigurationDataSource.GetDataSource(_uow);

            ConstituentType individualType = _constituentTypes.FirstOrDefault(p => p.Category == ConstituentCategory.Individual);
            
            _constituents = new List<Constituent>();
            _constituents.Add(new Constituent()
            {
                ConstituentNumber = 1,
                Prefix = _prefixes.FirstOrDefault(p => p.Code == "Mr"),
                FirstName = "John",
                MiddleName = "Quincy",
                LastName = "Public",
                Suffix = "Jr.",
                Nickname = "Jeff",
                ConstituentType = individualType,
                ConstituentStatus = _statuses.FirstOrDefault(p => p.BaseStatus == ConstituentBaseStatus.Active),
                Gender = _genders.FirstOrDefault(p => p.Code == "M"),
                Id = GuidHelper.NextGuid()
            });

            _constituents.Add(new Constituent()
            {
                ConstituentNumber = 2,
                Prefix = _prefixes.FirstOrDefault(p => p.Code == "Mon"),
                FirstName = "Jaques",
                MiddleName = "",
                LastName = "Cousteau",
                Suffix = "",
                ConstituentType = individualType,
                ConstituentStatus = _statuses.FirstOrDefault(p => p.BaseStatus == ConstituentBaseStatus.Active),
                Gender = _genders.FirstOrDefault(p => p.Code == "M"),
                Id = GuidHelper.NextGuid()
            });

            _uow.CreateRepositoryForDataSource(_constituents);

            _constituentAddresses = new List<ConstituentAddress>();
            _constituentAddresses.Add(new ConstituentAddress()
            {
                Address = _addresses[0],
                Constituent = _constituents[0],
                AddressType = _addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Home),
                IsPrimary = true,
                ResidentType = ResidentType.Primary,
                Id = GuidHelper.NextGuid()
            });

            _constituentAddresses.Add(new ConstituentAddress()
            {
                Address = AddressDataSource.FrenchAddress,
                Constituent = _constituents[1],
                AddressType = _addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Home),
                IsPrimary = true,
                ResidentType = ResidentType.Primary,
                Id = GuidHelper.NextGuid()
            });


            // Populate the ConstituentAddresses collection for each constituent.
            foreach (var entry in _constituents)
            {
                entry.ConstituentAddresses = _constituentAddresses.Where(p => p.Constituent == entry).ToList();
            }

            _uow.CreateRepositoryForDataSource(_constituentAddresses);

            // Create some contact information entries.

            // Phone # for US constituent
            _contactInfo = new List<ContactInfo>();
            _contactInfo.Add(_usPhoneContactInfo = new ContactInfo()
            {
                ContactType = _contactTypes.FirstOrDefault(p => p.Code == "H" && p.ContactCategory.Code == ContactCategoryCodes.Phone),
                Constituent = _constituents[0],
                Info = "3175551234"                
            });

            // Email
            _contactInfo.Add(_emailContactInfo = new ContactInfo()
            {
                ContactType = _contactTypes.FirstOrDefault(p => p.Code == "H" && p.ContactCategory.Code == ContactCategoryCodes.Email),
                Constituent = _constituents[0],
                Info = "jpublic@gmail.com"
            });


            // Phone # for French constituent
            _contactInfo.Add(_foreignPhoneContactInfo = new ContactInfo()
            {
                ContactType = _contactTypes.FirstOrDefault(p => p.Code == "H" && p.ContactCategory.Code == ContactCategoryCodes.Phone),
                Constituent = _constituents[1],
                Info = "561298196"
            });

            _uow.CreateRepositoryForDataSource(_contactInfo);

        }
    }
}
