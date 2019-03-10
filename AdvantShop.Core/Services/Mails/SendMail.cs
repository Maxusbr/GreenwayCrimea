//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using AdvantShop.Core.Common;
using AdvantShop.Core.Services.Loging;
using AdvantShop.Core.Services.Loging.Emails;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Mails
{
    public class SendMail
    {
        public static IEmailLoger Loger = LogingManager.GetEmailLoger();

        private static bool SendMailThread(Guid customerIdTo, string strTo, string strSubject, string strText, bool isBodyHtml, string smtpServer, string login, string password, int port, string emailFrom, bool ssl, string senderName, string replyTo)
        {
            string errror;
            return SendMailThreadStringResult(customerIdTo, strTo, strSubject, strText, isBodyHtml, smtpServer, login, password, port, emailFrom, ssl, senderName, out errror, replyTo);
        }

        public static bool SendMailThreadStringResult(Guid customerIdTo, string strTo, string strSubject, string strText, bool isBodyHtml,
                                                        string smtpServer, string login, string password, int port, string emailFrom,
                                                        bool ssl, string senderName, out string error, string replyTo, int retryCount = 5)
        {
            error = null;
            var result = true;
            var status = EmailStatus.Sent;

            try
            {
                string[] strMails = strTo.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string strEmail in strMails)
                {
                    RetryTool.Do(() => _send(strEmail, strSubject, strText, isBodyHtml, smtpServer, login, password, port, emailFrom, ssl, senderName, replyTo), 60, retryCount);
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions != null && ex.InnerExceptions.Count > 0)
                {
                    error = ex.InnerExceptions.Aggregate("", (current, innerEx) => current + " " + (innerEx.InnerException != null ? innerEx.InnerException.Message : innerEx.Message));
                }
                else
                {
                    error = ex.Message;
                }

                Debug.Log.Error(ex);
                status = EmailStatus.Error;
                result = false;
            }

            Loger.LogEmail(new Email()
            {
                CreateOn = DateTime.Now,
                CustomerId = customerIdTo,
                Body = strText,
                EmailAddress = strTo,
                Subject = strSubject,
                Status = status
            });

            return result;
        }

        private static void _send(string strTo, string strSubject, string strText, bool isBodyHtml, string smtpServer, string login, string password, int port, string emailFrom, bool ssl, string senderName, string replyTo)
        {
            using (var emailClient = new SmtpClient(smtpServer))
            {
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = new NetworkCredential(login, password);
                emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                emailClient.Port = port;
                emailClient.EnableSsl = ssl;

                string strE = strTo.Trim();
                if (string.IsNullOrEmpty(strE))
                    return;
                if (!ValidationHelper.IsValidEmail(strE))
                    return;

                using (MailMessage message = new MailMessage(
                                    new MailAddress(emailFrom, string.IsNullOrEmpty(senderName) ? emailFrom : senderName),
                                    new MailAddress(strE, strE)
                        ))
                {
                    message.Subject = strSubject;
                    message.Body = strText;
                    message.IsBodyHtml = isBodyHtml;
                    message.SubjectEncoding = Encoding.UTF8;
                    message.BodyEncoding = Encoding.UTF8;
                    message.HeadersEncoding = Encoding.UTF8;
                    if (replyTo != null && ValidationHelper.IsValidEmail(replyTo))
                    {
                        message.ReplyToList.Add(replyTo);
                    }
                    emailClient.Send(message);
                }
            }
        }

        private static bool SendMailNow(Guid customerIdTo, string strTo, string strSubject, string strText, bool isBodyHtml, string setSmtpServer, int setPort, string setLogin, string setPassword, string setEmailFrom, bool setSsl, string senderName, string replyTo)
        {
            Task.Factory.StartNew(() => SendMailThread(customerIdTo, strTo, strSubject, strText, isBodyHtml, setSmtpServer, setLogin, setPassword, setPort, setEmailFrom, setSsl, senderName, replyTo));
            return true;
        }

        /// <summary>
        /// Sending e-mail
        /// </summary>
        /// <param name="customerIdTo">ID of the customer reserving e-mail. If it's a system e-mail use Guid.Empty</param>
        /// <param name="strTo">List of addresses to send e-mail separated by semicolon (;)</param>
        /// <param name="strSubject">Subject of a e-mail</param>
        /// <param name="strText">Body of e-mail</param>
        /// <param name="isBodyHtml">Determines if e-mail body contais HTML entities</param>
        /// <returns></returns>
        public static bool SendMailNow(Guid customerIdTo, string strTo, string strSubject, string strText, bool isBodyHtml, string replyTo = null)
        {
            string smtp = SettingsMail.SMTP;
            string login = SettingsMail.Login == string.Empty || SettingsMail.Login == SettingsMail.SIX_STARS ? SettingsMail.InternalDataL : SettingsMail.Login;
            string password = SettingsMail.Password == string.Empty || SettingsMail.Password == SettingsMail.SIX_STARS ? SettingsMail.InternalDataP : SettingsMail.Password;
            int port = SettingsMail.Port;
            string email = SettingsMail.From;
            bool ssl = SettingsMail.SSL;
            string senderName = SettingsMail.SenderName;
            return SendMailNow(customerIdTo, strTo, strSubject, strText, isBodyHtml, smtp, port, login, password, email, ssl, senderName, replyTo);
        }
    }
}