using System;
using System.Collections.Generic;
using DDI.Shared.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace DDI.Shared.Test.Extensions
{
    [TestClass]
    public class JsonExtensionsTest
    {
        private const string TESTDESCR = "Shared | Extensions";

        public class TestObject
        {
            public string Test1 { get; set; }
            public string Test2 { get; set; }
            public int? Test3 { get; set; }
            public DateTime? Test4 { get; set; }
            public DateTime? Test5 { get; set; }
            public int[] Test6 { get; set; }
        }
         
        [TestMethod, TestCategory(TESTDESCR)]
        public void JsonExtensions_ConvertToType()
        {
            // Arrange
            var item = new 
            {
                Test1 = "Test1",
                Test2 = string.Empty,
                Test3 = 5,
                Test4 = new DateTime(2017, 1, 31),
                Test5 = "",
                Test6 = new int[] {}
            };
            JObject jObject = JObject.FromObject(item);

            // Act
            Dictionary<string, object> changedProperties = new Dictionary<string, object>();
            foreach (var pair in jObject)
            {
                var convertedPair = JsonExtensions.ConvertToType<TestObject>(pair);
                changedProperties.Add(convertedPair.Key, convertedPair.Value);
            }

            //Assert
            Assert.AreEqual(6, changedProperties.Count);

            var one = changedProperties.GetValueOrDefault("Test1");
            Assert.IsTrue(one is string);
            Assert.AreEqual("Test1", changedProperties.GetValueOrDefault("Test1"));

            var two = changedProperties.GetValueOrDefault("Test2");
            Assert.IsTrue(two is string);
            Assert.AreEqual(string.Empty, changedProperties.GetValueOrDefault("Test2"));

            var three = changedProperties.GetValueOrDefault("Test3");
            Assert.IsTrue(three is int?);
            Assert.AreEqual(5, changedProperties.GetValueOrDefault("Test3"));

            var four = changedProperties.GetValueOrDefault("Test4");
            Assert.IsTrue(four is DateTime?);
            Assert.AreEqual(new DateTime(2017, 1, 31), changedProperties.GetValueOrDefault("Test4"));

            var five = changedProperties.GetValueOrDefault("Test5");
            Assert.IsNull(five);

            var six = changedProperties.GetValueOrDefault("Test6");
            Assert.AreEqual(((int[]) six).Length, 0);
        }
    }
}
