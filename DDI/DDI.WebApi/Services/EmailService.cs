using System.Net.Mail;
using System.Web.Configuration;

namespace DDI.WebApi.Services
{
    public class EmailService
    {
        private SmtpClient _smtpClient;

        public EmailService()
            :this(new SmtpClient())
        {

        }

        internal EmailService(SmtpClient smtpClient)
        {
            if (string.IsNullOrWhiteSpace(smtpClient.Host))
            {
                smtpClient.Host = WebConfigurationManager.AppSettings["SmtpHost"] ?? "coloex1.ddi.org";
                smtpClient.Port = int.Parse(WebConfigurationManager.AppSettings["SmtpPort"] ?? "25");
            }

            _smtpClient = smtpClient;
        }

        public MailMessage CreateMailMessage(MailAddress from, MailAddress to, string subject, string body, bool IsHtml = true)
        {
            var mailMessage = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = IsHtml
            };

            return mailMessage;
        }

        public void SendMailMessage(MailMessage mailMessage)
        {
            _smtpClient.Send(mailMessage);
        }

    }
}