using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.Core;
using DDI.Shared;
using DDI.Shared.Enums;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace DDI.Business.Tests.Core
{
    [TestClass]
    public class ConfigurationBaseTest : TestBase
    {

        private const string TESTDESCR = "Business | Core";

        const string idBlueString = "01000000-0000-0000-0000-000000000000";
        const string idRedString = "02000000-0000-0000-0000-000000000000";
        const string idGreenString = "03000000-0000-0000-0000-000000000000";

        private Guid idBlue = Guid.Parse(idBlueString);
        private Guid idRed = Guid.Parse(idRedString);
        private Guid idGreen = Guid.Parse(idGreenString);

        private IUnitOfWork _uow;
        private List<Ethnicity> datasource;

        [TestInitialize]
        public void Initialize()
        {
            Factory.ConfigureForTesting();

            datasource = new List<Ethnicity>();
            datasource.Add(new Ethnicity() { Code = "B", Name = "Blue", Id = idBlue });
            datasource.Add(new Ethnicity() { Code = "R", Name = "Red", Id = idRed });
            datasource.Add(new Ethnicity() { Code = "G", Name = "Green", Id = idGreen });

            _uow = Factory.CreateUnitOfWork();
            _uow.CreateRepositoryForDataSource(datasource.AsQueryable());
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ConfigurationBase_GetGuidStrings()
        {
            var configuration = new TestConfiguration();
            configuration.Ethnicities = new List<Ethnicity>();
            configuration.Ethnicities.Add(_uow.GetById<Ethnicity>(idRed));
            configuration.Ethnicities.Add(_uow.GetById<Ethnicity>(idBlue));

            string result = configuration.GetGuidStrings(configuration.Ethnicities);

            Assert.AreEqual($"{idRed},{idBlue}", result);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ConfigurationBase_GetEntity()
        {
            var configuration = new TestConfiguration();
            Ethnicity result = configuration.GetEntity<Ethnicity>(idGreenString, _uow);
            Assert.AreEqual(datasource[2], result, "GetEntity should return valid ethnicity.");
            
            result = configuration.GetEntity<Ethnicity>("Not a valid guid...", _uow);
            Assert.IsNull(result, "GetEntity should return null if invalid guid string is passed.");

            result = configuration.GetEntity<Ethnicity>("", _uow);
            Assert.IsNull(result, "GetEntity should return null for an empty string.");

            result = configuration.GetEntity<Ethnicity>(null, _uow);
            Assert.IsNull(result, "GetEntity should return null for a null.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ConfigurationBase_GetEntityList()
        {
            var configuration = new TestConfiguration();
            List<Ethnicity> result = configuration.GetEntityList<Ethnicity>($"{idRed},{idBlue}", _uow).ToList();
            CollectionAssert.AreEquivalent(new List<Ethnicity>() { datasource[1], datasource[0] }, result, "GetEntityList should convert delimtited guid string to ethnicities.");

            result = configuration.GetEntityList<Ethnicity>($"{idRed},Not a valid guid...", _uow).ToList();
            CollectionAssert.AreEquivalent(new List<Ethnicity>() { datasource[1] }, result, "GetEntityList should ignore invalid guids.");

            result = configuration.GetEntityList<Ethnicity>("", _uow).ToList();
            CollectionAssert.AreEquivalent(new List<Ethnicity>() {  }, result, "GetEntityList should ignore empty strings.");

            result = configuration.GetEntityList<Ethnicity>(null, _uow).ToList();
            CollectionAssert.AreEquivalent(new List<Ethnicity>() { }, result, "GetEntityList should ignore nulls.");

        }

        private class TestConfiguration : ConfigurationBase
        {

            public override ModuleType ModuleType { get; } = ModuleType.None;
            public List<Ethnicity> Ethnicities { get; set; }
        }
    }
}
