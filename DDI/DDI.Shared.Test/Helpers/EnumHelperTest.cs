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
        public void When_Given_An_Enumeration_Member_Should_Return_Description_Attribute_Value()
        {
            //Arrange
            string description = string.Empty;

            //Act
            description = EnumHelper.GetDescription(PaymentMethodType.SWIFT);

            //Assert
            Assert.AreEqual(description, "SWIFT Transfer");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_Given_An_Enumeration_Should_Return_Description_Attribute_Of_Members_As_Dictionary_Of_String_And_Int()
        {
            //Arrange
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            //Act
            dictionary = EnumHelper.GetDescriptions<PaymentMethodType>();

            //Assert
            Assert.AreEqual(5, dictionary.Count);
            Assert.AreEqual(0, dictionary.GetValueOrDefault("None"));
            Assert.AreEqual(1, dictionary.GetValueOrDefault("Check"));
            Assert.AreEqual(2, dictionary.GetValueOrDefault("ACH Transfer"));
            Assert.AreEqual(3, dictionary.GetValueOrDefault("Wire Transfer"));
            Assert.AreEqual(4, dictionary.GetValueOrDefault("SWIFT Transfer"));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_GetDescriptions_Is_Passed_Something_Other_Than_Enumeration_Should_Throw_Exception()
        {
            //Arrange
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            Exception exception = null;

            //Act
            try
            {
                dictionary = EnumHelper.GetDescriptions<Constituent>();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            Assert.AreEqual("Type of argument must be Enum.", exception?.Message);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_Enumeration_Without_All_Members_Having_Descriptions_Should_Return_The_Elements_That_Do_Have_Descriptions()
        {
            //Arrange
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            //Act
            dictionary = EnumHelper.GetDescriptions<TestEnumeration>();

            //Assert
            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual(2, dictionary.GetValueOrDefault("Number Two"));
            Assert.AreEqual(3, dictionary.GetValueOrDefault("Number Three"));
            Assert.AreEqual(4, dictionary.GetValueOrDefault("Number Four"));
        }

    }
}
