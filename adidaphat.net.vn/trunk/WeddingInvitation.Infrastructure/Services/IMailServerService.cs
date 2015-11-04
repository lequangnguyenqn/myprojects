using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace WeddingInvitation.Infrastructure.Services
{
    public interface IMailServerService
    {
        bool SendByLocalHost(string fromMail, string toMail, string subject, string body, string[] file, ref string smsg);
        bool SendByServer(string fromMail, string toMail, string subject, string body, string[] file, bool isBcc,ref string smsg);
        bool SendByServer(string fromMail, string toMail, string subject, string body, string[] file, ref string smsg);
        bool SendByServer(string toMail, string subject, string body, string[] file, ref string smsg);
        bool SendByGmail(string fromMail, string toMail, string subject, string body, string[] file, string uid, string pwd, ref string smsg);
        bool ValidEmailInput(string email);
        MailMessage GetMailMessage(string fromMail, string toMail, string subject, string body, string[] file, bool isBcc);
        SmtpClient GetSmtpClient(bool isNetworkCredential);
        SmtpClient GetSmtpClient();
    }
}
