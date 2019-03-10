//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Saas;
using Resources;
using AdvantShop.Configuration;
using AdvantShop.Customers;

namespace Admin
{
    public partial class EditLead : AdvantShopAdminPage
    {
        #region Fields

        protected int LeadId = 0;
        protected float CurrencyValue;
        protected string CurrencyCode;
        protected int Count;
        protected Guid CustomerId;

        protected Lead Lead;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            CustomerId = Request["CustomerId"].TryParseGuid();
            LeadId = Request["id"].TryParseInt();

            if (!IsPostBack)
                LoadData();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCrm)
            {
                return;
            }

            if (PopupGridCustomers.SelectedCustomers != null && PopupGridCustomers.SelectedCustomers.Count > 0)
            {
                LoadCustomer(PopupGridCustomers.SelectedCustomers[0]);
                PopupGridCustomers.CleanSelection();
            }
            else if (CustomerId != Guid.Empty)
            {
                LoadCustomer(CustomerId);
            }
        }

        protected void orderItems_Updated(object sender, EventArgs args)
        {
            // LoadTotal();
        }

        protected void btnAddOrSaveLead_Click(object sender, EventArgs e)
        {
            txtCustomerEmail.CssClass = "customer-email-field";
            if ((!string.IsNullOrEmpty(txtCustomerEmail.Text) && !ValidationHelper.IsValidEmail(txtCustomerEmail.Text))
                && !string.Equals(txtCustomerEmail.Text, "admin"))
            {
                txtCustomerEmail.CssClass = "customer-email-field niceTextBox_faild";
                return;
            }

            var lead = AddOrUpdateLead();
            Response.Redirect("EditLead.aspx?id=" + lead.Id);
        }

        private void LoadData()
        {
            blockManagers.Visible = SettingsCheckout.EnableManagersModule;

            ddlManager.Items.Add(new ListItem("-", ""));

            foreach (var manager in ManagerService.GetManagersList())
            {
                ddlManager.Items.Add(new ListItem(string.Format("{0} {1}", manager.FirstName, manager.LastName),
                    manager.ManagerId.ToString()));

                ddlTaskManager.Items.Add(new ListItem(string.Format("{0} {1}", manager.FirstName, manager.LastName),
                    manager.ManagerId.ToString()));
            }

            //foreach (OrderType type in Enum.GetValues(typeof(OrderType)))
            //{
            //    ddlOrderType.Items.Add(new ListItem(type.ResourceKey(), type.ToString()));
            //}

            ddlOrderType.DataSource = OrderSourceService.GetOrderSources();
            ddlOrderType.DataBind();

            //foreach (LeadStatus type in Enum.GetValues(typeof(LeadStatus)))
            //{
            //    ddlLeadStatus.Items.Add(new ListItem(type.ResourceKey(), type.ToString()));
            //}

            Lead = LeadService.GetLead(LeadId);
            if (Lead != null)
            {
                GetLead();

                btnAddOrSaveLead.Text = Resource.Admin_Leads_SaveChanges;
                pnlLeadInfo.Visible = true;

                SetMeta(string.Format("{0} - {1} {2}", SettingsMain.ShopName, Resource.Admin_Leads_ItemNum, LeadId));
            }
            else
            {
                btnAddOrSaveLead.Text = Resource.Admin_Leads_AddLead;
                pnlNewLead.Visible = true;

                if (!string.IsNullOrWhiteSpace(Request["phone"]))
                {
                    txtPhone.Text = Request["phone"];
                    ddlOrderType.SelectedValue = OrderType.Phone.ToString();
                }

                var managerId = CustomerContext.CurrentCustomer.ManagerId;
                if (managerId != null && ddlManager.Items.FindByValue(managerId.Value.ToString()) != null)
                    ddlManager.SelectedValue = managerId.Value.ToString();

                var currency = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);
                orderItems.SetCurrency(currency.Iso3, currency.Rate, currency.NumIso3, currency.Symbol, currency.IsCodeBefore);

                SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Leads_AddNewLead));
            }
        }

        private void GetLead()
        {
            if (Lead == null)
                return;

            lblOrderId.Text = Lead.Id.ToString();
            lblOrderDate.Text = AdvantShop.Localization.Culture.ConvertDate(Lead.CreatedDate);

            var currency = Lead.LeadCurrency;
            if (currency != null)
            {
                CurrencyValue = currency.CurrencyValue;
                CurrencyCode = currency.CurrencyCode;

                orderItems.SetCurrency(currency.CurrencyCode, currency.CurrencyValue, currency.CurrencyNumCode,
                    currency.CurrencySymbol, currency.IsCodeBefore);
            }

            if (Lead.CustomerId != null)
                LoadCustomer((Guid)Lead.CustomerId);

            if (!string.IsNullOrWhiteSpace(Lead.Email))
                txtCustomerEmail.Text = Lead.Email;

            if (!string.IsNullOrWhiteSpace(Lead.FirstName))
                txtCustomerName.Text = Lead.FirstName;

            if (!string.IsNullOrWhiteSpace(Lead.Phone))
                txtPhone.Text = Lead.Phone;

            //txtUserComment.Text = Lead.Comment;
            //txtAdminComment.Text = Lead.AdminComment;

            if (Lead.ManagerId.HasValue && ddlManager.Items.FindByValue(Lead.ManagerId.ToString()) != null)
                ddlManager.SelectedValue = Lead.ManagerId.Value.ToString();

            //if (ddlLeadStatus.Items.FindByValue(Lead.LeadStatus.ToString()) != null)
            //    ddlLeadStatus.SelectedValue = Lead.LeadStatus.ToString();

            if (ddlOrderType.Items.FindByValue(Lead.OrderSourceId.ToString()) != null)
                ddlOrderType.SelectedValue = Lead.OrderSourceId.ToString();

            txtUserComment.Text = Lead.Comment;
            //txtAdminComment.Text = Lead.AdminComment;

            orderItems.IsLead = true;
            orderItems.OrderDiscount = Lead.Discount;

            orderItems.OrderItems.Clear();
            foreach (var leedItem in Lead.LeadItems)
            {
                orderItems.OrderItems.Add((OrderItem)leedItem);
            }

            createTaskBlock.Visible = true;
            createOrderBlock.Visible = true;
            hfTaskLeadId.Value = Lead.Id.ToString();

            var tasks = ManagerTaskService.GeTasksByLead(Lead.Id);
            if (tasks.Count > 0)
            {
                pnlTasks.Visible = true;
                lvTasks.DataSource = tasks;
                lvTasks.DataBind();
            }
        }

        private void LoadCustomer(Guid customerId)
        {
            hfCustomerId.Value = customerId.ToString();
            var customer = CustomerService.GetCustomer(customerId);

            if (customer == null)
                return;
            hlCustomer.Text = new string[] { customer.LastName, customer.FirstName, customer.Phone, customer.EMail }.Where(str => str.IsNotEmpty()).AggregateString(" ");
            hlCustomer.NavigateUrl = "ViewCustomer.aspx?CustomerID=" + customer.Id;

            txtCustomerEmail.Text = customer.EMail;
            txtCustomerName.Text = customer.LastName + " " + customer.FirstName;
            txtPhone.Text = customer.Phone;
        }

        private Lead AddOrUpdateLead()
        {
            var lead = new Lead()
            {
                FirstName = HttpUtility.HtmlEncode(txtCustomerName.Text),
                Email = HttpUtility.HtmlEncode(txtCustomerEmail.Text),
                Phone = HttpUtility.HtmlEncode(txtPhone.Text),
                Comment = HttpUtility.HtmlEncode(txtUserComment.Text),
                //AdminComment = HttpUtility.HtmlEncode(txtAdminComment.Text),
                OrderSourceId = Convert.ToInt32(ddlOrderType.SelectedValue),
                //LeadStatus = (LeadStatus)Enum.Parse(typeof(LeadStatus), ddlLeadStatus.SelectedValue, true),
                Discount = orderItems.OrderDiscount
            };

            Guid customerId;
            if (Guid.TryParse(hfCustomerId.Value, out customerId))
            {
                lead.CustomerId = customerId;
            }

            if (ddlManager.Visible && !string.IsNullOrWhiteSpace(ddlManager.SelectedValue))
            {
                lead.ManagerId = ddlManager.SelectedValue.TryParseInt(true);
            }

            lead.LeadCurrency = new LeadCurrency
            {
                CurrencyCode = orderItems.CurrencyCode,
                CurrencyValue = orderItems.CurrencyValue,
                CurrencyNumCode = orderItems.CurrencyNumCode,
                CurrencySymbol = orderItems.CurrencySymbol
            };

            lead.LeadItems = new List<LeadItem>();
            foreach (var orderItem in orderItems.OrderItems)
            {
                lead.LeadItems.Add((LeadItem)orderItem);
            }

            if (LeadId == 0)
            {
                lead.IsFromAdminArea = true;
                LeadService.AddLead(lead, true);
            }
            else
            {
                lead.Id = LeadId;
                LeadService.UpdateLead(lead);
            }

            return lead;
        }

        protected void lbCreateOrder_Click(object sender, EventArgs e)
        {
            Lead = LeadService.GetLead(LeadId);
            if (Lead == null)
            {
                return;
            }

            var order = OrderService.CreateOrder(Lead);
            if (order == null)
            {
                MsgErr(Resource.Admin_ManagerTask_Error_CreateOrder);
                return;
            }

            Response.Redirect("ViewOrder.aspx?orderid=" + order.OrderID);
        }

        private void MsgErr(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = message.IsNotEmpty();
        }
    }
}