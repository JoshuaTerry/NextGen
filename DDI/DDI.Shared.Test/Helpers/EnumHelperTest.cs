using System;
using System.Text;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test.Helpers
{
    [TestClass]
    public class EnumHelperTest
    {
        [TestMethod]
        public void When_Given_An_Enumeration_Member_Should_Return_Description_Attribute_Value()
        {
            //Act
            var description = EnumHelper.GetDescription(PaymentMethod.SWIFT);

            //Assert
            Assert.AreEqual(description, "SWIFT Transfer");
        }

        [TestMethod]
        public void When_Given_An_Enumeration_Should_Return_Description_Attribute_Of_Members_As_List_Of_Strings()
        {
            //Act
            var list = EnumHelper.GetDescriptions<PaymentMethod>();

            //Assert
            Assert.AreEqual(5, list.Count);
            Assert.AreEqual("None", list[0]);
            Assert.AreEqual("Check", list[1]);
            Assert.AreEqual("ACH Transfer", list[2]);
            Assert.AreEqual("Wire Transfer", list[3]);
            Assert.AreEqual("SWIFT Transfer", list[4]);
        }
    }
}
