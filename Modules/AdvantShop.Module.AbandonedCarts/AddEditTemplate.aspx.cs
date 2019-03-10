using System;
using System.Text;
using System.Web.UI;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Module.AbandonedCarts.Domain;

namespace AdvantShop.Module.AbandonedCarts
{
    public partial class AddEditTemplate : Page
    {
        private int _id;


        protected void Page_Load(object sender, EventArgs e)
        {
            _id = Request["Id"].TryParseInt();
            lBase.Text = string.Format("<base href='{0}'/>", UrlService.GetUrl("modules/abandonedcarts/"));
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            if (_id != 0)
            {
                LoadTemplate();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (_id != 0)
                SaveAction();
            else
                CreateAction();

            if (!lblMessage.Visible)
            {
                var jScript = new StringBuilder();
                jScript.Append("<script type=\'text/javascript\' language=\'javascript\'> ");
                jScript.Append("window.opener.location.reload(true); ");
                jScript.Append("self.close();");
                jScript.Append("</script>");
                Type csType = GetType();
                ClientScriptManager clScriptMng = ClientScript;
                clScriptMng.RegisterClientScriptBlock(csType, "Close_window", jScript.ToString());
            }
        }

        #region Private methods

        private void LoadTemplate()
        {
            AbandonedCartTemplate template = AbandonedCartsService.GetTemplate(_id);

            if (template != null)
            {
                txtName.Text = template.Name;
                txtSendingTime.Text = template.SendingTime.ToString();
                txtSubject.Text = template.Subject;
                ckeBody.Text = template.Body;
                chkActive.Checked = template.Active;
            }
        }

        private void SaveAction()
        {
            if (!IsValidData())
                return;

            try
            {
                AbandonedCartTemplate template = AbandonedCartsService.GetTemplate(_id);

                template.Name = txtName.Text;
                template.Subject = txtSubject.Text;
                template.Body = ckeBody.Text;
                template.SendingTime = txtSendingTime.Text.TryParseInt();
                template.Active = chkActive.Checked;

                AbandonedCartsService.UpdateTemplate(template);
            }
            catch (Exception ex)
            {
                MsgErr("cant add" + ex);
            }
        }

        private void CreateAction()
        {
            if (!IsValidData())
                return;

            try
            {
                var template = new AbandonedCartTemplate
                    {
                        Name = txtName.Text,
                        Subject = txtSubject.Text,
                        Body = ckeBody.Text,
                        SendingTime = txtSendingTime.Text.TryParseInt(),
                        Active = chkActive.Checked
                    };

                AbandonedCartsService.AddTemplate(template);
            }
            catch (Exception ex)
            {
                MsgErr("cant add" + ex);
            }
        }

        private bool IsValidData()
        {
            lblMessage.Visible = false;

            bool valid = txtName.Text.IsNotEmpty() && txtSubject.Text.IsNotEmpty() &&
                         txtSendingTime.Text.TryParseInt() >= 0 &&
                         ckeBody.Text.IsNotEmpty();

            if (!valid)
            {
                MsgErr("Заполните все поля");
            }

            return valid;
        }

        private void MsgErr(string msg)
        {
            lblMessage.Visible = true;
            lblMessage.Text = msg;
        }

        #endregion
    }
}