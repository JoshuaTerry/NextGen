using System.Collections.Generic;
using DDI.Shared.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test.Extensions
{
    [TestClass]
    public class DictionaryExtensionsTests
    {
        private const string TESTDESCR = "Shared | Extensions";

        [TestMethod, TestCategory(TESTDESCR)]
        public void DictionaryExtensions_GetValueOrDefault()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                { "A", "Apple" },
                { "B", "Boy" }
            };

            Assert.AreEqual("Apple", dictionary.GetValueOrDefault("A"), "A/Apple returned correctly.");
            Assert.IsNull(dictionary.GetValueOrDefault("C"), "C not found, returns null.");
            Assert.AreEqual("", dictionary.GetValueOrDefault("C", ""), "C not found, returns provided default value.");
        }

    }


    }
