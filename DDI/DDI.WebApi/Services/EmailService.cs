using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Remoting;
using System.Web.Configuration;
using Microsoft.Ajax.Utilities;

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
                smtpClient.Host = ConfigurationManager.AppSettings["SmtpHost"];
                smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            }

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