using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeddingInvitation.Infrastructure.Services
{
    public class MailServerSetting
    {
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public string EmailHost { get; set; }
        public int Port { get; set; }
        public string EmailSender { get; set; }
        public string EmailAddress { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
