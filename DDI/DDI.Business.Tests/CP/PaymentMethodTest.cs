using System;
using DDI.Business.CP;
using DDI.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.CP
{
    [TestClass]
    public class PaymentMethodTest : TestBase
    {

        private const string TESTDESCR = "Business | CP";
        private IUnitOfWork _uow;
        private PaymentMethodLogic _bl;

        [TestInitialize]
        public void Initialize()
        {
            Factory.ConfigureForTesting();

            _uow = Factory.CreateUnitOfWork();

            _bl = new PaymentMethodLogic(_uow);

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void PaymentMethodLogic_ValidateRoutingNumber()
        {
            AssertNoException(() => _bl.ValidateRoutingNumber("123123123"), "Valid routing number example.");
            AssertNoException(() => _bl.ValidateRoutingNumber("074900783"), "Valid routing number example.");

            AssertThrowsExceptionMessageContains<Exception>(() => _bl.ValidateRoutingNumber("123123124"), "Invalid routing number", "Invalid routing number based on checksum.");

            AssertThrowsExceptionMessageContains<Exception>(() => _bl.ValidateRoutingNumber("074900782"), "Invalid routing number", "Invalid routing number based on checksum.");

            AssertNoException(() => _bl.ValidateRoutingNumber(""), "Empty string is valid.");
            AssertNoException(() => _bl.ValidateRoutingNumber(null), "Null string is valid.");

            AssertThrowsExceptionMessageContains<Exception>(() => _bl.ValidateRoutingNumber("123"), "must have nine digits", "Invalid routing number based on regex.");

            AssertThrowsExceptionMessageContains<Exception>(() => _bl.ValidateRoutingNumber("07490078a"), "must have nine digits", "Invalid routing number based on regex.");

            AssertThrowsExceptionMessageContains<Exception>(() => _bl.ValidateRoutingNumber("000000000"), "cannot be all zero", "Invalid routing number based on all zeros.");

        }
    }
}