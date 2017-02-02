using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DDI.Shared.Enums.CRM;
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
            description = EnumHelper.GetDescription(PaymentMethod.SWIFT);

            //Assert
            Assert.AreEqual(description, "SWIFT Transfer");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_Given_An_Enumeration_Should_Return_Description_Attribute_Of_Members_As_List_Of_Strings()
        {
            //Arrange
            List<string> list = new List<string>();

            //Act
            list = EnumHelper.GetDescriptions<PaymentMethod>();

            //Assert
            Assert.AreEqual(5, list.Count);
            Assert.AreEqual("None", list[0]);
            Assert.AreEqual("Check", list[1]);
            Assert.AreEqual("ACH Transfer", list[2]);
            Assert.AreEqual("Wire Transfer", list[3]);
            Assert.AreEqual("SWIFT Transfer", list[4]);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_GetDescriptions_Is_Passed_Something_Other_Than_Enumeration_Should_Return_Nothing()
        {
            //Arrange
            List<string> list = new List<string>();
            Exception exception = null;

            //Act
            try
            {
                list = EnumHelper.GetDescriptions<Constituent>();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            Assert.AreEqual("Type of argument must be Enum.", exception.Message);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_Enumeration_Without_All_Members_Having_Descriptions_Should_Return_The_Elements_That_Do_Have_Descriptions()
        {
            //Arrange
            List<string> list = new List<string>();
            
            //Act
            list = EnumHelper.GetDescriptions<TestEnumeration>();

            //Assert
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("Number Two", list[0]);
            Assert.AreEqual("Number Three", list[1]);
            Assert.AreEqual("Number Four", list[2]);
        }

    }
}
