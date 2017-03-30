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
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.CRM
{
    [TestClass]
    public class NameFormatterTest : TestBase
    {

        private const string TESTDESCR = "Business | CRM";
        private UnitOfWorkNoDb _uow;
        private List<Constituent> _constituents;
        private List<ConstituentAddress> _constituentAddresses;
        private List<Relationship> _relationships;
        private IList<ConstituentType> _constituentTypes;
        private IList<Gender> _genders;
        private IList<Prefix> _prefixes;
        private IList<ConstituentStatus> _statuses;
        private IList<RelationshipType> _relationshipTypes;
        private IList<Address> _addresses;
        private IList<AddressType> _addressTypes;
        private NameFormatter _logic;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

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

            // Set up two constituents with a spouse relationship.
            _constituentAddresses = new List<ConstituentAddress>();

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
                Prefix = _prefixes.FirstOrDefault(p => p.Code == "Mrs"),
                FirstName = "Jane",
                MiddleName = "Elizabeth",
                LastName = "Public",
                Suffix = "",
                ConstituentType = individualType,
                ConstituentStatus = _statuses.FirstOrDefault(p => p.BaseStatus == ConstituentBaseStatus.Active),
                Gender = _genders.FirstOrDefault(p => p.Code == "F"),
                Id = GuidHelper.NewSequentialGuid()
            });

            // Organization

            _constituents.Add(new Constituent()
            {
                ConstituentNumber = 3,
                Name = "St. Mark's Church",
                ConstituentType = _constituentTypes.FirstOrDefault(p => p.Code == ConstituentTypeCodes.Organization),
                ConstituentStatus = _statuses.FirstOrDefault(p => p.BaseStatus == ConstituentBaseStatus.Active),
                Id = GuidHelper.NewSequentialGuid()
            });

            // Individual unrelated to John or Jane Public
            _constituents.Add(new Constituent()
            {
                ConstituentNumber = 4,
                Prefix = _prefixes.FirstOrDefault(p => p.Code == "Ms"),
                FirstName = "Mary",
                MiddleName = "",
                LastName = "Public",
                Suffix = "",
                ConstituentType = individualType,
                ConstituentStatus = _statuses.FirstOrDefault(p => p.BaseStatus == ConstituentBaseStatus.Active),
                Gender = _genders.FirstOrDefault(p => p.Code == "F"),
                Id = GuidHelper.NewSequentialGuid()
            });

            _uow.CreateRepositoryForDataSource(_constituents);

            // Relationships

            _relationships = new List<Relationship>();
            _relationships.Add(new Relationship()
            {
                Constituent1 = _constituents[0],
                Constituent2 = _constituents[1],
                RelationshipType = _relationshipTypes.FirstOrDefault(p => p.Code == "SPOU"),
                Id = GuidHelper.NewSequentialGuid()
            });

            _uow.CreateRepositoryForDataSource(_relationships);

            _constituents[0].Relationship1s = _relationships.Where(p => p.Constituent1 == _constituents[0]).ToList();
            _constituents[0].Relationship2s = _relationships.Where(p => p.Constituent2 == _constituents[0]).ToList();

            _constituents[1].Relationship1s = _relationships.Where(p => p.Constituent1 == _constituents[1]).ToList();
            _constituents[1].Relationship2s = _relationships.Where(p => p.Constituent2 == _constituents[1]).ToList();

            // Constituent addresses
            // Individuals will share two addresses.
            LinkTwoAddresses(_constituents[0]);
            LinkTwoAddresses(_constituents[1]);
            LinkTwoAddresses(_constituents[3]);

            // Organization 
            _constituentAddresses.Add(new ConstituentAddress()
            {
                Address = _addresses[0],
                Constituent = _constituents[2],
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

            _logic = new NameFormatter(_uow);

        }

        private void LinkTwoAddresses(Constituent constituent)
        {

            // First address is home
            _constituentAddresses.Add(new ConstituentAddress()
            {
                Address = _addresses[1],
                Constituent = constituent,
                AddressType = _addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Home),
                IsPrimary = true,
                ResidentType = ResidentType.Primary,
                Id = GuidHelper.NewSequentialGuid()
            });

            // Second address is work
            _constituentAddresses.Add(new ConstituentAddress()
            {
                Address = _addresses[2],
                Constituent = constituent,
                AddressType = _addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Work),
                IsPrimary = false,
                ResidentType = ResidentType.Primary,
                Id = GuidHelper.NewSequentialGuid()
            });
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void NameFormatter_FormatIndividualSortName()
        {
            Assert.AreEqual("Public John Q.", _logic.FormatIndividualSortName(_constituents[0]), "Default formatting");

            _constituents[0].NameFormat = "{FI}{MIDDLE}{LAST}";
            Assert.AreEqual("Public J. Quincy", _logic.FormatIndividualSortName(_constituents[0]), "First initial");

            _constituents[0].NameFormat = "{FIRST}{MIDDLE}{LI}";
            Assert.AreEqual("P. John Quincy", _logic.FormatIndividualSortName(_constituents[0]), "Last initial");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void NameFormatter_FormatIndividualName()
        {
            Assert.AreEqual("John Q. Public Jr.", _logic.FormatIndividualName(_constituents[0], null), "Default formatting");

            Assert.AreEqual("Mr. Public", _logic.FormatIndividualName(_constituents[0], "Mr. {LAST}"), "Specific format");

            Assert.AreEqual("Mr. Public", _logic.FormatIndividualName(_constituents[0], "{MR}{LAST}"), "MR macro - Male");
            Assert.AreEqual("Ms. Public", _logic.FormatIndividualName(_constituents[1], "{MR}{LAST}"), "MR macro - Female");

            Assert.AreEqual("Mr. Public", _logic.FormatIndividualName(_constituents[0], "{MADAM}{LAST}"), "MADAM macro - Male");
            Assert.AreEqual("Madam Public", _logic.FormatIndividualName(_constituents[1], "{MADAM}{LAST}"), "MADAM macro - Female");

            Assert.AreEqual("Brother John", _logic.FormatIndividualName(_constituents[0], "{BROTHER}{FIRST}"), "BROTHER macro - Male");
            Assert.AreEqual("Sister Jane", _logic.FormatIndividualName(_constituents[1], "{BROTHER}{FIRST}"), "BROTHER macro - Female");

            Assert.AreEqual("His Excellency", _logic.FormatIndividualName(_constituents[0], "{HIS} Excellency"), "HIS macro - Male");
            Assert.AreEqual("Her Excellency", _logic.FormatIndividualName(_constituents[1], "{HIS} Excellency"), "HIS macro - Female");

            Assert.AreEqual("Jeff", _logic.FormatIndividualName(_constituents[0], "{NICKNAME}"), "Nickname");
            Assert.AreEqual("Jane", _logic.FormatIndividualName(_constituents[1], "{NICKNAME}"), "Nickname defaults to first name");

            _constituents[0].NameFormat = "{FI}{MIDDLE}{LAST}";
            Assert.AreEqual("J. Quincy Public", _logic.FormatIndividualName(_constituents[0], null), "First initial");

            _constituents[0].NameFormat = "{FIRST}{MIDDLE}{LI}";
            Assert.AreEqual("John Quincy P.", _logic.FormatIndividualName(_constituents[0], null), "Last initial");

            _constituents[0].NameFormat = "{FIRST} ({NICKNAME}) {LAST}";
            Assert.AreEqual("John (Jeff) Public", _logic.FormatIndividualName(_constituents[0], null), "Nickname");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void NameFormatter_GetIndividualNameLines()
        {
            string line1, line2;

            _logic.GetIndividualNameLines(_constituents[0], null, LabelRecipient.Primary, false, false, false, 0, out line1, out line2);

            Assert.AreEqual("Mr. John Q. Public Jr.", line1, "Default formatting");
            // Further tests are in BuildNameLines which calls same logic as BuildIndividualNameLines.
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void NameFormatter_GetNameLines_Organization()
        {
            string line1, line2;
            NameFormattingOptions options = new NameFormattingOptions();

            _logic.GetNameLines(_constituents[2], null, options, out line1, out line2);
            Assert.AreEqual(_constituents[2].Name, line1, "Default formating - organization");

            _constituents[2].Name2 = "ATTN: Treasurer";

            _logic.GetNameLines(_constituents[2], null, options, out line1, out line2);
            Assert.AreEqual(_constituents[2].Name2, line2, "Organization with Name2");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void NameFormatter_GetNameLines_Individual()
        {
            string line1, line2;
            NameFormattingOptions options = new NameFormattingOptions();

            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. Public Jr.", line1, "Default formating - individual");

            options.OmitPrefix = true;
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("John Q. Public Jr.", line1, "Omit prefix");

            options.OmitPrefix = false;
            options.Recipient = LabelRecipient.Both;
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. and Mrs. John Q. Public Jr.", line1, "Recipient=Both");

            options.Recipient = LabelRecipient.Secondary;
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mrs. Jane E. Public", line1, "Recipient=Secondary");

            options.Recipient = LabelRecipient.Wife;
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mrs. Jane E. Public", line1, "Recipient=Wife");

            options.Recipient = LabelRecipient.Husband;
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. Public Jr.", line1, "Recipient=Husband");

            options.Recipient = LabelRecipient.Both;
            options.OmitPrefix = true;
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("John Q. Public Jr. and Jane E. Public", line1, "Recipient=Both, omit prefixes");

            options.OmitPrefix = false;
            options.KeepSeparate = true;
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. Public Jr. and Mrs. Jane E. Public", line1, "Recipient=Both, keep separate");

            options.KeepSeparate = false;
            options.AddFirstNames = true;
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. and Mrs. Jane E. Public", line1, "Recipient=Both, add first names");

            options.MaxChars = 20;
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. and", line1, "Recipient=Both, max chars line 1");
            Assert.AreEqual("Mrs. Jane E. Public", line2, "Recipient=Both, max chars line 2");

            options.AddFirstNames = false;
            options.MaxChars = 0;

            _constituents[0].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Dr");
            _constituents[1].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Dr");
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Drs. John Q. and Jane E. Public", line1, "Recipient=Both, pluralize prefix");

            _constituents[0].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Law");
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("John Q. Public Jr., Esq. and Dr. Jane E. Public", line1, "Recipient=Both, non-combining prefix");

            _constituents[0].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Mr");
            _constituents[1].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Law");
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Jane E. Public, Esq. and Mr. John Q. Public Jr.", line1, "Recipient=Both, swap names");

            _constituents[0].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Mr");
            _constituents[1].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Mrs");

            _constituents[1].ConstituentStatus = _statuses.FirstOrDefault(p => p.Code == "DEL");
            options.IncludeInactive = false;
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. Public Jr.", line1, "Don't include deleted spouse.");

            options.IncludeInactive = true;
            _logic.GetNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. and Mrs. John Q. Public Jr.", line1, "Include deleted spouse.");

            options.Recipient = LabelRecipient.Primary;
            _logic.GetNameLines(_constituents[0], _constituents[3], options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. Public Jr.", line1, "First of two constituents");
            Assert.AreEqual("Ms. Mary Public", line2, "Second of two constituents");

            options.IsSpouse = true;
            _logic.GetNameLines(_constituents[0], _constituents[3], options, out line1, out line2);
            Assert.AreEqual("Mr. and Ms. John Q. Public Jr.", line1, "Two constituents, IsSpouse=true");


        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void NameFormatter_GetAddressLabel()
        {
            LabelFormattingOptions options = new LabelFormattingOptions();
            AbbreviationDataSource.GetDataSource(_uow);

            // Note: The name formatting logic is tested in BuildNameLines.  The following tests are for 
            // additional logic in BuildAddressLabel.

            List<string> label = _logic.GetAddressLabel(_constituents[0], null, null, options, false);
            Assert.AreEqual("Mr. John Q. Public Jr.|24 SE 1st Ave|Ocala, FL 34471", string.Join("|", label), "Default formatting");

            options.AddressCategory = AddressCategory.Alternate;
            label = _logic.GetAddressLabel(_constituents[0], null, null, options, false);
            Assert.AreEqual("Mr. John Q. Public Jr.|204 W Broadway St.|Frankfort, KY 40601", string.Join("|", label), "Alternate address");

            options.AddressCategory = AddressCategory.Primary;
            options.AddressTypeCode = "W";
            label = _logic.GetAddressLabel(_constituents[0], null, null, options, false);
            Assert.AreEqual("Mr. John Q. Public Jr.|204 W Broadway St.|Frankfort, KY 40601", string.Join("|", label), "Work address");

            options.AddressTypeCode = string.Empty;
            label = _logic.GetAddressLabel(_constituents[0], null, _addresses[3], options, false);
            Assert.AreEqual("Mr. John Q. Public Jr.|324 S Grand St|Enid, OK 73701", string.Join("|", label), "Specific address");

            options.Caps = true;
            label = _logic.GetAddressLabel(_constituents[0], null, null, options, false);
            Assert.AreEqual("MR. JOHN Q. PUBLIC JR.|24 SE 1ST AVE|OCALA, FL 34471", string.Join("|", label), "Caps formatting");

            options.Caps = false;
            options.ExpandAddress = true;
            label = _logic.GetAddressLabel(_constituents[2], null, null, options, false);
            Assert.AreEqual("St. Mark's Church|101 West Ohio Street|Suite 1650|Indianapolis, IN 46204", string.Join("|", label), "Expand address formatting");

            options.ExpandName = true;
            label = _logic.GetAddressLabel(_constituents[2], null, null, options, false);
            Assert.AreEqual("Saint Mark's Church|101 West Ohio Street|Suite 1650|Indianapolis, IN 46204", string.Join("|", label), "Expand name formatting");

            options.ExpandAddress = false;
            options.ExpandName = false;
            options.ContactName = "ATTN: John Doe";
            label = _logic.GetAddressLabel(_constituents[2], null, null, options, false);
            Assert.AreEqual("St. Mark's Church|ATTN: John Doe|101 W. Ohio St.|Suite 1650|Indianapolis, IN 46204", string.Join("|", label), "Contact name");

            options.ContactName = string.Empty;
            label = _logic.GetAddressLabel(null, null, _addresses[0], options, false);
            Assert.AreEqual("101 W. Ohio St.|Suite 1650|Indianapolis, IN 46204", string.Join("|", label), "No constituent; address only");


        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void NameFormatter_GetSalutation_Individual()
        {
            SalutationFormattingOptions options = new SalutationFormattingOptions();
            string result;
            
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear Mr. and Mrs. Public:", result, "Default formating");

            options.AddFirstNames = true;
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear Mr. John Q. and Mrs. Jane E. Public:", result, "Add first names to spouses");

            options.AddFirstNames = false;
            options.KeepSeparate = true;
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear Mr. Public and Mrs. Public:", result, "KeepSeparate = true");

            options.KeepSeparate = false;
            _constituents[1].ConstituentStatus = _statuses.FirstOrDefault(p => p.Code == "DEL");
            options.IncludeInactive = false;
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear Mr. Public:", result, "Don't include deleted spouse.");

            options.IncludeInactive = true;
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear Mr. and Mrs. Public:", result, "Include deleted spouse.");

            _constituents[1].ConstituentStatus = null;
            options.PreferredType = SalutationType.Informal;
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear John and Jane,", result, "Informal formating");

            _constituents[0].SalutationType = SalutationType.Formal;
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear Mr. and Mrs. Public:", result, "Constituent salutation format overrides preferred format");

            options.ForcePreferredtype = true;
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear John and Jane,", result, "Forced informal formating");

            options.ForcePreferredtype = false;
            options.PreferredType = SalutationType.Default;
            options.CustomSalutation = "Dear Friends,";
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear Friends,", result, "Custom salutation from SalutationFormattingOptions");

            options.CustomSalutation = string.Empty;
            _constituents[0].Salutation = "Dear Jeff";
            _constituents[0].SalutationType = SalutationType.Custom;
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear Jeff and Mrs. Public:", result, "Custom salutation from constituent");
            
            _constituents[0].Salutation = "";
            _constituents[0].SalutationType = SalutationType.Default;
            _constituents[0].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Dr");
            _constituents[1].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Dr");
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear Drs. John Q. and Jane E. Public:", result, "Pluralize prefix");

            _constituents[0].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Abbot");
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear Father Abbot and Dr. Public:", result, "Non-combining prefix");

            _constituents[0].Prefix = null;
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear Mr. and Dr. Public:", result, "Missing prefix implied by gender");

            _constituents[0].Gender = null;
            result = _logic.GetSalutation(_constituents[0], options);
            Assert.AreEqual("Dear John Q. Public Jr. and Dr. Public:", result, "Missing prefix with no gender forces full name.");
            
        }
    }
}