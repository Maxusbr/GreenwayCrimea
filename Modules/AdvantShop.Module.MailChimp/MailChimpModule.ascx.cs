using System;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Customers;
using AdvantShop.Modules;
using System.Globalization;

namespace Advantshop.Modules.UserControls
{
    public partial class Admin_MailChimpModule : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void Save()
        {
            var lastMailChimpId = MailChimpSettings.ApiKey;
            MailChimpSettings.FromName = txtFromName.Text;
            MailChimpSettings.FromEmail = txtFromEmail.Text.Trim();
            MailChimpSettings.ApiKey = txtMailChimpId.Text;

            if (string.IsNullOrEmpty(txtMailChimpId.Text))
            {
                MailChimpService.UnsubscribeListMembers(MailChimpSettings.ApiKey, MailChimpSettings.RegUsersList);
                MailChimpService.UnsubscribeListMembers(MailChimpSettings.ApiKey, MailChimpSettings.OrderCustomersList);

                MailChimpSettings.RegUsersList = string.Empty;
                MailChimpSettings.OrderCustomersList = string.Empty;
                return;
            }

            var currentMailChimpId = MailChimpSettings.ApiKey;

            if (!ValidateData())
            {
                lblMessage.Text = (string)GetLocalResourceObject("MailChimp_Error");
                lblMessage.ForeColor = Color.Red;

                MailChimpSettings.ApiKey = lastMailChimpId;
                return;
            }

            if (!string.Equals(lastMailChimpId, currentMailChimpId))
            {
                MailChimpSettings.RegUsersList = string.Empty;
                MailChimpSettings.OrderCustomersList = string.Empty;
                return;
            }

            if (ddlMailChimpLists.SelectedValue == "0")
            {
                MailChimpService.UnsubscribeListMembers(MailChimpSettings.ApiKey, MailChimpSettings.RegUsersList);
            }
            else
            {
                MailChimpService.SubscribeListMembers(MailChimpSettings.ApiKey, ddlMailChimpLists.SelectedValue, 
                    SubscriptionService.GetSubscribedEmails());
            }

            if (ddlMailChimpOrderCustomer.SelectedValue == "0")
            {
                MailChimpService.UnsubscribeListMembers(MailChimpSettings.ApiKey, MailChimpSettings.OrderCustomersList);
            }
            else
            {
                MailChimpService.SubscribeListMembers(MailChimpSettings.ApiKey, ddlMailChimpOrderCustomer.SelectedValue,
                    AdvantShop.Orders.OrderService.GetOrderCustomersEmails());
            }
            
            MailChimpSettings.RegUsersList = ddlMailChimpLists.SelectedValue != "0"
                ? ddlMailChimpLists.SelectedValue : string.Empty;
            MailChimpSettings.OrderCustomersList = ddlMailChimpOrderCustomer.SelectedValue != "0"
                ? ddlMailChimpOrderCustomer.SelectedValue : string.Empty;
  
            LoadData();

            lblMessage.Text = (string)GetLocalResourceObject("MailChimp_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void LoadData()
        {
            txtMailChimpId.Text = MailChimpSettings.ApiKey;
            txtFromName.Text = MailChimpSettings.FromName;
            txtFromEmail.Text = MailChimpSettings.FromEmail;

            var lists = MailChimpService.GetLists(MailChimpSettings.ApiKey);
            if (lists != null && lists.data != null && lists.data.Count > 0)
            {
                //Subscribers
                ddlMailChimpLists.DataSource = lists.data;
                ddlMailChimpLists.DataBind();
                if (lists.data.All(item => item.id != MailChimpSettings.RegUsersList))
                    MailChimpSettings.RegUsersList = string.Empty;
                ddlMailChimpLists.SelectedValue = !string.IsNullOrEmpty(MailChimpSettings.RegUsersList)
                    ? MailChimpSettings.RegUsersList
                    : "0";

                //Ordered customers
                ddlMailChimpOrderCustomer.DataSource = lists.data;
                ddlMailChimpOrderCustomer.DataBind();
                if (lists.data.All(item => item.id != MailChimpSettings.OrderCustomersList))
                    MailChimpSettings.OrderCustomersList = string.Empty;
                ddlMailChimpOrderCustomer.SelectedValue = !string.IsNullOrEmpty(MailChimpSettings.OrderCustomersList)
                    ? MailChimpSettings.OrderCustomersList
                    : "0";
            }
            else
            {
                //Subscribers
                ddlMailChimpLists.Items.Clear();
                ddlMailChimpLists.Items.Add(new ListItem
                {
                    Text = CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? "Нет привязки к списку" : "No binding to the list",
                    Value = @"0"
                });
                ddlMailChimpLists.DataBind();

                //Ordered customers
                ddlMailChimpOrderCustomer.Items.Clear();
                ddlMailChimpOrderCustomer.Items.Add(new ListItem
                {
                    Text = CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? "Нет привязки к списку" : "No binding to the list",
                    Value = @"0"
                });
                ddlMailChimpOrderCustomer.DataBind();
            }
        }

        private bool ValidateData()
        {
            var valid = true;

            valid &= MailChimpSettings.ApiKey.Contains("-") &&
                     MailChimpSettings.ApiKey.LastIndexOf("-") + 1 < MailChimpSettings.ApiKey.Length;

            valid &= !string.IsNullOrEmpty(txtFromName.Text) && !string.IsNullOrEmpty(MailChimpSettings.FromEmail);
            valid &= AdvantShop.Helpers.ValidationHelper.IsValidEmail(MailChimpSettings.FromEmail);
            valid &= !txtFromName.Text.Contains("\"");
            valid &= MailChimpService.PingMailchimpAccount(MailChimpSettings.ApiKey);

            return valid;
        }
    }
}