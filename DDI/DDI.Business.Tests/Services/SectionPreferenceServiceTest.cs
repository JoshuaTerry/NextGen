using DDI.Business.Services;
using DDI.Data;
using DDI.Data.Models.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Business.Tests.Services
{
    [TestClass]
    public class SectionPreferenceServiceTest
    {
        [TestMethod]
        public void GetPreferencesBySectionName_ReturnsConstituentPreferences()
        { 
            var repo = new Mock<IRepository<SectionPreference>>();
            repo.Setup(r => r.Entities).Returns(SetupRepositorySectionPreferences());
            var service = new SectionPreferenceService(repo.Object);
            var result = service.GetPreferencesBySectionName("Constituent");

            Assert.IsTrue(result.Data.Count == 2);
        }

        private IQueryable<SectionPreference> SetupRepositorySectionPreferences()
        {
            var preferences = new List<SectionPreference>();
            preferences.Add(new SectionPreference { SectionName = "Constituent", Name = "Color", Value = "Red" });
            preferences.Add(new SectionPreference { SectionName = "Constituent", Name = "FunFactor", Value = 4 });
            preferences.Add(new SectionPreference { SectionName = "Loans", Name = "Color", Value = "Blue" });

            var response = preferences.AsQueryable<SectionPreference>();
            return response;
        }
    }
}
