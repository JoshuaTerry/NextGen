using System;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting;
using System.Text;
using DDI.WebApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.WebApi.Tests.Services
{
    [TestClass]
    public class EmailServiceTest
    {
        [TestMethod]
        public void When_email_service_with_fake_smtp_client_is_called_Should_throw_exception()
        {
            //Arrange
            Exception expectedException = null;

            var service = new EmailService(new SmtpClient("fakeDDI.smtp.net", 25));
            

            var To = new MailAddress("RClough@ddi.org");
            var From = new MailAddress("no-reply@ddi.org");
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
        }

        [Ignore]
        [TestMethod]
        public void When_correct_SMTP_info_is_used_Should_send_email()
        {
            //Arrange
            Exception expectedException = null;

            var service = new EmailService();

            // ***NOTE*** when running this test, PLEASE make sure to change the email to YOUR OWN email address
            var To = new MailAddress("RClough@ddi.org");
            var From = new MailAddress("no-reply@ddi.org");
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
