#region Apache Notice
/*****************************************************************************
 * Date: November 11, 2007
 * 
 * Modelus Log4Net Extensions
 * Copyright (C) 2007 - Modelus LLC
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

using System.Net;
using System.Net.Mail;
using System.Text;
using log4net.Appender;

namespace AdvantShop.Diagnostics
{
    /// <summary>Sends an HTML email when logging event occurs</summary>
    public class HtmlSmtpAppender : SmtpAppender
    {
        /// <summary>Sends an email message</summary>
        /// <summary>Sends an email message</summary>
        protected override void SendEmail(string body)
        {
            Send(SmtpHost, Username, Password, Port, EnableSsl, From, To, Subject, body);
        }

        private static void Send(string mailSmtpServer, string mailUserName, string mailPassword, int mailSmtpPort, bool mailEnableSsl, string mailFrom, string toemail, string title, string message, string file = null)
        {
            using (var emailClient = new SmtpClient(mailSmtpServer))
            {
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = new NetworkCredential(mailUserName, mailPassword);
                emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                emailClient.Port = mailSmtpPort;
                emailClient.EnableSsl = mailEnableSsl;

                using (var messageSend = new MailMessage(mailFrom, toemail, title, message))
                {
                    Attachment attach = null;
                    if (file != null)
                    {
                        attach = new Attachment(file);
                        messageSend.Attachments.Add(attach);
                    }
                    messageSend.IsBodyHtml = true;
                    messageSend.SubjectEncoding = Encoding.UTF8;
                    messageSend.HeadersEncoding = Encoding.UTF8;
                    messageSend.BodyEncoding = Encoding.UTF8;
                    emailClient.Send(messageSend);
                    if (attach != null) attach.Dispose();
                }
            }
        }
    }
}
