//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using Resources;

namespace Admin
{
    public partial class MailFormatDetail : AdvantShopAdminPage
    {
        protected bool AddingNew
        {
            get { return Request["id"] == null || Request["id"].ToLower() == "add"; }
        }

        private int _mailFormatId;
        public int MailFormatId
        {
            get
            {
                return _mailFormatId == 0 ? Int32.Parse(Request["id"]) : _mailFormatId;
            }
            set
            {
                _mailFormatId = value;
            }
        }

        protected void Page_PreRender()
        {
            if (!IsPostBack)
            {
                MailFormatId = 0;
                lblMessage.Text = "";
                lblMessage.Visible = false;
                MsgErr(true);

                var formatTypes = MailFormatService.GetMailFormatTypes();
                ddlTypes.Items.Clear();
                ddlTypes.Items.Add(new ListItem("---", ""));

                foreach (var formatType in formatTypes)
                    ddlTypes.Items.Add(new ListItem(formatType.TypeName, formatType.MailFormatTypeId.ToString()));
                

                if (AddingNew)
                {
                    ShowMailFormatTypeDescription();
                }
                else
                {
                    btnSave.Text = Resource.Admin_Update;
                    lblSubHead.Text = Resource.Admin_MailFormatDetail_Edit;

                    MailFormatId = SQLDataHelper.GetInt(Request["id"]);
                    LoadMailFormat();
                }
            }
            SetMeta(string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, Resource.Admin_MailFormat_Header));
        }

        protected void ddlTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowMailFormatTypeDescription();
        }

        private void ShowMailFormatTypeDescription()
        {
            txtDescription.Text = "";

            var mailFormatTypeId = ddlTypes.SelectedValue.TryParseInt(true);
            if (mailFormatTypeId != null)
            {
                var mailFormatType = MailFormatService.GetMailFormatType(mailFormatTypeId.Value);
                if (mailFormatType != null)
                    txtDescription.Text = mailFormatType.Comment;
            }
        }

        private void LoadMailFormat()
        {
            if (!AddingNew)
            {
                var mailFormat = MailFormatService.Get(MailFormatId);
            
                txtName.Text = mailFormat.FormatName;
                txtSubject.Text = mailFormat.FormatSubject;
                lblHead.Text = mailFormat.FormatName;
                CKEditorControl1.Text = mailFormat.FormatText;
                chkActive.Checked = mailFormat.Enable;
                txtSortOrder.Text = mailFormat.SortOrder.ToString(CultureInfo.InvariantCulture);

                if (ddlTypes.Items.FindByValue(mailFormat.MailFormatTypeId.ToString()) != null)
                    ddlTypes.SelectedValue = mailFormat.MailFormatTypeId.ToString();

                ShowMailFormatTypeDescription();

                Page.Title = string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, mailFormat.FormatName);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim().Length == 0 || txtSubject.Text.Trim().Length == 0)
            {
                MsgErr(Resource.Admin_MailFormat_NoName);
                return;
            }

            if (CKEditorControl1.Text.Trim().Length == 0)
            {
                MsgErr(Resource.Admin_MailFormat_NoText);
                return;
            }

            int result = 0;
            if (!Int32.TryParse(txtSortOrder.Text, out result))
            {
                MsgErr(Resource.Admin_MailFormat_SortNotNum);
                return;
            }

            MailFormat mailFormat = AddingNew ? new MailFormat() : MailFormatService.Get(MailFormatId);
            mailFormat.FormatName = txtName.Text.Trim();
            mailFormat.FormatSubject = txtSubject.Text.Trim();
            mailFormat.FormatText = CKEditorControl1.Text.Trim();
            mailFormat.Enable = chkActive.Checked;
            mailFormat.SortOrder = Int32.Parse(txtSortOrder.Text);
            mailFormat.MailFormatTypeId = ddlTypes.SelectedValue.TryParseInt();

            if (AddingNew)
            {
                MailFormatService.Add(mailFormat);
                Response.Redirect("MailFormat.aspx");
            }
            else
            {
                lblMessage.Text = Resource.Admin_MailFormatDetail_Saved + "<br />";
                lblMessage.Visible = true;
                MailFormatService.Update(mailFormat);
                LoadMailFormat();
            }
        }

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                Message.Visible = false;
                Message.Text = "";
            }
            else
            {
                Message.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            Message.Visible = true;
            Message.Text = "<br/>" + messageText;
        }
    }
}