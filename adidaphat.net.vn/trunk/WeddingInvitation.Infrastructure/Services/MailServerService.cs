using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;

namespace WeddingInvitation.Infrastructure.Services
{
    public class MailServerService: IMailServerService
    {
        public const string SmtpCredentialUserName = "dating.world.2010@gmail.com";
        public const string SmtpCredentialPassword = "dating2010";
        //Timeout: An Int32 that specifies the time-out value in milliseconds. The default value is 100,000 (100 seconds).
        private const int SmtpClientTimeOut = 1200000;// 1200 seconds --> 20 minutes
        private readonly MailServerSetting _mailServerSetting;

        public MailServerService()
        {
            _mailServerSetting = new MailServerSetting 
            {
                EmailAddress = ConfigurationManager.AppSettings["EmailSender"],
                EmailHost = ConfigurationManager.AppSettings["EmailHost"],
                EmailSender = ConfigurationManager.AppSettings["DisplaySender"],
                EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]),
                Port = int.Parse(ConfigurationManager.AppSettings["EmailPort"]),
                SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"],
                SmtpUserName = ConfigurationManager.AppSettings["SmtpUserName"],
                UseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["UseDefaultCredentials"])
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromMail"></param>
        /// <param name="toMail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="file"></param>
        /// <param name="smsg"></param>
        /// <returns></returns>
        public bool SendByLocalHost(string fromMail, string toMail, string subject, string body, string[] file, ref string smsg)
        {
            try
            {
                MailMessage mailMessage = GetMailMessage(fromMail, toMail, subject, body, file, false);

                SmtpClient smtpClient = GetSmtpClient();
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                smsg = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromMail"></param>
        /// <param name="toMail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="file"></param>
        /// <param name="isBcc"></param>
        /// <param name="smsg"></param>
        /// <returns></returns>
        public bool SendByServer(string fromMail, string toMail, string subject, string body, string[] file, bool isBcc, ref string smsg)
        {
            try
            {
                MailMessage mailMessage = GetMailMessage(fromMail, toMail, subject, body, file, isBcc);
                SmtpClient smtpClient = GetSmtpClient(true);
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                smsg = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromMail"></param>
        /// <param name="toMail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="file"></param>
        /// <param name="smsg"></param>
        /// <returns></returns>
        public bool SendByServer(string fromMail, string toMail, string subject, string body, string[] file, ref string smsg)
        {
            return SendByServer(fromMail, toMail, subject, body, file, false, ref smsg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toMail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="file"></param>
        /// <param name="smsg"></param>
        /// <returns></returns>
        public bool SendByServer(string toMail, string subject, string body, string[] file, ref string smsg)
        {
           return SendByServer(_mailServerSetting.EmailAddress, toMail, subject, body, file, false, ref smsg);
        }

        /// <summary>
        /// Send By Gmail
        /// </summary>
        /// <param name="fromMail"></param>
        /// <param name="toMail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="file"></param>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        /// <param name="smsg"></param>
        /// <returns></returns>
        public bool SendByGmail(string fromMail, string toMail, string subject, string body, string[] file, string uid, string pwd, ref string smsg)
        {
            try
            {
                //body = body.Replace("<", "&lt;");
                //body = body.Replace(">", "&gt;");
                MailMessage mailMessage = GetMailMessage(fromMail, toMail, subject, body, file, false);

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Timeout = SmtpClientTimeOut;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(uid, pwd);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                smsg = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool ValidEmailInput(string email)
        {
            string pattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            return reg.IsMatch(email);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromMail"></param>
        /// <param name="toMail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="file"></param>
        /// <param name="isBcc"></param>
        /// <returns></returns>
        public MailMessage GetMailMessage(string fromMail, string toMail, string subject, string body, string[] file, bool isBcc)
        {
            string[] mailsTo = toMail.Split(new string[] { ";", "," }, StringSplitOptions.None);

            string sender = _mailServerSetting.EmailSender;

            MailMessage mail = new MailMessage();
            if (!string.IsNullOrEmpty(sender))
            {
                mail.From = new MailAddress(fromMail, sender);
            }
            else
            {
                mail.From = new MailAddress(fromMail);
            }

            if (isBcc)
            {
                mail.To.Add(new MailAddress(fromMail));
            }

            //bool isAddTo = false;

            foreach (string mailTo in mailsTo)
            {
                if (ValidEmailInput(mailTo))
                {
                    if (isBcc)
                    {
                        mail.Bcc.Add(new MailAddress(mailTo));
                        //if (!isAddTo)
                        //{
                        //    mail.To.Add(new MailAddress(mailTo));
                        //    isAddTo = true;
                        //}
                        //else
                        //{
                        //    mail.Bcc.Add(new MailAddress(mailTo));
                        //}
                    }
                    else
                    {
                        mail.To.Add(new MailAddress(mailTo));
                    }
                }
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            mail.Priority = MailPriority.High;
            mail.Headers.Add("X-Company", sender ?? "");
            //mail.Headers.Add("X-Location", _mailServerSetting.Location);
            foreach (var s in file)
            {
                if (!String.IsNullOrEmpty(s)) mail.Attachments.Add(new Attachment(s));
            }
            
            return mail;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isNetworkCredential"></param>
        /// <returns></returns>
        public SmtpClient GetSmtpClient(bool isNetworkCredential)
        {
            SmtpClient smtpClient = new SmtpClient(_mailServerSetting.EmailHost, _mailServerSetting.Port);
            smtpClient.Timeout = SmtpClientTimeOut;
            if (isNetworkCredential)
            {
                smtpClient.UseDefaultCredentials = _mailServerSetting.UseDefaultCredentials;
                smtpClient.Credentials = new NetworkCredential(_mailServerSetting.SmtpUserName,
                                                               _mailServerSetting.SmtpPassword);
                smtpClient.EnableSsl = _mailServerSetting.EnableSsl;
            }
            return smtpClient;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SmtpClient GetSmtpClient()
        {
            return GetSmtpClient(false);
        }

        #region Test
        public bool SendByServerBySetting(string smtpCredentialUserName, string smtpCredentialPassword, string host, string fromMail, string toMail, string subject, string body, string[] file, ref string smsg)
        {
            try
            {
                MailMessage mailMessage = GetMailMessage(fromMail, toMail, subject, body, file, false);

                SmtpClient smtpClient = new SmtpClient(host);
                smtpClient.Timeout = SmtpClientTimeOut;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpCredentialUserName, smtpCredentialPassword);
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                smsg = ex.ToString();
                return false;
            }
        }
        #endregion
    }
}





