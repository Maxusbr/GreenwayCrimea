//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using Resources;


namespace Admin
{
    public partial class EditManagerTask : AdvantShopAdminPage
    {

        private void SetConrtolValidationStyle(System.Web.UI.WebControls.WebControl control, bool valid)
        {
            control.CssClass += valid ? "niceTextBox_faild" : "niceTextBox";
        }

        private List<string> _errors = new List<string>();

        private void MsgErr(string message)
        {
            if (message.IsNullOrEmpty())
                _errors.Clear();

            _errors.Add(message);

            lblMessage.Text = string.Join("<br> ", _errors);
            lblMessage.Visible = _errors != null && _errors.Count > 0;
        }

        protected int TaskId
        {
            get { return Request["taskid"].TryParseInt(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MsgErr(string.Empty);
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ManagerTask_SubHeader));
            
            if (TaskId == 0 && !CustomerContext.CurrentCustomer.IsManager)
            {
                btnSave.Visible = false;
                MsgErr(Resource.Admin_ManagerTask_ShoudBeManager);
            }

            if (IsPostBack)
                return;

            if (TaskId != 0)
            {
                var task = ManagerTaskService.GetManagerTask(TaskId);
                if (task == null)
                {
                    Response.Redirect("default.aspx");
                }

                LoadTask(task);
                lblTitle.Text = Resource.Admin_ManagerTask_Number + task.TaskId;
            }
            else
            {
                hlAppointedManager.Text = string.Format("{0} {1}", CustomerContext.CurrentCustomer.FirstName, CustomerContext.CurrentCustomer.LastName);
                hlAppointedManager.NavigateUrl = UrlService.GetAdminAbsoluteLink("ViewCustomer.aspx?CustomerID=" + CustomerContext.CurrentCustomer.Id);
                txtDueDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
                txtDueMinutes.Text = DateTime.Now.ToString("HH:mm");
                lblTitle.Text = Resource.Admin_ManagerTask_NewTask;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (PopupGridManagers.SelectedManagers != null && PopupGridManagers.SelectedManagers.Any())
            {
                var manager = ManagerService.GetManager(PopupGridManagers.SelectedManagers.First());
                hfAssignedManagerId.Value = manager.ManagerId.ToString();
                hlAssignedManager.Text = string.Format("{0} {1}", manager.FirstName, manager.LastName);
                hlAssignedManager.NavigateUrl = UrlService.GetAdminAbsoluteLink("ViewCustomer.aspx?CustomerID=" + manager.CustomerId);

                PopupGridManagers.CleanSelection();
            }
            if (PopupGridCustomers.SelectedCustomers != null && PopupGridCustomers.SelectedCustomers.Any())
            {
                var clientCustomer = CustomerService.GetCustomer(PopupGridCustomers.SelectedCustomers.First());
                SetClientCustomer(clientCustomer);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValidData())
            {
                MsgErr(Resource.Admn_NewsCategory_WrongData);
                return;
            }
            
            if (TaskId == 0)
            {
                var appointedManager = ManagerService.GetManager(CustomerContext.CustomerId);
                if (appointedManager == null)
                {
                    MsgErr(Resource.Admin_ManagerTask_ShoudBeManager);
                    return;
                }
                AddTask(appointedManager);
            }
            else
            {
                UpdateTask();
            }
            lblMessageSuccess.Text = Resource.Admin_m_Category_SaveSuccesed;
            lblMessageSuccess.Visible = true;
        }

        public void LoadTask(ManagerTask task)
        {
            ddlStatus.SelectedValue = task.Status.ConvertIntString();
            txtName.Text = task.Name;
            txtDescription.Text = task.Description;

            hfAssignedManagerId.Value = task.AssignedManagerId.ToString();
            hlAssignedManager.Text = string.Format("{0} {1}", task.AssignedManager.FirstName, task.AssignedManager.LastName);
            hlAssignedManager.NavigateUrl = UrlService.GetAdminAbsoluteLink("ViewCustomer.aspx?CustomerID=" + task.AssignedManager.CustomerId);

            hlAppointedManager.Text = string.Format("{0} {1}", task.AppointedManager.FirstName, task.AppointedManager.LastName);
            hlAppointedManager.NavigateUrl = UrlService.GetAdminAbsoluteLink("ViewCustomer.aspx?CustomerID=" + task.AppointedManager.CustomerId);

            txtDueDate.Text = task.DueDate.ToString("dd.MM.yyyy");
            txtDueMinutes.Text = task.DueDate.ToString("hh:mm");
            lblDateCreated.Text = task.DateCreated.ToString("dd.MM.yyyy HH:mm");

            if (task.OrderId.HasValue)
            {
                var order = OrderService.GetOrder(task.OrderId.Value);
                if (order != null)
                    txtOrderId.Text = order.Number;
            }

            if (task.LeadId != null)
            {
                leadDiv.Visible = true;
                lbCreateOrder.Visible = true;

                hlLead.Text = "Лид № " + task.LeadId;
                hlLead.NavigateUrl = UrlService.GetAdminAbsoluteLink("EditLead.aspx?id=" + task.LeadId);
            }

            txtResultShort.Text = task.ResultShort;
            txtResultFull.Text = task.ResultFull;

            SetClientCustomer(task.ClientCustomer);
        }

        public void AddTask(Manager appointedManager)
        {
            var task = new ManagerTask
            {
                Status = (ManagerTaskStatus) (ddlStatus.SelectedValue.TryParseInt()),
                Name = txtName.Text,
                Description = txtDescription.Text,
                AssignedManagerId = hfAssignedManagerId.Value.TryParseInt(),
                AppointedManagerId = appointedManager.ManagerId,
                DueDate = (txtDueDate.Text + " " + txtDueMinutes.Text).TryParseDateTime(DateTime.Now),
                ResultShort = txtResultShort.Text,
                ResultFull = txtResultFull.Text
            };

            GetAdditionalFields(task);

            task.TaskId = ManagerTaskService.AddManagerTask(task);
            
            ManagerTaskService.OnSetManagerTask(task);
            Response.Redirect("ManagersTasks.aspx");
        }

        public void UpdateTask()
        {
            var task = ManagerTaskService.GetManagerTask(TaskId);
            var prevStatus = task.Status;
            var prevAssignedManagerId = task.AssignedManagerId;

            task.Status = (ManagerTaskStatus)(ddlStatus.SelectedValue.TryParseInt());
            task.Name = txtName.Text;
            task.Description = txtDescription.Text;
            task.AssignedManagerId = hfAssignedManagerId.Value.TryParseInt();
            task.DueDate = (txtDueDate.Text + " " + txtDueMinutes.Text).TryParseDateTime(DateTime.Now);
            task.ResultShort = txtResultShort.Text;
            task.ResultFull = txtResultFull.Text;

            GetAdditionalFields(task);

            ManagerTaskService.UpdateManagerTask(task);

            if (task.AssignedManagerId != prevAssignedManagerId)
                ManagerTaskService.OnSetManagerTask(task);
            if (task.Status != prevStatus)
                ManagerTaskService.OnChangeManagerTaskStatus(task);

            LoadTask(task);
        }

        protected bool IsValidData()
        {
            bool valid = true;
                       
            SetConrtolValidationStyle(txtName, valid &= txtName.Text.IsNotEmpty());
                        
            SetConrtolValidationStyle(txtDescription, valid &= txtDescription.Text.IsNotEmpty());

            valid &= hfAssignedManagerId.Value.IsInt();
            if (!hfAssignedManagerId.Value.IsInt())
            {
                MsgErr(Resource.Admn_ManagerTask_ManagerNotExit);
            }

            DateTime date;
            valid &= DateTime.TryParse(txtDueDate.Text + " " + txtDueMinutes.Text, out date);

            SetConrtolValidationStyle(txtDueDate, DateTime.TryParse(txtDueDate.Text + " " + txtDueMinutes.Text, out date));
            SetConrtolValidationStyle(txtDueMinutes, DateTime.TryParse(txtDueDate.Text + " " + txtDueMinutes.Text, out date));

            if (!string.IsNullOrWhiteSpace(txtOrderId.Text))
            {
                var existOrder = OrderService.GetOrderByNumber(txtOrderId.Text.Trim()) != null;
                valid &= existOrder;

                if (!existOrder)
                    MsgErr(Resource.Admn_ManagerTask_OrderNotExist);
            }
            return valid;
        }

        protected void ibRemoveClientCustomer_Click(object sender, ImageClickEventArgs e)
        {
            SetClientCustomer(null);
        }

        private void GetAdditionalFields(ManagerTask task)
        {
            var orderNumber = txtOrderId.Text;
            if (!string.IsNullOrWhiteSpace(orderNumber))
            {
                var order = OrderService.GetOrderByNumber(orderNumber);
                if (order != null)
                    task.OrderId = order.OrderID;
            }

            Customer customer;
            if (hfClientCustomerId.Value.IsNotEmpty() &&
                (customer = CustomerService.GetCustomer(hfClientCustomerId.Value.TryParseGuid())) != null)
            {
                task.CustomerId = customer.Id;
            }
            else
            {
                task.CustomerId = null;
            }
        }

        private void SetClientCustomer(Customer clientCustomer)
        {
            if (clientCustomer == null)
            {
                hfClientCustomerId.Value = string.Empty;
                hlClientCustomer.Visible = ibRemoveClientCustomer.Visible = false;
                lblClientNotSelected.Visible = true;
            }
            else
            {
                hfClientCustomerId.Value = clientCustomer.Id.ToString();
                hlClientCustomer.Text = string.Format("{0} {1}", clientCustomer.FirstName, clientCustomer.LastName);
                hlClientCustomer.NavigateUrl = UrlService.GetAdminAbsoluteLink("ViewCustomer.aspx?CustomerID=" + clientCustomer.Id);
                hlClientCustomer.Visible = ibRemoveClientCustomer.Visible = true;

                if (clientCustomer.StandardPhone.HasValue)
                {
                    pnlPhone.Visible = true;
                    pnlPhone.Text =
                        SettingsTelephony.PhonerLiteActive &&
                        (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTelephony))
                            ? String.Format("<a href=\"callto:{0}\">{1}</a>", clientCustomer.StandardPhone, clientCustomer.Phone)
                            : clientCustomer.StandardPhone.ToString();
                    
                    pnlPhone.Text += " " + IPTelephonyOperator.Current.RenderCallButton(clientCustomer.StandardPhone, ECallButtonType.Big);
                }
                else
                {
                    pnlPhone.Visible = false;
                }

                lblClientNotSelected.Visible = false;
            }
            PopupGridCustomers.CleanSelection();
        }

        protected void lbCreateOrder_Click(object sender, EventArgs e)
        {
            if (TaskId == 0)
                return;

            var task = ManagerTaskService.GetManagerTask(TaskId);

            var order = CreateOrder(task);
            if (order == null)
            {
                MsgErr(Resource.Admin_ManagerTask_Error_CreateOrder);
                return;
            }
            
            if (task.CustomerId == null)
            {
                if (order.OrderCustomer != null && CustomerService.GetCustomer(order.OrderCustomer.CustomerID) != null)
                {
                    task.CustomerId = order.OrderCustomer.CustomerID;
                }
            }

            task.OrderId = order.OrderID;
            task.Status = ManagerTaskStatus.Closed;

            ManagerTaskService.UpdateManagerTask(task);

            Response.Redirect("ViewOrder.aspx?orderid=" + order.OrderID);
        }

        private Order CreateOrder(ManagerTask task)
        {
            if (task.LeadId == null)
                return null;
            
            var lead = LeadService.GetLead((int)task.LeadId);
            if (lead == null)
                return null;

            var order = OrderService.CreateOrder(lead);

            return order;
        }
    }
}