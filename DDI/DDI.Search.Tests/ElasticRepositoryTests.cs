using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDI.Search.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DDI.Search.Tests
{
    [TestClass]
    public class ElasticRepositoryTests
    {
        private NestClient _client;

        private const string TESTDESCR = "Search";

        [TestInitialize]
        public void Initialize()
        {
            _client = new NestClient("http://localhost:9200", "test");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void SimpleMatchQuery()
        {
            var repo = new ElasticRepository<ConstituentDocument>(_client);
            var query = repo.CreateQuery();
            query.Must.Match(p => p.Name, "Smith");

            JObject result = JObject.Parse(repo.GetQueryJsonBody(query));

            var boolQuery = Find(result, "bool");
            Assert.IsNotNull(boolQuery, "Json body contains 'bool' query.");

            var mustQuery = Find(result, "must");
            Assert.IsNotNull(mustQuery, "Json body contains 'must' query.");

            var nameTerm = Find(mustQuery, "name");
            Assert.IsNotNull(nameTerm, "Json body contains 'name' term.");

            var nameValue = Find(nameTerm, "query");
            Assert.AreEqual("Smith", nameValue.Value<string>(), "'name' term contains search string.");                       
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetSearchUri()
        {
            var repo = new ElasticRepository<ConstituentDocument>(_client);
            Uri uri = repo.GetSearchUri();
            Assert.IsTrue(uri.ToString().EndsWith("/constituent/_search"), "Search Uri formatted correctly.");
        }

        /// <summary>
        /// Recursively search through Json to find a JProperty with specified name and return the property's value.
        /// </summary>
        private JToken Find(JToken parent, string name)
        {
            foreach (JToken entry in parent.Children())
            {
                if (entry is JProperty && ((JProperty)entry).Name == name)
                {
                    return ((JProperty)entry).Value;
                }
                JToken result = Find(entry, name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
