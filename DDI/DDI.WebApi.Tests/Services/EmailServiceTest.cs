using System;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting;
using System.Text;
using System.Web.Configuration;
using DDI.WebApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.WebApi.Tests.Services
{
    [TestClass]
    public class EmailServiceTest
    {
        private const string TESTDESCR = "WebApi | Services";

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_email_service_with_fake_smtp_client_is_called_Should_throw_exception()
        {
            //Arrange
            Exception expectedException = null;

            var service = new EmailService(new SmtpClient("localhost", 39));
            

            var To = new MailAddress("RClough@ddi.org");
            var From = new MailAddress("MadeUp@ddi.org");
            var body = "This was sent using our new email service.";
            var subject = "Test email service";

            var message = service.CreateMailMessage(To, From, subject, body);
            

            //Act
            try
            {
                service.SendMailMessage(message);
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            //Assert
            Assert.IsNotNull(expectedException);
            Assert.AreEqual("Unknown remoting error.", expectedException.Message);
        }

        [Ignore]
        [TestMethod, TestCategory(TESTDESCR)]
        public void When_correct_SMTP_info_is_used_Should_send_email()
        {
            //Arrange
            Exception expectedException = null;

            var service = new EmailService();

            // ***NOTE*** when running this test, PLEASE make sure to change the email to YOUR OWN email address
            var To = new MailAddress("RClough@ddi.org");
            var From = new MailAddress(WebConfigurationManager.AppSettings["NoReplyEmail"]);  //This will Fail. There is no Webconfig for the test project
            var body = "This was sent using our new email service.";
            var subject = "Test email service";

            var message = service.CreateMailMessage(To, From, subject, body);

            //Act
            try
            {
                service.SendMailMessage(message);
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            //Assert
            Assert.IsNull(expectedException);
        }
    }
}
