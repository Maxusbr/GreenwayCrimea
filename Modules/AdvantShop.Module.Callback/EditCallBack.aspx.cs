using System;
using System.Text;
using System.Web.UI;
using AdvantShop.Module.Callback.Services;

namespace AdvantShop.Module.Callback
{
    public partial class Modules_Callback_EditCallback : Page
    {
        private int CallbackID;

        private CallbackCustomer callbackCustomer;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["id"]))
            {
                return;
            }
            if (!Int32.TryParse(Request["id"], out CallbackID))
            {
                return;
            }

            if ((callbackCustomer = CallbackRepository.GetCallbackCustomer(CallbackID)) == null)
            {
                return;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void LoadData()
        {
            if (callbackCustomer == null)
            {
                return;
            }

            lblDateAdded.Text = callbackCustomer.DateAdded.ToString("yyyy.MM.dd HH:mm");
            txtName.Text = callbackCustomer.Name;
            txtPhone.Text = callbackCustomer.Phone;
            txtComment.Text = callbackCustomer.Comment;
            txtAdminComment.Text = callbackCustomer.AdminComment;
            ckbProcessed.Checked = callbackCustomer.Processed;
        }

        protected void btnSaveClick(object sender, EventArgs e)
        {
            if (callbackCustomer == null)
            {
                return;
            }

            callbackCustomer.Name = txtName.Text;
            callbackCustomer.Phone = txtPhone.Text;
            callbackCustomer.Comment = txtComment.Text;
            callbackCustomer.AdminComment = txtAdminComment.Text;
            callbackCustomer.Processed = ckbProcessed.Checked;

            CallbackRepository.UpdateCallbackCustomer(callbackCustomer);

            var jScript = new StringBuilder();
            jScript.Append("<script type=\'text/javascript\' language=\'javascript\'> ");
            if (string.IsNullOrEmpty(string.Empty))
                jScript.Append("window.opener.location.reload();");
            else
                jScript.Append("window.opener.location =" + string.Empty);
            jScript.Append("self.close();");
            jScript.Append("</script>");
            Type csType = GetType();
            ClientScriptManager clScriptMng = ClientScript;
            clScriptMng.RegisterClientScriptBlock(csType, "Close_window", jScript.ToString());
        }
    }
}