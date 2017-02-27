using System;
using System.Linq;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Data.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        private const string TESTDESCR = "Data";

        [TestMethod,TestCategory(TESTDESCR)]
        public void Repository_NameFor()
        {
            var result = Repository<Constituent>.NameFor<Constituent>(c => c.ConstituentAddresses, true);
            Assert.AreEqual("ConstituentAddresses", result);
            result = Repository<Constituent>.NameFor<Constituent>(c => c.Language.Code, true);
            Assert.AreEqual("Language.Code", result);
            result = Repository<Constituent>.NameFor<Constituent>(c => c.ConstituentAddresses.First().Address, true);
            Assert.AreEqual("ConstituentAddresses.Address", result);
        }
    }
}
