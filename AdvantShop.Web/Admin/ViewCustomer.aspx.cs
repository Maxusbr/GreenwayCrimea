//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using Resources;

namespace Admin
{
    public partial class ViewCustomer : AdvantShopAdminPage
    {
        #region Fields

        protected Guid CustomerId;
        protected int Rating;

        protected string BonusCardNumber;
        protected string BonusCardAmount;
        protected string BonusCardGrade;

        protected Customer CurrentCustomer;
        protected bool ShowRoleAccess = false;
        protected string Code;

        #endregion

        #region Help methods

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
            Message.Text = @"<br/>" + messageText;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            MsgErr(true);
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ViewCustomer_Header));

            if (string.IsNullOrEmpty(Request["customerid"]))
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                CustomerId = Request["customerid"].TryParseGuid();
                CurrentCustomer = CustomerService.GetCustomer(CustomerId);
            }

            if (Request["code"].IsNotEmpty())
            {
                if (CurrentCustomer == null)
                    CurrentCustomer = ClientCodeService.GetCustomerByCode(Request["code"], CustomerId);

                var codeInt = Request["code"].Replace("-", "").TryParseInt();
                Code = codeInt < 1000
                        ? codeInt.ToString("###")
                        : (codeInt < 1000000 ? codeInt.ToString("####-###") : codeInt.ToString("####-####-###"));
            }


            if (CurrentCustomer == null)
                Response.Redirect("default.aspx");

            if (IsPostBack) return;

            lblCustomerName.Text = CurrentCustomer.LastName + @" " + CurrentCustomer.FirstName;

            foreach (var group in CustomerGroupService.GetCustomerGroupList())
            {
                ddlCustomerGroup.Items.Add(new ListItem(string.Format("{0} - {1}%", group.GroupName, group.GroupDiscount), group.CustomerGroupId.ToString()));
            }
            ddlCustomerGroup.SelectedValue = CurrentCustomer.CustomerGroupId.ToString();
            ddlCustomerGroup.Attributes["data-customerid"] = CustomerId.ToString();


            lblRegistrationDate.Text = AdvantShop.Localization.Culture.ConvertDate(CurrentCustomer.RegistrationDateTime);
            lblCustomerPhone.Text = CurrentCustomer.Phone;

            if (SettingsTelephony.PhonerLiteActive &&
                (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTelephony)) &&
                CurrentCustomer.StandardPhone.HasValue)
            {
                lblCustomerPhone.Text = string.Format("<a href=\"callto:{0}\">{1}</a>", CurrentCustomer.StandardPhone, CurrentCustomer.Phone);
            }

            if (CurrentCustomer.StandardPhone.HasValue)
            {
                lblCustomerPhone.Text += " " + IPTelephonyOperator.Current.RenderCallButton(CurrentCustomer.StandardPhone, ECallButtonType.Big);
                foreach (var classInstance in AttachedModules.GetModules<IModuleSms>().Select(smsModule => (IModuleSms)Activator.CreateInstance(smsModule)))
                {
                    lblCustomerPhone.Text += " " + classInstance.RenderSendSmsButton(CurrentCustomer.Id, CurrentCustomer.StandardPhone.Value);
                    break;
                }
            }
            lblCustomerEmail.Text = CurrentCustomer.EMail;
            if (CurrentCustomer.EMail.IsNotEmpty())
                ckbSubscribed4News.Checked = SubscriptionService.IsSubscribe(CurrentCustomer.EMail);
            ckbSubscribed4News.Attributes.Add("data-email", CurrentCustomer.EMail);

            txtAdminComment.Text = CurrentCustomer.AdminComment;
            txtAdminComment.Attributes.Add("data-customerid", CustomerId.ToString());
            txtAdminComment.Attributes.Add("data-currentvalue", CurrentCustomer.AdminComment);

            Rating = CurrentCustomer.Rating;

            if (CurrentCustomer.Contacts.Count > 0)
            {
                lblCustomerContactCountry.Text = CurrentCustomer.Contacts[0].Country;
                lblCustomerContactCity.Text = CurrentCustomer.Contacts[0].City;
                lblCustomerContactZone.Text = CurrentCustomer.Contacts[0].Region;
                lblCustomerContactZip.Text = CurrentCustomer.Contacts[0].Zip;
                lblCustomerContactAddress.Text = CurrentCustomer.Contacts[0].Street;

            }

            lvCustomerOrders.DataSource = OrderService.GetCustomerOrderHistory(CurrentCustomer.Id);
            lvCustomerOrders.DataBind();

            if (SettingsCheckout.EnableManagersModule && (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm)))
            {
                blockManagers.Visible = true;
                if (CustomerContext.CurrentCustomer.IsAdmin || CustomerContext.CurrentCustomer.IsModerator || CustomerContext.CurrentCustomer.HasRoleAction(RoleAction.Crm))
                {
                    ddlViewCustomerManager.Visible = true;
                    ddlViewCustomerManager.Items.Add(new ListItem("-", ""));
                    foreach (var manager in ManagerService.GetManagersList())
                    {
                        ddlViewCustomerManager.Items.Add(
                            new ListItem(string.Format("{0} {1}", manager.FirstName, manager.LastName),
                                manager.ManagerId.ToString()));
                    }

                    if (CurrentCustomer.ManagerId != null)
                    {
                        ddlViewCustomerManager.SelectedValue = CurrentCustomer.ManagerId.ToString();
                    }

                    ddlViewCustomerManager.Attributes["data-customerid"] = CustomerId.ToString();
                }
                else
                {
                    if (CurrentCustomer.ManagerId == null)
                    {
                        lblCustomerManager.Text = @"-";
                    }
                    else
                    {
                        var manager = ManagerService.GetManager((int)CurrentCustomer.ManagerId);
                        lblCustomerManager.Text = manager != null
                            ? string.Format("{0} {1}", manager.FirstName, manager.LastName)
                            : "-";
                    }
                }
            }

            if (!SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCustomerLog)
            {
                lvEmailLog.DataSource = CurrentCustomer.EMail.IsNotEmpty() ? CustomerService.GetEmails(CurrentCustomer.Id, CurrentCustomer.EMail) : null;
                lvEmailLog.DataBind();


                lvSms.DataSource = CurrentCustomer.StandardPhone.HasValue
                    ? CustomerService.GetSms(CurrentCustomer.Id, CurrentCustomer.StandardPhone.Value)
                    : null;
                lvSms.DataBind();

                CallLog.Calls = CurrentCustomer.StandardPhone.HasValue
                    ? CallService.GetCalls(CurrentCustomer.StandardPhone.Value)
                    : null;

                ActivitiLog.ActivityEvents = CustomerService.GetEvent(CurrentCustomer.Id);
            }
            else
            {
                pnlCustomerActivity.Visible = false;
                LogingNotAvailable.Visible = true;

                if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCustomerLog)
                {
                    LogingNotAvailable.Text = Resource.Admin_ViewCustomer_UnavailableLoging;
                }
            }

            if (!SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCrm)
            {
                CustomerLeads.CustomerId = CurrentCustomer.Id;
                CustomerLeads.Phone = CurrentCustomer.StandardPhone.HasValue
                    ? CurrentCustomer.StandardPhone.ToString()
                    : null;
            }
            else
            {
                CustomerLeads.Visible = false;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            bool bonusSystemIsActive = BonusSystem.IsActive;

            if (bonusSystemIsActive)
            {
                //divBonusCard.Visible = true;
                liBonusAmount.Visible = false;

                var card = BonusSystemService.GetCard(CurrentCustomer.Id);
                if (card != null && !card.Blocked)
                {
                    BonusCardNumber = card.CardNumber.ToString();
                    BonusCardAmount = card.BonusAmount.ToString();
                    BonusCardGrade = card.Grade.BonusPercent.ToString();

                    lblBonusCardNumber.Text = BonusCardNumber;
                    lblBonusCardAmount.Text = BonusCardAmount;
                    lblBonusCardGrade.Text = BonusCardGrade;

                    //lblCustomerSegment.Text = card.CityName + @", " + (card.Gender ? Resource.Admin_Females : Resource.Admin_Males);
                    
                    liBonusAmount.Visible = true;
                }
            }

            if (!bonusSystemIsActive)
            {
                indicatorsBlock.Visible = false;
            }

        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            //_paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }
    }
}