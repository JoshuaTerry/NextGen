using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Common;
using DDI.Business.Core;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.ModuleInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DDI.Business.Tests.Core
{
    [TestClass]
    public class ConfigurationLogicTest
    {

        private const string TESTDESCR = "Business | Core";

        const string idBlueString = "01000000-0000-0000-0000-000000000000";
        const string idRedString = "02000000-0000-0000-0000-000000000000";
        const string idGreenString = "03000000-0000-0000-0000-000000000000";

        const string idBaptistString = "04000000-0000-0000-0000-000000000000";
        const string idMethodistString = "05000000-0000-0000-0000-000000000000";

        private Guid idBlue = Guid.Parse(idBlueString);
        private Guid idRed = Guid.Parse(idRedString);
        private Guid idGreen = Guid.Parse(idGreenString);
        private Guid idBaptist = Guid.Parse(idBaptistString);
        private Guid idMethodist = Guid.Parse(idMethodistString);

        private UnitOfWorkNoDb _uow;
        private List<Ethnicity> ethnicityDataSource;
        private List<Denomination> denominationDataSource;

        [TestInitialize]
        public void Initialize()
        {
            ethnicityDataSource = new List<Ethnicity>();
            ethnicityDataSource.Add(new Ethnicity() { Code = "B", Name = "Blue", Id = idBlue });
            ethnicityDataSource.Add(new Ethnicity() { Code = "R", Name = "Red", Id = idRed });
            ethnicityDataSource.Add(new Ethnicity() { Code = "G", Name = "Green", Id = idGreen });

            denominationDataSource = new List<Denomination>();
            denominationDataSource.Add(new Denomination { Code = "B", Name = "Baptist", Id = idBaptist });
            denominationDataSource.Add(new Denomination { Code = "M", Name = "Methodist", Id = idMethodist });

            _uow = new UnitOfWorkNoDb();
            _uow.CreateRepositoryForDataSource(ethnicityDataSource.AsQueryable());
            _uow.CreateRepositoryForDataSource(denominationDataSource.AsQueryable());
            _uow.CreateRepositoryForDataSource(new List<Configuration>().AsQueryable());
        }

        private TestConfiguration BuildTestConfiguration()
        {
            TestConfiguration config = new TestConfiguration()
            {
                Age = 29,
                Day = DayOfWeek.Wednesday,
                Enabled = true,
                EndDate = DateTime.Parse("12/31/1999"),
                TimeStamp = DateTime.Parse("1/1/2000").AddSeconds(14726),
                StartDate = null,

                Title = "Testing"
            };
            config.Denomination = denominationDataSource[0];
            config.Ethnicities = new List<Ethnicity>() { ethnicityDataSource[1], ethnicityDataSource[0] };
            return config;
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ConfigurationLogic_SaveConfiguration()
        {
            // Build a configuration and save it.
            var config = BuildTestConfiguration();
            var configurationLogic = new ConfigurationLogic(_uow);
            configurationLogic.SaveConfiguration(config);

            var configRows = _uow.GetRepository<Configuration>().Entities;

            Configuration row = configRows.FirstOrDefault(p => p.Name == "Title");
            Assert.IsNotNull(row, "Title was saved");
            Assert.AreEqual("Testing", row.Value, "Title saved with correct value.");

            row = configRows.FirstOrDefault(p => p.Name == "Age");
            Assert.IsNotNull(row, "Age was saved");
            Assert.AreEqual("29", row.Value, "Age saved with correct value.");

            row = configRows.FirstOrDefault(p => p.Name == "Day");
            Assert.IsNotNull(row, "Day was saved");
            Assert.AreEqual(((int)DayOfWeek.Wednesday).ToString(), row.Value, "Day saved with correct value.");

            row = configRows.FirstOrDefault(p => p.Name == "Enabled");
            Assert.IsNotNull(row, "Enabled was saved");
            Assert.AreEqual(true.ToString(), row.Value, "Enabled saved with correct value.");

            row = configRows.FirstOrDefault(p => p.Name == "EndDate");
            Assert.IsNotNull(row, "EndDate was saved");
            Assert.AreEqual("12/31/1999", row.Value, "End date saved with correct value.");

            row = configRows.FirstOrDefault(p => p.Name == "StartDate");
            Assert.IsNotNull(row, "StartDate was saved");
            Assert.IsNotNull(row.Value, "Start date value saved as null.");

            row = configRows.FirstOrDefault(p => p.Name == "TimeStamp");
            Assert.IsNotNull(row, "TimeStamp was saved");
            Assert.AreEqual("1/1/2000 4:05 AM", row.Value, "TimeStamp saved with correct value.");

            row = configRows.FirstOrDefault(p => p.Name == "Denomination");
            Assert.IsNotNull(row, "Denomination was saved");
            Assert.AreEqual($"{idBaptist}", row.Value, "TimeStamp saved with correct value.");

            row = configRows.FirstOrDefault(p => p.Name == "Ethnicities");
            Assert.IsNotNull(row, "Ethnicities was saved");
            Assert.AreEqual($"{idRed},{idBlue}", row.Value, "Ethnicities saved with correct value.");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ConfigurationLogic_LoadConfiguration()
        {
            // Build a configuration and save it.
            var configToSave = BuildTestConfiguration();
            var configurationLogic = new ConfigurationLogic(_uow);
            configurationLogic.SaveConfiguration(configToSave);

            // Load the configuration from the saved data (ignoring cache)
            var configToLoad = configurationLogic.GetConfiguration<TestConfiguration>(true);

            Assert.AreEqual(configToSave.Age, configToLoad.Age, "Age loaded correctly.");
            Assert.AreEqual(configToSave.Title, configToLoad.Title, "Title loaded correctly.");
            Assert.AreEqual(configToSave.Day, configToLoad.Day, "Day loaded correctly.");
            Assert.AreEqual(configToSave.Enabled, configToLoad.Enabled, "Enabled loaded correctly.");
            Assert.IsNull(configToLoad.StartDate, "Start date loaded correctly.");
            Assert.AreEqual(configToSave.EndDate, configToLoad.EndDate, "EndDate loaded correctly.");
            Assert.AreEqual(configToSave.TimeStamp, configToLoad.TimeStamp, "TimeStamp loaded correctly.");
            Assert.AreEqual(configToSave.Denomination, configToLoad.Denomination, "Denomination loaded correctly.");
            CollectionAssert.AreEquivalent(configToSave.Ethnicities, configToLoad.Ethnicities, "Ethnicities loaded correctly.");

            // Blow away the Configuration datasource and ensure caching is working.
            ((RepositoryNoDb<Configuration>)_uow.GetRepository<Configuration>()).Clear();

            configToLoad = configurationLogic.GetConfiguration<TestConfiguration>(false);

            Assert.AreEqual(configToSave.Age, configToLoad.Age, "Age cached correctly.");
            Assert.AreEqual(configToSave.Title, configToLoad.Title, "Title cached correctly.");
            Assert.AreEqual(configToSave.Day, configToLoad.Day, "Day cached correctly.");
            Assert.AreEqual(configToSave.Enabled, configToLoad.Enabled, "Enabled cached correctly.");
            Assert.IsNull(configToLoad.StartDate, "Start date cached correctly.");
            Assert.AreEqual(configToSave.EndDate, configToLoad.EndDate, "EndDate cached correctly.");
            Assert.AreEqual(configToSave.TimeStamp, configToLoad.TimeStamp, "TimeStamp cached correctly.");
            Assert.AreEqual(configToSave.Denomination, configToLoad.Denomination, "Denomination cached correctly.");
            CollectionAssert.AreEquivalent(configToSave.Ethnicities, configToLoad.Ethnicities, "Ethnicities cached correctly.");

            // Ensure loading a configuration that was never saved returns a valid configuration with default values.
            var emptyConfig = configurationLogic.GetConfiguration<AccountingConfiguration>();
            Assert.IsNotNull(emptyConfig, "Accounting configuration loaded even though not saved.");
            Assert.IsNull(emptyConfig.Title, "Accounting configuration has default title.");
        }


        [ModuleType(Shared.Enums.ModuleType.CRM)]
        private class TestConfiguration : ConfigurationBase
        {
            // Some random configuration properties

            public string Title { get; set; }
            public bool Enabled { get; set; }
            public int Age { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public DateTime? TimeStamp { get; set; }
            public DayOfWeek Day { get; set; }
            public Denomination Denomination { get; set; }
            public List<Ethnicity> Ethnicities { get; set; }

            public override string SaveProperty(string name)
            {
                if (name == nameof(Ethnicities))
                {
                    return GetGuidStrings(Ethnicities);
                }
                return null;
            }

            public override void LoadProperty(string name, string value, IUnitOfWork uow)
            {
                if (name == nameof(Ethnicities))
                {
                    Ethnicities = GetEntityList<Ethnicity>(value, uow).ToList();
                }
            }
        }

        [ModuleType(Shared.Enums.ModuleType.Accounting)]
        private class AccountingConfiguration : ConfigurationBase
        {
            public string Title { get; set; }
        }
    }
}
