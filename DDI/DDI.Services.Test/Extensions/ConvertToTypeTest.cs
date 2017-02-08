﻿using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace DDI.Services.Test.Extensions
{
    [TestClass]
    public class ConvertToTypeTest
    {
        public class TestObject
        {
            public string Test1 { get; set; }
            public string Test2 { get; set; }
            public int? Test3 { get; set; }
            public DateTime? Test4 { get; set; }
            public DateTime? Test5 { get; set; }
        }
         
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange2
            var item = new TestObject()
            {
                Test1 = "Test1",
                Test2 = string.Empty,
                Test3 = 5,
                Test4 = new DateTime(2017, 1, 31),
                Test5 = null
            };
            JObject jObject = JObject.FromObject(item);

            // Act
            Dictionary<string, object> changedProperties = new Dictionary<string, object>();
            foreach (var pair in jObject)
            {
                changedProperties.Add(JsonExtensions.ConvertToType<TestObject>(pair).Key, JsonExtensions.ConvertToType<TestObject>(pair).Value);
            }

            //Assert
            Assert.AreEqual(5, changedProperties.Count);

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

            //var five = changedProperties.GetValueOrDefault("Test5");
            //Assert.IsTrue(five is DateTime?);
            //Assert.AreEqual(null, changedProperties.GetValueOrDefault("Test5"));
        }
    }
}
