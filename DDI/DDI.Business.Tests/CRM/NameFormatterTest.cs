using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Business.Tests.CRM.DataSources;
using DDI.Data;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.CRM
{
    [TestClass]
    public class NameFormatterTest
    {

        private const string TESTDESCR = "Business | CRM";
        private UnitOfWorkNoDb _uow;
        private List<Constituent> _constituents;
        private List<Relationship> _relationships;
        private IList<ConstituentType> _constituentTypes;
        private IList<Gender> _genders;
        private IList<Prefix> _prefixes;
        private IList<ConstituentStatus> _statuses;
        private IList<RelationshipType> _relationshipTypes;
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

            CRMConfigurationDataSource.GetDataSource(_uow);

            ConstituentType individualType = _constituentTypes.FirstOrDefault(p => p.Category == ConstituentCategory.Individual);

            // Set up two constituents with a spouse relationship.

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
                Id = Guid.NewGuid()
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
                Id = Guid.NewGuid()
            });

            _constituents.Add(new Constituent()
            {
                ConstituentNumber = 3,
                Name = "DiscipleData Inc.",                
                ConstituentType = _constituentTypes.FirstOrDefault(p => p.Code == ConstituentTypeCodes.Organization),
                ConstituentStatus = _statuses.FirstOrDefault(p => p.BaseStatus == ConstituentBaseStatus.Active),
                Id = Guid.NewGuid()
            });

            _uow.CreateRepositoryForDataSource(_constituents);

            // Relationships

            _relationships = new List<Relationship>();
            _relationships.Add(new Relationship()
            {
                Constituent1 = _constituents[0],
                Constituent2 = _constituents[1],
                RelationshipType = _relationshipTypes.FirstOrDefault(p => p.Code == "SPOU"),
                Id = Guid.NewGuid()
            });

            _uow.CreateRepositoryForDataSource(_relationships);

            _constituents[0].Relationship1s = _relationships.Where(p => p.Constituent1 == _constituents[0]).ToList();
            _constituents[0].Relationship2s = _relationships.Where(p => p.Constituent2 == _constituents[0]).ToList();

            _constituents[1].Relationship1s = _relationships.Where(p => p.Constituent1 == _constituents[1]).ToList();
            _constituents[1].Relationship2s = _relationships.Where(p => p.Constituent2 == _constituents[1]).ToList();

            _logic = new NameFormatter(_uow);

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
        public void NameFormatter_BuildIndividualNameLines()
        {
            string line1, line2;

            _logic.BuildIndividualNameLines(_constituents[0], null, LabelRecipient.Primary, false, false, false, 0, out line1, out line2);

            Assert.AreEqual("Mr. John Q. Public Jr.", line1, "Default formatting");
            // Further tests are in BuildNameLines which calls same logic as BuildIndividualNameLines.
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void NameFormatter_BuildNameLines_Organization()
        {
            string line1, line2;
            NameFormattingOptions options = new NameFormattingOptions();

            _logic.BuildNameLines(_constituents[2], null, options, out line1, out line2);
            Assert.AreEqual(_constituents[2].Name, line1, "Default formating - organization");

            _constituents[2].Name2 = "ATTN: Treasurer";

            _logic.BuildNameLines(_constituents[2], null, options, out line1, out line2);
            Assert.AreEqual(_constituents[2].Name2, line2, "Organization with Name2");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void NameFormatter_BuildNameLines_Individual()
        {
            string line1, line2;
            NameFormattingOptions options = new NameFormattingOptions();            

            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. Public Jr.", line1, "Default formating - individual");

            options.OmitPrefix = true;
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("John Q. Public Jr.", line1, "Omit prefix");

            options.OmitPrefix = false;
            options.Recipient = LabelRecipient.Both;
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. and Mrs. John Q. Public Jr.", line1, "Recipient=Both");

            options.Recipient = LabelRecipient.Secondary;
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mrs. Jane E. Public", line1, "Recipient=Secondary");

            options.Recipient = LabelRecipient.Wife;
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mrs. Jane E. Public", line1, "Recipient=Wife");

            options.Recipient = LabelRecipient.Husband;
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. Public Jr.", line1, "Recipient=Husband");

            options.Recipient = LabelRecipient.Both;
            options.OmitPrefix = true;
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("John Q. Public Jr. and Jane E. Public", line1, "Recipient=Both, omit prefixes");

            options.OmitPrefix = false;
            options.KeepSeparate = true;
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. Public Jr. and Mrs. Jane E. Public", line1, "Recipient=Both, keep separate");

            options.KeepSeparate = false;
            options.AddFirstNames = true;
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. and Mrs. Jane E. Public", line1, "Recipient=Both, add first names");

            options.MaxChars = 20;
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Mr. John Q. and", line1, "Recipient=Both, max chars line 1");
            Assert.AreEqual("Mrs. Jane E. Public", line2, "Recipient=Both, max chars line 2");
            
            options.AddFirstNames = false;
            options.MaxChars = 0;

            //Need some prefix changes.
            _constituents[0].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Dr");
            _constituents[1].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Dr");
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Drs. John Q. and Jane E. Public", line1, "Recipient=Both, pluralize prefix");

            _constituents[0].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Law");
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("John Q. Public Jr., Esq. and Dr. Jane E. Public", line1, "Recipient=Both, non-combining prefix");

            _constituents[0].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Mr");
            _constituents[1].Prefix = _prefixes.FirstOrDefault(p => p.Code == "Law");
            _logic.BuildNameLines(_constituents[0], null, options, out line1, out line2);
            Assert.AreEqual("Jane E. Public, Esq. and Mr. John Q. Public Jr.", line1, "Recipient=Both, swap names");

        }

    }
}