using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DDI.Shared.Enums;
using DDI.Shared.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DDI.Shared.Extensions;

namespace DDI.Shared.Test.Extensions
{
    [TestClass]
    public class MemberInfoExtensionsTests
    {
        private const string TESTDESCR = "Shared | Extensions";

        [TestMethod, TestCategory(TESTDESCR)]
        public void MemberInfoExtensions_GetAttribute()
        {
            MemberInfo titleProperty = typeof(MyClass).GetProperty("Title");
            var defaultValueAttribute = titleProperty.GetAttribute<DefaultValueAttribute>();

            Assert.IsNotNull(defaultValueAttribute, "GetAttribute found DefaultValue attribute.");
            Assert.AreEqual("test", defaultValueAttribute.Value, "GetAttribute returned correct attribute.");

            Assert.IsNull(titleProperty.GetAttribute<BrowsableAttribute>(), "Title does not have browsable attribute.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void MemberInfoExtensions_GetAttributes()
        {
            MemberInfo subTitle = typeof(MyClass).GetProperty("Subtitle");

            var attributes = subTitle.GetAttributes<PaintColorAttribute>().ToList();
            Assert.IsNotNull(attributes, "GetAttributes returned a value.");
            Assert.AreEqual(2, attributes.Count, "Subtitle has 2 attribute instances.");
            CollectionAssert.AllItemsAreInstancesOfType(attributes, typeof(PaintColorAttribute));
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void MemberInfoExtensions_IsAttributeDefined()
        {
            Assert.IsTrue(typeof(MemberInfoExtensionsTests).IsAttributeDefined<TestClassAttribute>(), "Test class has TestClass attribute defined.");
        }
        

        private class MyClass
        {
            [DefaultValue("test")]
            public string Title { get; set; }

            [PaintColor(ConsoleColor.Black), PaintColor(ConsoleColor.Blue)]
            public string Subtitle { get; set; }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        private class PaintColorAttribute : Attribute
        {            
            public ConsoleColor Color { get; private set; }
            public PaintColorAttribute(ConsoleColor color)
            {
                Color = color;
            }
        }
    }


}
