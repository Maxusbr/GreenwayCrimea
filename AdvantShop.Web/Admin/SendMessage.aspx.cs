//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Mails;
using Resources;

namespace Admin
{
    public partial class SendMessage : AdvantShopAdminPage
    {
        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lblError.Visible = false;
                lblError.Text = string.Empty;
            }
            else
            {
                lblError.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = @"<br/>" + messageText + @"<br/>";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_SendMessage_Title));
            MsgErr(true);
            lblInfo.Text = string.Empty;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var modules = AttachedModules.GetModules<ISendMails>();
            var modulesAny = false;
            foreach (var moduleType in modules)
            {
                var moduleObject = (ISendMails)Activator.CreateInstance(moduleType, null);
                if (ModulesRepository.IsActiveModule(moduleObject.ModuleStringId))
                {
                    modulesAny = true;
                }
            }

            if (!modulesAny)
            {
                lblMessage.Text = Resource.Admin_m_News_SendMailOff;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                divMessage.Visible = true;
                divSend.Visible = false;
            }
            else if (!IsPostBack)
            {
                divMessage.Visible = false;
                divSend.Visible = true;
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!IsValidData())
            {
                return;
            }

            var isSend = false;
            var modules = AttachedModules.GetModules<ISendMails>().ToArray();
            
            var recipientType = EMailRecipientType.None;
            if (ckbSubscribers.Checked)
                recipientType |= EMailRecipientType.Subscriber;
            if (ckbOrderedCustomers.Checked)
                recipientType |= EMailRecipientType.OrderCustomer;

            foreach (var moduleType in modules)
            {
                var moduleObject = (ISendMails)Activator.CreateInstance(moduleType, null);
                if (ModulesRepository.IsActiveModule(moduleObject.ModuleStringId))
                {
                    isSend |= moduleObject.SendMails(txtTitle.Text, fckMailContent.Text, recipientType);
                }
            }
            
            if (isSend)
            {
                lblMessage.Text = Resource.Admin_m_News_MessageIsSend;
                lblMessage.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                lblMessage.Text = Resource.Admin_m_News_MessageIsSend;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }

            divMessage.Visible = true;
            divSend.Visible = false;
        }

        private bool IsValidData()
        {
            if ((txtTitle.Text.IndexOf(">") != -1) || (txtTitle.Text.IndexOf("<") != -1))
            {
                MsgErr(Resource.Admin_SendMessage_HtmlNotSupported);
                return false;
            }

            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                MsgErr(Resource.Admin_SendMessage_NoTitle);
                return false;
            }

            if (string.IsNullOrEmpty(fckMailContent.Text))
            {
                MsgErr(Resource.Admin_SendMessage_NoEmailText);
                return false;
            }

            if (!ckbSubscribers.Checked && !ckbOrderedCustomers.Checked)
            {
                MsgErr(Resource.Admin_SendMessage_ChooseRecipient);
                return false;
            }

            return true;
        }
    }
}