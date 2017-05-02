using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Extensions;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace DDI.Shared.Test.Helpers
{
    [TestClass]
    public class EnumHelperTest
    {
        private const string TESTDESCR = "Shared | Helpers";
        public enum TestEnumeration
        {
            Zero = 0,

            One = 1,

            [Description("Number Two")]
            Two = 2,

            [Description("Number Three")]
            Three = 3,

            [Description("Number Four")]
            Four = 4
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void EnumHelper_ConvertToEnum()
        {
            Assert.AreEqual(PaymentMethodType.Check, EnumHelper.ConvertToEnum<PaymentMethodType>("Check"));
            Assert.AreEqual(PaymentMethodType.ACH, EnumHelper.ConvertToEnum<PaymentMethodType>("ACH"));
            Assert.AreEqual(PaymentMethodType.ACH, EnumHelper.ConvertToEnum<PaymentMethodType>("ACH Transfer"));
            Assert.AreEqual(PaymentMethodType.ACH, EnumHelper.ConvertToEnum<PaymentMethodType>("2"));
            Assert.AreEqual(PaymentMethodType.ACH, EnumHelper.ConvertToEnum<PaymentMethodType>(2));
            Assert.AreEqual(PaymentMethodType.ACH, EnumHelper.ConvertToEnum<PaymentMethodType>(PaymentMethodType.ACH));
            Assert.AreEqual(PaymentMethodType.None, EnumHelper.ConvertToEnum<PaymentMethodType>(null));
            Assert.AreEqual(PaymentMethodType.ACH, EnumHelper.ConvertToEnum<PaymentMethodType>(null, PaymentMethodType.ACH));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void EnumHelper_GetDescription()
        {
            //Arrange
            string description = string.Empty;

            //Act
            description = EnumHelper.GetDescription(PaymentMethodType.SWIFT);

            //Assert
            Assert.AreEqual(description, "SWIFT Transfer");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void EnumHelper_GetDescriptions()
        {
            Dictionary<string, int> dictionary = EnumHelper.GetDescriptions<PaymentMethodType>();

            Assert.AreEqual(5, dictionary.Count);
            Assert.AreEqual(0, dictionary.GetValueOrDefault("None"));
            Assert.AreEqual(1, dictionary.GetValueOrDefault("Check"));
            Assert.AreEqual(2, dictionary.GetValueOrDefault("ACH Transfer"));
            Assert.AreEqual(3, dictionary.GetValueOrDefault("Wire Transfer"));
            Assert.AreEqual(4, dictionary.GetValueOrDefault("SWIFT Transfer"));

            dictionary = EnumHelper.GetDescriptions<TestEnumeration>();
            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual(2, dictionary.GetValueOrDefault("Number Two"));
            Assert.AreEqual(3, dictionary.GetValueOrDefault("Number Three"));
            Assert.AreEqual(4, dictionary.GetValueOrDefault("Number Four"));

            Exception exception = null;

            try
            {
                dictionary = EnumHelper.GetDescriptions<Constituent>();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.AreEqual("Type of argument must be Enum.", exception?.Message);


        }      

    }
}
