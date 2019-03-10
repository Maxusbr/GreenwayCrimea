//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using Resources;
using AdvantShop.Saas;

namespace Admin
{
    public partial class CreateCustomer : AdvantShopAdminPage
    {
        protected Guid CustomerId;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            CustomerId = Request["CustomerId"].TryParseGuid();
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_CreateCustomer_Header));

            if (!Page.IsPostBack)
            {
                //Check item count for region dropDownList
                ddlCustomerRole.Items.Add(new ListItem(Resource.Admin_ViewCustomer_CustomerRole_User, ((int)Role.User).ToString()));
                ddlCustomerRole.Items.Add(new ListItem(Resource.Admin_ViewCustomer_CustomerRole_Moderator, ((int)Role.Moderator).ToString()));
                if (CustomerContext.CurrentCustomer.IsAdmin)
                    ddlCustomerRole.Items.Add(new ListItem(Resource.Admin_ViewCustomer_CustomerRole_Administrator, ((int)Role.Administrator).ToString()));


                if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.RoleActions)
                {
                    ddlCustomerRole.Items.FindByValue(((int)Role.Moderator).ToString()).Attributes.Add("disabled", "disabled");
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request["role"]))
                    {
                        var role = Request["role"].TryParseEnum<Role>();
                        ddlCustomerRole.SelectedValue = ((int)role).ToString();
                    }
                }

                txtPhone.Text = Request["phone"];
            }
            foreach (var group in CustomerGroupService.GetCustomerGroupList())
            {
                ddlCustomerGroup.Items.Add(new ListItem(string.Format("{0} - {1}%", group.GroupName, group.GroupDiscount), group.CustomerGroupId.ToString()));
            }
            userRoleTr.Visible = CustomerContext.CurrentCustomer.IsAdmin;


        }

        protected void btnChangeCommonInfo_Click(object sender, EventArgs e)
        {
            if (DataValidation())
            {
                var roleId = userRoleTr.Visible ? (Role)SQLDataHelper.GetInt(ddlCustomerRole.SelectedValue) : Role.User;

                if (CustomerContext.CurrentCustomer.IsModerator && roleId == Role.Administrator)
                    roleId = Role.User;

                int groupId;
                Int32.TryParse(ddlCustomerGroup.SelectedValue, out groupId);

                var res = CustomerService.ExistsEmail(txtEmail.Text);
                if (res)
                {
                    MsgErr(Resource.Admin_CreateCustomer_CustomerErrorEmailExist);
                    return;
                }

                var customerId = CustomerService.InsertNewCustomer(new Customer
                {
                    Id = CustomerId,
                    Password = txtPassword.Text,
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Patronymic = SettingsCheckout.IsShowPatronymic ? txtPatronymic.Text : string.Empty,
                    Phone = txtPhone.Text,
                    StandardPhone = StringHelper.ConvertToStandardPhone(txtPhone.Text),
                    SubscribedForNews = chkSubscribed4News.Checked,
                    EMail = txtEmail.Text,
                    CustomerRole = roleId,
                    CustomerGroupId = groupId
                });

                if (!customerId.Equals(Guid.Empty))
                {
                    Response.Redirect("ViewCustomer.aspx?CustomerID=" + customerId);
                }
                else
                {
                    MsgErr(Resource.Admin_CreateCustomer_CustomerError);
                }
            }
        }

        private bool DataValidation()
        {
            bool boolIsValidPast = true;

            ulUserRegistarionValidation.InnerHtml = string.Empty;


            // begin password
            if (!string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text.Length >= 6)
            {
                txtPassword.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtPassword.CssClass = "OrderConfirmation_InvalidTextBox";
                ulUserRegistarionValidation.InnerHtml += string.Format("<li>{0}</li>",
                                                                       Resource.Client_Registration_PasswordLenght);
                boolIsValidPast = false;
            }

            if (!string.IsNullOrEmpty(txtPasswordConfirm.Text))
            {
                txtPasswordConfirm.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtPasswordConfirm.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if ((!string.IsNullOrEmpty(txtPasswordConfirm.Text)) &&
                (!string.IsNullOrEmpty(txtPassword.Text)) && (txtPassword.Text == txtPasswordConfirm.Text))
            {
                txtPassword.CssClass = "OrderConfirmation_ValidTextBox";
                txtPasswordConfirm.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtPassword.CssClass = "OrderConfirmation_InvalidTextBox";
                txtPasswordConfirm.CssClass = "OrderConfirmation_InvalidTextBox";
                ulUserRegistarionValidation.InnerHtml += string.Format("<li>{0}</li>", Resource.Client_Registration_PasswordNotMatch);
                boolIsValidPast = false;
            }
            // begin password

            if (!string.IsNullOrEmpty(txtFirstName.Text))
            {
                txtFirstName.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtFirstName.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if (!string.IsNullOrEmpty(txtLastName.Text))
            {
                txtLastName.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtLastName.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                txtEmail.CssClass = "OrderConfirmation_ValidTextBox";

                if (ValidationHelper.IsValidEmail(txtEmail.Text))
                {
                    txtEmail.CssClass = "OrderConfirmation_ValidTextBox";
                }
                else
                {
                    txtEmail.CssClass = "OrderConfirmation_InvalidTextBox";
                    boolIsValidPast = false;
                }
            }
            else
            {
                txtEmail.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            // ------------------------------------------------------

            if (!boolIsValidPast)
            {
                ulUserRegistarionValidation.Visible = true;
                ulUserRegistarionValidation.InnerHtml += string.Format("<li>{0}</li>", Resource.Client_OrderConfirmation_EnterEmptyField);
                return false;
            }
            ulUserRegistarionValidation.Visible = false;
            return true;
        }

    }
}