using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace DDI.Business.Tests.Helpers
{
    [TestClass]
    public class LinkedEntityHelperTest
    {

        private const string TESTDESCR = "Business | Helpers";

        private UnitOfWorkNoDb _uow;
        private List<Constituent> constituents;
        private List<TestNote> notes;

        [TestInitialize]
        public void Initialize()
        {
            constituents = new List<Constituent>();
            constituents.Add(new Constituent() { ConstituentNumber = 1, Name = "John", Id = Guid.NewGuid() });
            constituents.Add(new Constituent() { ConstituentNumber = 2, Name = "Jane", Id = Guid.NewGuid() });
            constituents.Add(new Constituent() { ConstituentNumber = 3, Name = "Jim", Id = Guid.NewGuid() });

            _uow = new UnitOfWorkNoDb();
            _uow.CreateRepositoryForDataSource(constituents.AsQueryable());

            notes = new List<TestNote>();
            string entityType = LinkedEntityHelper.GetEntityTypeName(typeof(Constituent));
            notes.Add(new TestNote() { EntityType = entityType, Id = Guid.NewGuid(), ParentEntityId = constituents[1].Id });
            _uow.CreateRepositoryForDataSource(notes.AsQueryable());
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void LinkedEntityHelper_GetEntityTypeName()
        {
            Assert.AreEqual("Constituent", LinkedEntityHelper.GetEntityTypeName(typeof(Constituent)));
            Assert.AreEqual("Country", LinkedEntityHelper.GetEntityTypeName<DDI.Shared.Models.Common.Country>());
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void LinkedEntityHelper_GetParentEntity()
        {
            EntityBase parent = LinkedEntityHelper.GetParentEntity(notes[0], _uow);
            Assert.IsNotNull(parent, "GetParentEntity returned non-null value.");
            Assert.IsInstanceOfType(parent, typeof(Constituent), "GetParentEntity returned a constituent.");
            Assert.AreEqual(constituents[1].Id, parent.Id, "GetParentEntity returned correct constituent.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void LinkedEntityHelper_SetParentEntity()
        {
            TestNote note = notes[0];
            Address parent = new Address() { Id = Guid.NewGuid() };
            LinkedEntityHelper.SetParentEntity(note, parent);

            Assert.AreEqual(parent.Id, note.ParentEntityId, "SetParentEntity set ParentEntityId correctly.");
            Assert.AreEqual("Address", note.EntityType, "SetParentEntity set EntityType correctly.");

            LinkedEntityHelper.SetParentEntity(note, null);
            Assert.IsNull(note.ParentEntityId, "SetParentEntity cleared ParentEntityID.");
            Assert.IsNull(note.EntityType, "SetParentEntity cleared EntityType.");

        }

        // Class to simulate a note (i.e. because we don't have a Note entity defined yet)            
        private class TestNote : LinkedEntityBase
        {
            public override Guid Id { get; set; }

        }
    }
}
