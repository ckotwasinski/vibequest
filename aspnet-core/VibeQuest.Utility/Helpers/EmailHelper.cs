using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        private readonly IConfiguration _appConfiguration;

        public EmailHelper(IConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public bool SendEmail(List<string> To, List<string> CC, List<string> BCC, string Subject, string Body, List<string> Attachments, List<Tuple<byte[], string>> StreamAttachments = null)
        {
            MailMessage message = new MailMessage();

            string host = _appConfiguration.GetValue<string>("Settings:MailHost");
            string username = _appConfiguration.GetValue<string>("Settings:MailUsername");
            string password = _appConfiguration.GetValue<string>("Settings:MailPassword");
            int port = _appConfiguration.GetValue<int>("Settings:MailPort");
            bool enableSSL = true;

            message.IsBodyHtml = true;
            if (To.Count > 0)
            {
                message.To.Add(string.Join(",", To));
            }

            if (CC.Count > 0)
            {
                message.CC.Add(string.Join(",", CC));
            }

            if (BCC.Count > 0)
            {
                message.CC.Add(string.Join(",", BCC));
            }

            message.From = new MailAddress(username, "VibeQuest");
            message.Subject = Subject;
            message.Body = Body;

            if (Attachments.Count > 0)
            {
                foreach (var i in Attachments)
                {
                    message.Attachments.Add(new Attachment(i));
                }
            }

            if (StreamAttachments != null && StreamAttachments.Count > 0)
            {
                foreach (var i in StreamAttachments)
                {
                    message.Attachments.Add(new Attachment(new MemoryStream(i.Item1), i.Item2));
                }
            }

            var smtpClient = new SmtpClient();
            if (!string.IsNullOrEmpty(host))
                smtpClient.Host = host; //Define Host or Sending URL

            if (port > 0)
                smtpClient.Port = port;

            if (enableSSL)
                smtpClient.EnableSsl = enableSSL;

            //smtpClient.UseDefaultCredentials = false;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                smtpClient.Credentials = new System.Net.NetworkCredential(username, password);

            smtpClient.Send(message);

            //GC.Collect();
            return true;
        }
    }
}
