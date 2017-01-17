using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DDI.Shared.Enums;
using DDI.Shared.Helpers;
using DDI.Shared.ModuleInfo;
using DDI.Shared.ModuleInfo.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test.Extensions
{
    [TestClass]
    public class DateTimeExtensionsTests
    {
        private const string TESTDESCR = "Shared | Extensions";

        [TestMethod, TestCategory(TESTDESCR)]
        public void DateTimeExtensions_IsValidSQLDate()
        {
            DateTime validDate = DateTime.Now;
            DateTime invalidDate = new DateTime(1753, 1, 1).AddDays(-1);

            Assert.IsTrue(validDate.IsValidSQLDate(), "Today is a valid SQL date.");
            Assert.IsFalse(invalidDate.IsValidSQLDate(), "Prior to 1/1/1753 is invalid SQL date.");
            Assert.IsFalse(default(DateTime?).IsValidSQLDate(), "Null is not a valid SQL date.");
        }

    }


}
