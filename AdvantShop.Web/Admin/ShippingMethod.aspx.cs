//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using Admin.UserControls.ShippingMethods;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping;
using AdvantShop.Trial;
using System;
using System.Linq;
using AdvantShop.Taxes;

namespace Admin
{
    public partial class EditShippingMethod : AdvantShopAdminPage
    {
        private int _shippingMethodID;
        protected int ShippingMethodID
        {
            get
            {
                if (_shippingMethodID != 0)
                    return _shippingMethodID;
                var intval = 0;
                int.TryParse(Request["shippingmethodid"], out intval);
                return intval;
            }
            set
            {
                _shippingMethodID = value;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resources.Resource.Admin_ShippingMethod_Header));

            ClearMsg();
            if (!IsPostBack)
                LoadMethods();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlType.DataSource = AdvantShop.Core.AdvantshopConfigService.GetDropdownShippings();
            ddlType.DataBind();
        }

        protected void LoadMethods()
        {
            var methods = ShippingMethodService.GetAllShippingMethods().Where(item => item.ShippingMethodId != 1).ToList();
            if (methods.Count > 0)
            {
                if (ShippingMethodID == 0)
                    ShippingMethodID = methods.First().ShippingMethodId;
                rptTabs.DataSource = methods;
                rptTabs.DataBind();
            }
            else
                pnEmpty.Visible = true;

            ShowMethod(ShippingMethodID);
        }

        protected void ShowMethod(int methodId)
        {
            var method = ShippingMethodService.GetShippingMethod(methodId);
            var listKeys = ReflectionExt.GetAttributeValue<ShippingKeyAttribute>(typeof(BaseShipping));
            foreach (var ucId in listKeys)
            {
                var uc = (MasterControl)pnMethods.FindControl("uc" + ucId);
                if (uc == null)
                {
                    continue;
                }
                if (method == null)
                {
                    uc.Visible = false;
                    continue;
                }
                if (ucId.ToLower() == method.ShippingType.ToLower())
                    uc.Method = method;
                uc.Visible = ucId.ToLower() == method.ShippingType.ToLower();
                //if (ucId == method.ShippingType)
                //    uc.Method = method;
                //uc.Visible = ucId == method.ShippingType;
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            var type = ddlType.SelectedValue;
            var method = new ShippingMethod
            {
                ShippingType = type,
                Name = txtName.Text,
                Description = txtDescription.Text,
                Enabled = type == "FreeShipping",
                DisplayCustomFields = true,
                SortOrder = txtSortOrder.Text.TryParseInt(),
                ZeroPriceMessage = Resources.Resource.Admin_ShippingMethod_ZeroPriceMessage,
                TaxType = TaxType.Without
            };

            TrialService.TrackEvent(TrialEvents.AddShippingMethod, method.ShippingType);
            var id = ShippingMethodService.InsertShippingMethod(method);
            if (id != 0)
                Response.Redirect("~/Admin/ShippingMethod.aspx?ShippingMethodID=" + id);
        }

        protected void ShippingMethod_Saved(object sender, MasterControl.SavedEventArgs args)
        {
            LoadMethods();
            Msg(string.Format(Resources.Resource.Admin_ShippingMethod_Saved, args.Name));
        }
    }
}