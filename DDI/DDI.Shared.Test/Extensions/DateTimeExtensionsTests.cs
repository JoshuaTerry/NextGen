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

        [TestMethod, TestCategory(TESTDESCR)]
        public void DateTimeExtensions_ToRoundTripString()
        {
            DateTime dt = DateTime.SpecifyKind(new DateTime(2014, 6, 1).AddSeconds(47205).AddTicks(4196823), DateTimeKind.Utc);
            DateTime? dtn = null;
            const string EXPECTED_RESULT = "2014-06-01T13:06:45.4196823Z";

            Assert.AreEqual(string.Empty, dtn.ToRoundTripString(), "Null date returns blank.");
            Assert.AreEqual(EXPECTED_RESULT, dt.ToRoundTripString(), "Returns correct formatted value.");

            dtn = dt;
            Assert.AreEqual(EXPECTED_RESULT, dtn.ToRoundTripString(), "Test for nullable version.");

            dt = dt = DateTime.SpecifyKind(new DateTime(2014, 6, 1).AddSeconds(47205).AddTicks(4196823), DateTimeKind.Unspecified);
            Assert.AreEqual(EXPECTED_RESULT, dt.ToRoundTripString(), "Assumes UTC if kind is unspecified");
        }


    }


}
