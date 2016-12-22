using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Remoting;
using Microsoft.Ajax.Utilities;

namespace DDI.WebApi.Services
{
    public class EmailService
    {
        private const string SMTP_HOST = "coloex1.ddi.net";
        private const int SMTP_PORT = 25;
        
        private SmtpClient _smtpClient { get; set; }


        public EmailService()
            :this(new SmtpClient(SMTP_HOST, SMTP_PORT))
        {

        }

        internal EmailService(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public MailMessage CreateMailMessage(MailAddress to, MailAddress from, string subject, string body)
        {
            var mailMessage = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body
            };

            return mailMessage;
        }

        public void SendMailMessage(MailMessage mailMessage)
        {
            try
            {
                _smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                throw new ServerException();
            }
        }

    }
}