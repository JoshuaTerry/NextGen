using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DDI.Shared.Enums;
using DDI.Shared.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test.Extensions
{
    [TestClass]
    public class GuidExtensionsTests
    {
        private const string TESTDESCR = "Shared | Extensions";

        [TestMethod, TestCategory(TESTDESCR)]
        public void GuidExtensions_IsEmpty()
        {
            Assert.IsTrue(Guid.Empty.IsEmpty(), "Empty guid should return true.");
            Assert.IsFalse(Guid.Parse("{1DB4F9E5-639A-46D9-950A-9C347FB4C765}").IsEmpty(), "Non-empty guid should return false.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GuidExtensions_IsNullOrEmpty()
        {
            Guid? guid = null;
            Assert.IsTrue(guid.IsNullOrEmpty(), "Null value should return true.");

            guid = Guid.Empty;
            Assert.IsTrue(guid.IsNullOrEmpty(), "Empty guid should return true.");

            guid = Guid.Parse("{1DB4F9E5-639A-46D9-950A-9C347FB4C765}");
            Assert.IsFalse(guid.IsNullOrEmpty(), "Non-empty guid should return false.");
        }


    }


}
