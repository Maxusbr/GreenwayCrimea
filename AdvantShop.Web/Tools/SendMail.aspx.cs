//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace Tools
{
    public partial class SendMail : System.Web.UI.Page
    {

        private void MsgErr(bool boolClean)
        {
            if (boolClean)
            {
                Message.Visible = false;
                Message.Text = string.Empty;
            }
            else
            {
                Message.Visible = false;
            }
        }

        private void MsgErr(string strMessageText, bool isSucces)
        {
            const string strSuccesFormat = "<div class=\"label-box good\">{0} // at {1}</div>";
            const string strFailFormat = "<div class=\"label-box error\">{0} // at {1}</div>";

            Message.Visible = true;

            if (isSucces)
            {
                Message.Text = string.Format(strSuccesFormat, strMessageText, DateTime.Now.ToString());
            }
            else
            {
                Message.Text = string.Format(strFailFormat, strMessageText, DateTime.Now.ToString());
            }

        }

        private bool ValidForm()
        {

            // Validation

            bool valid = true;

            if (string.IsNullOrEmpty(txtSmtp.Text))
            {
                txtSmtp.CssClass = "clsTextBase clsText_faild";
                valid = false;
            }
            else
            {
                txtSmtp.CssClass = "clsTextBase clsText";
            }

            if (string.IsNullOrEmpty(txtLogin.Text))
            {
                txtLogin.CssClass = "clsTextBase clsText_faild";
                valid = false;
            }
            else
            {
                txtLogin.CssClass = "clsTextBase clsText";
            }


            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.CssClass = "clsTextBase clsText_faild";
                valid = false;
            }
            else
            {
                txtPassword.CssClass = "clsTextBase clsText";
            }

            if (string.IsNullOrEmpty(txtTo.Text))
            {
                txtTo.CssClass = "clsTextBase clsText_faild";
                valid = false;
            }
            else
            {
                txtTo.CssClass = "clsTextBase clsText";
            }

            if (string.IsNullOrEmpty(txtFrom.Text))
            {
                txtFrom.CssClass = "clsTextBase clsText_faild";
                valid = false;
            }
            else
            {
                txtFrom.CssClass = "clsTextBase clsText";
            }

            if (string.IsNullOrEmpty(txtSubject.Text))
            {
                txtSubject.CssClass = "clsTextBase clsText_faild";
                valid = false;
            }
            else
            {
                txtSubject.CssClass = "clsTextBase clsText";
            }

            if (string.IsNullOrEmpty(txtMessage.Text))
            {
                txtMessage.CssClass = "clsTextBase clsText_faild";
                valid = false;
            }
            else
            {
                txtMessage.CssClass = "clsTextBase clsText";
            }

            if (string.IsNullOrEmpty(txtPort.Text))
            {
                txtPort.CssClass = "clsTextBase clsText_faild";
                valid = false;
            }
            else
            {
                txtPort.CssClass = "clsTextBase clsText";

                int ti;
                if (int.TryParse(txtPort.Text, out ti))
                {
                    txtPort.CssClass = "clsTextBase clsText";
                }
                else
                {
                    txtPort.CssClass = "clsTextBase clsText_faild";
                    valid = false;
                }
            }

            return valid;

        }

        protected void btnSendMail_Click(object sender, System.EventArgs e)
        {

            if (ValidForm())
            {
                int intPort = 25;
                int.TryParse(txtPort.Text, out intPort);

                string strResult;
                strResult = SendMail3(txtTo.Text, txtSubject.Text, txtMessage.Text, false, chkSSL.Checked, intPort, txtSmtp.Text, txtLogin.Text, txtPassword.Text, txtFrom.Text);

                if (strResult.Equals("True"))
                {
                    MsgErr("Message was successfuly sent", true);
                }
                else
                {
                    MsgErr(strResult, false);
                }

            }
            else
            {
                MsgErr("Not valid parameters", false);
            }

        }

        public static string SendMail3(string strTo,
                                        string strSubject,
                                        string strText,
                                        bool isBodyHtml,
                                        bool SSL,
                                        int Port,
                                        string smtpServer,
                                        string login,
                                        string password,
                                        string emailFrom)
        {
            string strResult;
            try
            {

                var emailClient = new System.Net.Mail.SmtpClient(smtpServer)
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(login, password),
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    Port = Port
                };

                /* --- v1 ------------------------------------------------------------------ */
                //string strMailList = string.Empty;
                //string[] strMails = strTo.Split(';');
                //foreach (string str in strMails)
                //{
                //    if ((str != null) && string.IsNullOrEmpty(str) == false)
                //    {
                //        strMailList += str + "; ";
                //        var message = new System.Net.Mail.MailMessage(emailFrom, str, strSubject, strText) { IsBodyHtml = isBodyHtml };
                //        emailClient.EnableSsl = SSL;
                //        emailClient.Send(message);

                //    }
                //}
                //strMailList.TrimEnd(null);


                /* --- v2 ------------------------------------------------------------------ */
                var message = new System.Net.Mail.MailMessage(emailFrom, strTo, strSubject, strText) { IsBodyHtml = isBodyHtml };
                emailClient.EnableSsl = SSL;
                emailClient.Send(message);


                strResult = "True";
            }
            catch (Exception ex)
            {
                strResult = ex.Message + " at SendMail";
            }
            return strResult;
        }

        public static string SendMail4(string strTo,
                           string strSubject,
                           string strText,
                           bool isBodyHtml,
                           bool SSL,
                           int Port,
                           string smtpServer,
                           string login,
                           string password,
                           string emailFrom)
        {
            //Адрес SMTP-сервера
            String smtpHost = "SMTP.SERVER.RU";

            //Порт SMTP-сервера
            int smtpPort = 25;

            //Логин
            String smtpUserName = "LOGIN";

            //Пароль
            String smtpUserPass = "PASSWORD";

            //Создание подключения
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpHost, smtpPort);
            client.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpUserPass);

            //Адрес для поля "От"
            String msgFrom = "LOGIN@SERVER.RU";

            //Адрес для поля "Кому" (адрес получателя)
            String msgTo = "KUDA@TO.RU";

            //Тема письма
            String msgSubject = "Test email" ;

            //Текст письма
            String msgBody = "Hello!\r\n\r\nThis is test email from advantshop e-shop\r\n\r\n--\r\nBest regards, your e-shop";

            //Создание сообщения
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(msgFrom, msgTo, msgSubject, msgBody);

            ////Вложение для письма
            ////Если нужно больше вложений, для каждого вложения создаем свой объект Attachment с нужным путем к файлу
            //System.Net.Mail.Attachment attachData = new System.Net.Mail.Attachment("D:\Тестовое вложение.zip");

            ////Крепим к сообщению подготовленное заранее вложение
            //message.Attachments.Add(attachData);

            try
            {
                //Отсылаем сообщение
                client.Send(message);
            }
            catch
            {
                //В случае ошибки при отсылке сообщения можем увидеть, в чем проблема
                // Console.WriteLine(ex.InnerException.Message.ToString());
            }

            return "";
        }

    }
}
