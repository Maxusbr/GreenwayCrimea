using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Module.UniSender.Domain;
using AdvantShop.Orders;


namespace AdvantShop.Module.UniSender
{
    public partial class Admin_UniSenderSettings : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUniSenderId.Text = UniSenderSettings.ApiKey;
                txtFromName.Text = UniSenderSettings.FromName;
                txtFromEmail.Text = UniSenderSettings.FromEmail;
                cbSendConfirm.Checked = UniSenderSettings.SendConfirmation;
            }

            LoadLists();

            lbSubscribeReg.Visible = ddlUniSenderListsReg.SelectedValue != "0";
            lbSubscribeOrderCustomers.Visible = ddlUniSenderListsOrderCustomers.SelectedValue != "0";

        }

        protected void Save()
        {
            //var lastUniSenderId = UniSenderSettings.ApiKey;

            if (!ValidateData())
            {
                lblMessage.Text = (string)GetLocalResourceObject("UniSender_Error");
                lblMessage.ForeColor = Color.Red;
                lblMessage.Visible = true;
                return;
            }

            UniSenderSettings.ApiKey = txtUniSenderId.Text;
            UniSenderSettings.FromName = txtFromName.Text;
            UniSenderSettings.FromEmail = txtFromEmail.Text;
            UniSenderSettings.SendConfirmation = cbSendConfirm.Checked;

            if (string.IsNullOrEmpty(txtUniSenderId.Text))
            {
                UniSenderSettings.RegUsersList = string.Empty;
                UniSenderSettings.OrderCustomersList = string.Empty;
            }

            //if (string.IsNullOrEmpty(txtUniSenderId.Text) || !string.Equals(lastUniSenderId, UniSenderSettings.ApiKey))
            //{
            //    UniSenderSettings.ApiKey = lastUniSenderId;
            //    UniSenderService.UnsubscribeListMembers(UniSenderSettings.RegUsersList);
            //    UniSenderService.UnsubscribeListMembers(UniSenderSettings.OrderCustomersList);

            //    UniSenderSettings.ApiKey = txtUniSenderId.Text;
            //    UniSenderSettings.RegUsersList = string.Empty;
            //    UniSenderSettings.OrderCustomersList = string.Empty;
            //    return;
            //}

            //if (ddlUniSenderListsReg.SelectedValue == "0")
            //{
            //    UniSenderService.UnsubscribeListMembers(UniSenderSettings.RegUsersList);
            //}
            //else
            //{
            //    UniSenderService.SubscribeListMembers(ddlUniSenderListsReg.SelectedValue,
            //        SubscriptionService.GetSubscribedEmails());
            //}


            //if (ddlUniSenderListsOrderCustomers.SelectedValue == "0")
            //{
            //    UniSenderService.UnsubscribeListMembers(UniSenderSettings.OrderCustomersList);
            //}
            //else
            //{
            //    UniSenderService.SubscribeListMembers(ddlUniSenderListsOrderCustomers.SelectedValue,
            //        OrderService.GetOrderCustomersEmails());
            //}

            UniSenderSettings.RegUsersList = ddlUniSenderListsReg.SelectedValue != "0" || string.IsNullOrEmpty(ddlUniSenderListsReg.SelectedValue) ? ddlUniSenderListsReg.SelectedValue : string.Empty;
            UniSenderSettings.OrderCustomersList = ddlUniSenderListsOrderCustomers.SelectedValue != "0" || string.IsNullOrEmpty(ddlUniSenderListsReg.SelectedValue) ? ddlUniSenderListsOrderCustomers.SelectedValue : string.Empty;

            LoadLists();

            lblMessage.Text = (string)GetLocalResourceObject("UniSender_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void LoadLists()
        {
            List<UniSenderList> lists = UniSenderService.GetLists();
            if (lists != null && lists.Count > 0)
            {
                ddlUniSenderListsReg.DataSource = lists;
                ddlUniSenderListsReg.DataBind();
                if (lists.All(item => item.id != UniSenderSettings.RegUsersList))
                    UniSenderSettings.RegUsersList = string.Empty;
                ddlUniSenderListsReg.SelectedValue = string.IsNullOrEmpty(UniSenderSettings.RegUsersList)
                    ? "0"
                    : UniSenderSettings.RegUsersList;

                ddlUniSenderListsOrderCustomers.DataSource = lists;
                ddlUniSenderListsOrderCustomers.DataBind();
                if (lists.All(item => item.id != UniSenderSettings.OrderCustomersList))
                    UniSenderSettings.OrderCustomersList = string.Empty;
                ddlUniSenderListsOrderCustomers.SelectedValue = string.IsNullOrEmpty(UniSenderSettings.OrderCustomersList)
                    ? "0"
                    : UniSenderSettings.OrderCustomersList;
            }
            else
            {
                ddlUniSenderListsReg.Items.Clear();
                ddlUniSenderListsReg.Items.Add(new ListItem
                {
                    Text = CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? "Нет привязки к списку" : "No binding to the list",
                    Value = @"0"
                });
                ddlUniSenderListsReg.DataBind();

                ddlUniSenderListsOrderCustomers.Items.Clear();
                ddlUniSenderListsOrderCustomers.Items.Add(new ListItem
                {
                    Text = CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? "Нет привязки к списку" : "No binding to the list",
                    Value = @"0"
                });
                ddlUniSenderListsOrderCustomers.DataBind();
            }
        }

        private bool ValidateData()
        {
            bool valid = true;

            if (valid)
            {
                valid &= !string.IsNullOrEmpty(txtFromName.Text) && !string.IsNullOrEmpty(txtFromEmail.Text);
                valid &= ValidationHelper.IsValidEmail(txtFromEmail.Text);
                valid &= !txtFromName.Text.Contains("\"");
            }
            return valid;
        }

        protected void lbSubscribeReg_Click(object sender, EventArgs e)
        {
            UniSenderService.SubscribeListMembers(ddlUniSenderListsReg.SelectedValue, SubscriptionService.GetSubscribedEmails());
            lblMessage.Text = (string)GetLocalResourceObject("UniSender_Subscribed");
        }

        protected void lbSubscribeOrderCustomers_Click(object sender, EventArgs e)
        {
            UniSenderService.SubscribeListMembers(ddlUniSenderListsOrderCustomers.SelectedValue, OrderService.GetOrderCustomersEmails());
            lblMessage.Text = (string)GetLocalResourceObject("UniSender_Subscribed");
        }
    }
}