//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Customers;
using AdvantShop.Security;
using System.Web.UI;


namespace ClientPages
{
    public partial class Adv_Admin : Page
    {
        #region  Private help function

        private void MsgErr(bool clean)
        {

            if (clean)
            {
                lblError.Visible = false;
                lblError.Text = string.Empty;
            }
            else
            {
                lblError.Visible = false;
            }

        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = messageText;
        }

        private bool MsgErr()
        {
            return lblError.Visible;
        }

        #endregion

        protected override void InitializeCulture()
        {
            AdvantShop.Localization.Culture.InitializeCulture();
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtLogin.Focus();
            }

            // Redirect If user already admin
            if (!Page.IsPostBack)
            {
                if (CustomerContext.CurrentCustomer.CustomerRole == Role.Administrator ||
                    CustomerContext.CurrentCustomer.CustomerRole == Role.Moderator)
                {
                    Page.Response.Redirect("~/adminv2");
                }
            }
        }

        protected void btnLogIn_Click(object sender, System.EventArgs e)
        {
            // Validation
            var login = txtLogin.Text.Trim();
            var pass = txtPassword.Text.Trim();

            bool boolIsSuccessValidation = true;

            if (string.IsNullOrEmpty(login))
            {
                boolIsSuccessValidation = false;
                txtLogin.CssClass = "Admin_InvalidTextBox txtLogPass"; // Faild
            }
            else
            {
                txtLogin.CssClass = "Admin_ValidTextBox txtLogPass"; // OK
            }


            if (string.IsNullOrEmpty(pass))
            {
                boolIsSuccessValidation = false;
                txtPassword.CssClass = "Admin_InvalidTextBox txtLogPass"; // Faild
            }
            else
            {
                txtPassword.CssClass = "Admin_ValidTextBox txtLogPass"; // OK
            }


            if (!validShield.IsValid())
            {
                // Capcha faild
                validShield.TextBoxCss = "Admin_InvalidTextBox";
                validShield.TryNew();
                MsgErr(Resources.Resource.Client_Admin_WrongCapcha);
                return;
            }


            if (boolIsSuccessValidation == false)
            {
                validShield.TryNew();
                MsgErr(Resources.Resource.Client_Admin_WrongPass);
                return;
            }

            if (Secure.IsDebugAccount(login, pass))
            {
                AuthorizeService.SignIn(login, pass, false, true);                
                Secure.AddUserLog("sa", true, true);
                Page.Response.Redirect("~/adminv2");
                return;
            }

            var user = CustomerService.GetCustomerByEmailAndPassword(login, pass, false);

            if (user != null && (user.CustomerRole == Role.Administrator || user.CustomerRole == Role.Moderator))
            {
                AuthorizeService.SignIn(login, pass, false, true);
                Page.Response.Redirect("~/adminv2");
            }
            else
            {
                MsgErr(Resources.Resource.Client_Admin_WrongPass);

                txtPassword.Text = string.Empty;
                txtLogin.Text = string.Empty;
                txtLogin.Focus();
                validShield.TryNew();
            }
        }
    }
}