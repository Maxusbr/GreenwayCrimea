using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using AdvantShop.Core.Modules;
using AdvantShop.Customers;
using AdvantShop.Module.SmsNotifications;
using AdvantShop.Module.SmsNotifications.Domain;
using AdvantShop.Module.SmsNotifications.Services;
using AdvantShop.Orders;

namespace Advantshop.Modules.UserControls
{
    public partial class Modules_SmsNotifications_SmsSending : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ESMSSenderService serviceType;
                Enum.TryParse(ModuleSettingsProvider.GetSettingValue<string>("SmsService", SmsNotifications.ModuleId), true, out serviceType);
                if (serviceType == ESMSSenderService.None)
                {
                    btnSend.Enabled = false;

                    lblMessage.Text = (String)GetLocalResourceObject("SmsNotifications_ServiceNotSet");
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Visible = true;
                }

                if (!string.IsNullOrEmpty(Request["phone"]))
                {
                    txtPhone.Text = Request["phone"];
                    ddlRecipientType.SelectedValue = "One";
                }
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (ddlRecipientType.SelectedValue == "One")
            {
                long phone;
                if (!long.TryParse(txtPhone.Text, out phone))
                {
                    lblMessage.Text = (String)GetLocalResourceObject("SmsNotifications_WrongNumber");
                    lblMessage.ForeColor = Color.Blue;
                    lblMessage.Visible = true;
                    return;
                }

                Guid customerId;
                Guid.TryParse(Request["customerId"], out customerId);
                SmsNotificationsService.SendNow(customerId, phone, txtMessage.Text);
            }
            else
            {
                ESMSRecipientType recipientType;
                Enum.TryParse(ddlRecipientType.SelectedValue, true, out recipientType);
                SmsNotificationsService.SendingNews(txtMessage.Text, recipientType);
            }

            txtMessage.Text = string.Empty;

            lblMessage.Text = (String)GetLocalResourceObject("SmsNotifications_Sending");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void lnkPhonesList_Click(object sender, EventArgs e)
        {
            ESMSRecipientType recipientType;
            Enum.TryParse(ddlRecipientType.SelectedValue, true, out recipientType);
            Dictionary<Guid, long> phones;

            switch (recipientType)
            {
                case ESMSRecipientType.Subscriber:
                    phones = CustomerService.GetSubscribedCustomersPhones();
                    break;
                case ESMSRecipientType.Customer:
                    phones = CustomerService.GetCustomersPhones();
                    break;
                case ESMSRecipientType.OrderCustomer:
                    phones = OrderService.GetAllOrdersPhones();
                    break;
                case ESMSRecipientType.All:
                    phones = CustomerService.GetCustomersPhones();
                    foreach (var kvp in OrderService.GetAllOrdersPhones().Where(kvp => !phones.ContainsKey(kvp.Key)))
                        phones.Add(kvp.Key, kvp.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("recipientType");
            }

            var phonesList = new StringBuilder();
            foreach (var phone in phones.Distinct(new PhoneComparer()))
            {
                phonesList.AppendFormat("{0}\r\n", phone.Value);
            }

            Response.Clear();
            Response.Charset = "utf-8";
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Connection", "Keep-Alive");
            Response.AddHeader("Content-Length", phonesList.Length.ToString());
            Response.AddHeader("content-disposition", string.Format("attachment; filename=phones.txt"));
            Response.Write(phonesList.ToString());
            Response.Flush();
            Response.End();
        }
    }
}