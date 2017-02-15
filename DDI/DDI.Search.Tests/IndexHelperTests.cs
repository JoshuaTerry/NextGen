using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDI.Search.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Search.Tests
{
    [TestClass]
    public class IndexHelperTests
    {

        private const string TESTDESCR = "Search";
        private NestClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _client = new NestClient("http://localhost:9200", "test");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetIndexAlias()
        {
            Assert.AreEqual("test_crm", IndexHelper.GetIndexAlias<ConstituentDocument>(), "GetIndexAlias for ConstituentDocument");

            Assert.AreEqual("test_crm", IndexHelper.GetIndexAlias("crm"), "GetIndexAlias for crm");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetIndexName()
        {
            string indexName = IndexHelper.GetIndexName<ConstituentDocument>();
            Assert.IsTrue(indexName.StartsWith("test_crm_"), "ConstituentDocument index name starts with test_crm");

            var parts = indexName.Split('_');
            Assert.IsTrue(parts.Length >= 3, "ConstituentDocument index name has >= 3 parts.");

            indexName = IndexHelper.GetIndexName("crm");
            Assert.IsTrue(indexName.StartsWith("test_crm_"), "CRM index name starts with test_crm");

            parts = indexName.Split('_');
            Assert.IsTrue(parts.Length >= 3, "CRM index name has >= 3 parts.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetTypesForIndexSuffix()
        {
            var types = IndexHelper.GetTypesForIndexSuffix("crm").ToList();

            CollectionAssert.Contains(types, typeof(ConstituentDocument), "CRM types includes ConstituentDocument");
            CollectionAssert.Contains(types, typeof(AddressDocument), "CRM types includes AddressDocument");
            CollectionAssert.Contains(types, typeof(ContactInfoDocument), "CRM types includes ContactInfoDocument");

        }

    }
}
