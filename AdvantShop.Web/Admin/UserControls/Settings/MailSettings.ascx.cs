//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Helpers;
using AdvantShop.Trial;
using Resources;

namespace Admin.UserControls.Settings
{
    public partial class MailSettings : System.Web.UI.UserControl
    {
        public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidEmail;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            txtEmailSMTP.Text = SettingsMail.SMTP;
            txtEmailLogin.Text = SettingsMail.Login;
            txtEmailPassword.Text = SettingsMail.Password;
            txtEmail.Text = SettingsMail.From;
            chkEnableSSL.Checked = SettingsMail.SSL;
            txtEmailPort.Text = SettingsMail.Port.ToString();
            txtSenderName.Text = SettingsMail.SenderName;
        }

        public bool SaveData()
        {
            if (!ValidateData())
            {
                return false;
            }

            SettingsMail.SMTP = txtEmailSMTP.Text;
            SettingsMail.Login = txtEmailLogin.Text;

            if (!string.IsNullOrEmpty(txtEmailPassword.Text))
                SettingsMail.Password = txtEmailPassword.Text;

            SettingsMail.From = txtEmail.Text;
            SettingsMail.Port = int.Parse(txtEmailPort.Text);
            SettingsMail.SSL = chkEnableSSL.Checked;

            SettingsMail.SenderName = txtSenderName.Text;


            LoadData();

            return true;
        }

        private bool ValidateData()
        {
            if (string.IsNullOrEmpty(txtEmailSMTP.Text))
            {
                //MsgErr(Resource.Admin_CommonSettings_NoSmtp);
                return false;
            }

            if (string.IsNullOrEmpty(txtEmailLogin.Text))
            {
                //MsgErr(Resource.Admin_CommonSettings_NoLogin);
                return false;
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                //MsgErr(Resource.Admin_CommonSettings_NoEmail);
                return false;
            }
            int ti;
            if (!int.TryParse(txtEmailPort.Text, out ti))
            {
                //MsgErr(Resource.Admin_CommonSettings_NoNumberPortEmail);
                return false;
            }
            return true;
        }

        private bool ValidateData_TestMailForm()
        {

            bool valid = true;

            if (string.IsNullOrEmpty(txtTo.Text) || !AdvantShop.Helpers.ValidationHelper.IsValidEmail(txtTo.Text))
            {
                txtTo.CssClass = "niceTextBox_faild shortTextBoxClass";
                valid = false;
            }
            else
            {
                txtTo.CssClass = "niceTextBox shortTextBoxClass";
            }

            if (string.IsNullOrEmpty(txtSubject.Text))
            {
                txtSubject.CssClass = "niceTextBox_faild textBoxClass";
                valid = false;
            }
            else
            {
                txtSubject.CssClass = "niceTextBox textBoxClass";
            }

            if (string.IsNullOrEmpty(txtMessage.Text))
            {
                txtMessage.CssClass = "niceTextArea_faild textArea7Lines";
                valid = false;
            }
            else
            {
                txtMessage.CssClass = "niceTextArea textArea7Lines";
            }

            return valid;
        }

        protected void btnSendMail_Click(object sender, EventArgs e)
        {

            if ((!ValidateData() || !ValidateData_TestMailForm()))
            {
                MsgErr(Resource.Admin_CommonSettings_TestEmail_NotValid, false);
                return;
            }

            //System.Threading.Thread.Sleep(1200);
            int ti;
            if (!int.TryParse(txtEmailPort.Text, out ti))
            {
                ti = 25;
            }

            string error;
            var result = AdvantShop.Mails.SendMail.SendMailThreadStringResult(Guid.Empty, txtTo.Text,
                                                                                    txtSubject.Text,
                                                                                    txtMessage.Text,
                                                                                    false,
                                                                                    txtEmailSMTP.Text,
                                                                                    txtEmailLogin.Text,
                                                                                    txtEmailPassword.Text,
                                                                                    ti,
                                                                                    txtEmail.Text,
                                                                                    chkEnableSSL.Checked, 
                                                                                    txtSenderName.Text, 
                                                                                    out error, null,
                                                                                    1);

            if (result)
            {
                if (!txtEmail.Text.Contains("advantshop"))
                {
                    TrialService.TrackEvent(TrialEvents.SendTestEmail, string.Empty);
                }
                MsgErr(Resource.Admin_CommonSettings_TestEmail_Success, true);
            }
            else
            {
                MsgErr(error, false);
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
    }
}