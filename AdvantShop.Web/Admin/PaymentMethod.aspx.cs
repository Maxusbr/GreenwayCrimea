//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Admin.UserControls.PaymentMethods;
using AdvantShop.Core;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;
using AdvantShop.Configuration;
using AdvantShop.Trial;

namespace Admin
{
    public partial class EditPaymentMethod : AdvantShopAdminPage
    {

        private int _paymentMethodId;
        protected int PaymentMethodId
        {
            get
            {
                if (_paymentMethodId != 0)
                    return _paymentMethodId;
                var intval = 0;
                int.TryParse(Request["paymentmethodid"], out intval);
                return intval;
            }

            set { _paymentMethodId = value; }
        }
        

        public bool IsIframe
        {
            get
            {
                if (Request["iframe"] != null && Request["iframe"] == "true")
                    return true;

                return false;
            }
        }


        protected void Msg(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }

        protected void ClearMsg()
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (IsIframe)
            {
                MasterPageFile = "~/Admin/MasterPageEmpty.master";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resources.Resource.Admin_PaymentMethod_Header));

            ClearMsg();
            if (!IsPostBack)
                LoadMethods();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlType.DataSource = AdvantshopConfigService.GetDropdownPayments();
            ddlType.DataBind();
        }

        protected void LoadMethods()
        {
            var methods = PaymentService.GetAllPaymentMethods(false).ToList();
            if (methods.Count > 0)
            {
                if (PaymentMethodId == 0)
                    PaymentMethodId = methods.First().PaymentMethodId;
                rptTabs.DataSource = methods;
                rptTabs.DataBind();

            }
            else
                pnEmpty.Visible = true;

            ShowMethod(PaymentMethodId);
        }

        protected void ShowMethod(int methodId)
        {
            //var method = PaymentService.GetPaymentMethod(methodId);
            //var uc = (MasterControl)pnMethods.FindControl("uc"+method.PaymentKey);
            //if (uc != null)
            //    uc.Visible = true;
            //else
            //{
            //    uc.Visible = false;
            //}
            
            var method = PaymentService.GetPaymentMethod(methodId);
            var listKeys = ReflectionExt.GetAttributeValue<PaymentKeyAttribute>(typeof(PaymentMethod));

            foreach (var ucId in listKeys)
            {
                var uc = (MasterControl)pnMethods.FindControl("uc" + ucId);
                if (uc == null)
                    continue;

                if (method == null)
                {
                    uc.Visible = false;
                    continue;
                }
                if (ucId == method.PaymentKey)
                    uc.Method = method;
                uc.Visible = ucId == method.PaymentKey;
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            var type = ddlType.SelectedValue;
            var method = PaymentMethod.Create(type);
            if (method == null)
                return;

            method.Name = txtName.Text;
            method.Description = txtDescription.Text;
            if (!string.IsNullOrEmpty(txtSortOrder.Text))
                method.SortOrder = int.Parse(txtSortOrder.Text);
            method.Enabled = (method is AdvantShop.Payment.Cash);
            //Some dirty magic
            if (method.Parameters.ContainsKey(AssistTemplate.CurrencyValue))
            {
                var parameters = method.Parameters;
                parameters[AssistTemplate.CurrencyValue] = "1";
                method.Parameters = parameters;
            }
            //End of dirty magic
            TrialService.TrackEvent(TrialEvents.AddPaymentMethod, method.PaymentKey);
            var id = PaymentService.AddPaymentMethod(method);
            if (id != 0)
                Response.Redirect("~/Admin/PaymentMethod.aspx?PaymentMethodID=" + id);
        }

        protected void PaymentMethod_Saved(object sender, MasterControl.SavedEventArgs args)
        {
            LoadMethods();
            Msg(string.Format(Resources.Resource.Admin_PaymentMethod_Saved, args.Name));
        }

        protected void PaymentMethod_Error(object arg1, MasterControl.ErrorEventArgs arg2)
        {
            Msg(arg2.Message);
        }
    }
}