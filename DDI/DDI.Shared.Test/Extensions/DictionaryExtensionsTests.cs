using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test.Extensions
{
    [TestClass]
    public class DictionaryExtensionsTests
    {
        private const string TESTDESCR = "Shared | Extensions";

        //[TestMethod, TestCategory(TESTDESCR)]
        //public void DictionaryExtensions_AddOrUpdate()
        //{
        //    Dictionary<string, string> dictionary = new Dictionary<string, string>()
        //    {
        //        { "A", "Apple" },
        //        { "B", "Boy" }
        //    };

        //    var returnValue = dictionary.AddOrUpdate("C", "Cat");
        //    dictionary.AddOrUpdate("B", "Ball");

        //    Assert.AreEqual("Cat", dictionary["C"], "C/Cat was added.");
        //    Assert.AreEqual("Ball", dictionary["B"], "B/Boy updated to Ball.");
        //    Assert.AreSame(dictionary, returnValue, "AddOrUpdate returned self.");
        //}

        //[TestMethod, TestCategory(TESTDESCR)]
        //public void DictionaryExtensions_GetValueOrDefault()
        //{
        //    Dictionary<string, string> dictionary = new Dictionary<string, string>()
        //    {
        //        { "A", "Apple" },
        //        { "B", "Boy" }
        //    };

        //    Assert.AreEqual("Apple", dictionary.GetValueOrDefault("A"), "A/Apple returned correctly.");
        //    Assert.IsNull(dictionary.GetValueOrDefault("C"), "C not found, returns null.");
        //    Assert.AreEqual("", dictionary.GetValueOrDefault("C", ""), "C not found, returns provided default value.");
        //}

    }


    }
