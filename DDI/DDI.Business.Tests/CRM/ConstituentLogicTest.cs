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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.CRM
{
    [TestClass]
    public class ConstituentLogicTest
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
        private ConstituentLogic _bl;

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
                ConstituentType = individualType,
                ConstituentStatus = _statuses.FirstOrDefault(p => p.BaseStatus == ConstituentBaseStatus.Active),
                Gender = _genders.FirstOrDefault(p => p.Code == "M"),
                Id = GuidHelper.NextGuid()
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
                Id = GuidHelper.NextGuid()
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

            _bl = new ConstituentLogic(_uow);

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ConstituentLogic_GetSortName()
        {
            Assert.AreEqual("Public John Q.", _bl.GetSortName(_constituents[0]));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ConstituentLogic_GetFormattedName()
        {
            Assert.AreEqual("John Q. Public Jr.", _bl.GetFormattedName(_constituents[0]));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ConstituentLogic_GetSpouse()
        {
            Constituent other = _bl.GetSpouse(_constituents[0]);
            Assert.IsNotNull(other, "GetSpouse 1 returned valid result.");
            Assert.AreEqual(2, other.ConstituentNumber, "GetSpouse 1 returned constituent 2.");

            other = _bl.GetSpouse(_constituents[1]);
            Assert.IsNotNull(other, "GetSpouse 2 returned valid result.");
            Assert.AreEqual(1, other.ConstituentNumber, "GetSpouse 2 returned constituent 1.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ConstituentLogic_IsConstituentActive()
        {
            Assert.IsTrue(_bl.IsConstituentActive(_constituents[0]), "Constituent 1 is active.");
        }


    }
}