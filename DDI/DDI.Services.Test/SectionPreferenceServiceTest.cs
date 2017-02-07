using System.Collections.Generic;
using System.Linq;
using DDI.Data; 
using DDI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;

namespace DDI.Services.Tests.Services
{
    [TestClass]
    public class SectionPreferenceServiceTest
    {
        private const string TESTDESCR = "WebApi | Services";

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetPreferencesBySectionName_ReturnsConstituentPreferences()
        {
            var repo = new Mock<IRepository<SectionPreference>>();
            var unitOfWork = new UnitOfWorkNoDb();
            unitOfWork.SetRepository(repo.Object);
            repo.Setup(r => r.Entities).Returns(SetupRepositorySectionPreferences());

            var service = new SectionPreferenceService(unitOfWork);

            var result = service.GetPreferencesBySectionName("Constituent");

            Assert.IsTrue(result.Data.Count == 2);
        }

        private IQueryable<SectionPreference> SetupRepositorySectionPreferences()
        {
            var preferences = new List<SectionPreference>();
            preferences.Add(new SectionPreference { SectionName = "Constituent", Name = "Color", Value = "Red" });
            preferences.Add(new SectionPreference { SectionName = "Constituent", Name = "FunFactor", Value = "4" });
            preferences.Add(new SectionPreference { SectionName = "Loans", Name = "Color", Value = "Blue" });

            var response = preferences.AsQueryable<SectionPreference>();
            return response;
        }
    }
}
