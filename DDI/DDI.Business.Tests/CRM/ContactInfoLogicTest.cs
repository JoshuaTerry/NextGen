﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Business.Tests.Common.DataSources;
using DDI.Business.Tests.CRM.DataSources;
using DDI.Data;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.CRM
{
    [TestClass]
    public class ContactInfoLogicTest : TestBase
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
        public void ContactInfoLogic_ValidatePhoneNumber()
        {
            BuildConstituentDataSource();

            _usPhoneContactInfo.Info = "317-555-1234";
            AssertNoException(() => _bl.ValidatePhoneNumber(_usPhoneContactInfo), "10-digit US format valid");
            Assert.AreEqual("3175551234", _usPhoneContactInfo.Info, "10-digit US format returns raw digits");

            AssertNoException(() => _bl.ValidatePhoneNumber(_foreignPhoneContactInfo), "9-digit French format valid");

            AssertThrowsException<ArgumentNullException>(() => _bl.ValidatePhoneNumber(null), "Null argument should throw exception.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ContactInfoLogic_SimplifyPhoneNumberForCountry()
        {
            string result;
            Country country = _countries.FirstOrDefault(p => p.ISOCode == AddressDefaults.DefaultCountryCode);
            
            result = _bl.SimplifyPhoneNumberForCountry("317-555-1234", country);
            Assert.IsNotNull(result, "10-digit US format valid");
            Assert.AreEqual("3175551234", result, "10-digit US format returns raw digits");

            result = _bl.SimplifyPhoneNumberForCountry("(317) 555-1234 x5678", country);
            Assert.IsNotNull(result, "10-digit with extension valid");
            Assert.AreEqual("3175551234 x5678", result, "First 10 digits returned raw");

            result = _bl.SimplifyPhoneNumberForCountry("1-317-555-1234", country);        
            Assert.IsNotNull(result, "10-digit US format with trunk prefix valid");
            Assert.AreEqual("3175551234", result, "10-digit US format with trunk prefix returns raw digits");

            result = _bl.SimplifyPhoneNumberForCountry("555-1234", country);
            Assert.IsNull(result, "7 digit format not valid");

            country = _countries.FirstOrDefault(p => p.ISOCode == "CA");            
            result = _bl.SimplifyPhoneNumberForCountry("403-555-1234", country);
            Assert.IsNotNull(result, "10-digit Canadian format valid");
            Assert.AreEqual("4035551234", result, "10-digit Canadian format returns raw digits");

            result = _bl.SimplifyPhoneNumberForCountry("+011-1-403-555-1234", country);
            Assert.IsNotNull(result, "Full international Canadian format valid");
            Assert.AreEqual("4035551234", result, "Full international Canadian format returns raw digits");

            country = _countries.FirstOrDefault(p => p.ISOCode == "FR");
            result = _bl.SimplifyPhoneNumberForCountry("5 23 45 67 89", country);
            Assert.IsNotNull(result, "9-digit French format valid");
            Assert.AreEqual("523456789", result, "9-digit French format returns raw digits");

            result = _bl.SimplifyPhoneNumberForCountry("+011-33-523-456789", country);
            Assert.IsNotNull(result, "9-digit French format with US dialing prefix valid");
            Assert.AreEqual("523456789", result, "9-digit French format with US dialing prefix returns raw digits");
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

            AssertNoException(() => _bl.Validate(_usPhoneContactInfo), "Validate for valid ContactInfo should pass.");

            _usPhoneContactInfo.Info = string.Empty;
            AssertThrowsException<Exception>(() => _bl.Validate(_usPhoneContactInfo), "Validate should fail on blank contact info.");

            _usPhoneContactInfo.Info = "1234";
            AssertThrowsException<Exception>(() => _bl.Validate(_usPhoneContactInfo), "Validate should fail on invalid phone #.");

            _usPhoneContactInfo.Info = "3175551234";
            _usPhoneContactInfo.ContactTypeId = null;
            AssertThrowsException<Exception>(() => _bl.Validate(_usPhoneContactInfo), "Validate should fail if no contact type ID.");

            _usPhoneContactInfo.ContactTypeId = _usPhoneContactInfo.ContactType.Id;
            _usPhoneContactInfo.ConstituentId = null;
            AssertThrowsException<Exception>(() => _bl.Validate(_usPhoneContactInfo), "Validate should fail if no parent.");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ContactInfoLogic_Validate_IsPreferred()
        {
            BuildConstituentDataSource();
            ContactInfo isPreferredContactInfo1 = new ContactInfo()
            {
                Id = GuidHelper.NewSequentialGuid(),
                ContactType = _contactTypes.FirstOrDefault(p => p.Code == "H" && p.ContactCategory.Code == ContactCategoryCodes.Phone),
                ContactTypeId = _usPhoneContactInfo.ContactType.Id,
                Constituent = _constituents[0],
                ConstituentId = _constituents[0].Id,
                Info = "3175551235",
                IsPreferred = true
            };

            _uow.GetRepository<ContactInfo>().Insert(isPreferredContactInfo1);
            _bl.Validate(isPreferredContactInfo1);
            var actualPreferredContactInfo = _uow.GetRepository<ContactInfo>().Entities.FirstOrDefault(ci => ci.ConstituentId == isPreferredContactInfo1.ConstituentId && ci.ContactType.ContactCategory.Code == isPreferredContactInfo1.ContactType.ContactCategory.Code && ci.IsPreferred); 

            Assert.AreEqual(actualPreferredContactInfo, isPreferredContactInfo1, "Sets isPreferred ContactInfo"); 

            ContactInfo isPreferredContactInfo2 = new ContactInfo()
            {
                Id = GuidHelper.NewSequentialGuid(),
                ContactType = _contactTypes.FirstOrDefault(p => p.Code == "H" && p.ContactCategory.Code == ContactCategoryCodes.Phone),
                ContactTypeId = _usPhoneContactInfo.ContactType.Id,
                Constituent = _constituents[0],
                ConstituentId = _constituents[0].Id,
                Info = "3175554321",
                IsPreferred = true
            };

            _uow.GetRepository<ContactInfo>().Insert(isPreferredContactInfo2);
            _bl.Validate(isPreferredContactInfo2);
            actualPreferredContactInfo = _uow.GetRepository<ContactInfo>().Entities.FirstOrDefault(ci => ci.ConstituentId == isPreferredContactInfo2.ConstituentId && ci.ContactType.ContactCategory.Code == isPreferredContactInfo2.ContactType.ContactCategory.Code && ci.IsPreferred); // not picking up the preferred contact info... // && ci.Id != oldPreferredContactInfo.Id && ci.IsPreferred

            Assert.AreEqual(actualPreferredContactInfo, isPreferredContactInfo2, "Ensure previous isPreferred was overwritten");
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
                Id = GuidHelper.NewSequentialGuid()
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
                Id = GuidHelper.NewSequentialGuid()
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
                Id = GuidHelper.NewSequentialGuid()
            });

            _constituentAddresses.Add(new ConstituentAddress()
            {
                Address = AddressDataSource.FrenchAddress,
                Constituent = _constituents[1],
                AddressType = _addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Home),
                IsPrimary = true,
                ResidentType = ResidentType.Primary,
                Id = GuidHelper.NewSequentialGuid()
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
