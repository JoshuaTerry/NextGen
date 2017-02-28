using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DDI.Shared.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting; 
using DDI.Shared.Models;

namespace DDI.Shared.Test.Helpers
{
    [TestClass]
    public class ReflectionHelperTests
    {
        private const string TESTDESCR = "Shared | Helpers";

        

        [TestMethod, TestCategory(TESTDESCR)]
        public void Reflectionhelper_GetDecoratedWith_NotNull()
        {
            var types = ReflectionHelper.GetDecoratedWith<TestClassAttribute>();
            Assert.IsNotNull(types, "GetDecoratedWith returned non-null value");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void Reflectionhelper_GetDecoratedWith_MultipleValues()
        {
            var types = ReflectionHelper.GetDecoratedWith<TestClassAttribute>();
            Assert.IsTrue(types.Count > 1, "GetDecoratedWith returned multiple results");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ReflectionHelper_GetDerivedTypes_NotNull()
        {
            var types = ReflectionHelper.GetDerivedTypes<EntityBase>();
            Assert.IsNotNull(types, "GetDerivedTypes returned non-null value");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ReflectionHelper_GetDerivedTypes_MultipleValues()
        {
            var types = ReflectionHelper.GetDerivedTypes<EntityBase>();
            Assert.IsTrue(types.Count > 1, "GetDerivedTypes returned multiple results.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ReflectionHelper_GetImplementingTypes()
        {
            var types = ReflectionHelper.GetImplementingTypes<IDoesSomething>();
            Assert.IsNotNull(types, "GetImplementingTypes returned non-null value.");
            Assert.IsTrue(types.Count == 2, "GetImplementingTypes returned multiple results.");
            CollectionAssert.Contains(types, typeof(MyClass), "GetImplementingTypes contains MyClass.");
            CollectionAssert.Contains(types, typeof(MySubClass), "GetImplementingTypes contains MySubClass.");
        }

        
        [TestMethod, TestCategory(TESTDESCR)]
        public void ReflectionHelper_GetInstancesOf()
        {
            var instances = ReflectionHelper.GetInstancesOf<IDoesSomething>(typeof(ReflectionHelperTests).Assembly);
            Assert.IsNotNull(instances, "GetInstancesOf returned non-null value.");
            Assert.IsTrue(instances.Count == 2, "GetInstancesOf returned multiple results.");
            CollectionAssert.AllItemsAreInstancesOfType(instances, typeof(IDoesSomething), "GetInstancesOf returned instances of IDoesSomething.");
        }        

        // Interfaces and classes used for these tests.
        private interface IDoesSomething
        {
        }

        private class MyClass : IDoesSomething { }
        private class MySubClass : MyClass { }
    }
}