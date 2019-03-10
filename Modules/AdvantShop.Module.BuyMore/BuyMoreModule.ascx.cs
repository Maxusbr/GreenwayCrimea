using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Helpers;
using AdvantShop.Module.BuyMore.Domain;
using AdvantShop.Shipping;
using System.Collections.Generic;
using System.Linq;

namespace Advantshop.Modules.UserControls.BuyMore
{
    public partial class Admin_BuyMoreModule : UserControl
    {
        protected List<ShippingMethod> ShippingMethodList;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            ShippingMethodList = ShippingMethodService.GetAllShippingMethods();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cblExcludedShippings.Items.AddRange(ShippingMethodList.Select(x => new ListItem(x.Name, x.ShippingMethodId.ToString())).ToArray());
            }

            var excludedShippingsIds = ModuleSettingsProvider.GetSettingValue<string>("ExcludedShippingsIds", AdvantShop.Module.BuyMore.BuyMore.ModuleStringId);
            foreach (ListItem item in cblExcludedShippings.Items)
            {
                item.Selected = excludedShippingsIds != null ? excludedShippingsIds.Split(',').Contains(item.Value) : false;
            }

            txtShippingPriceTo.Text = ModuleSettingsProvider.GetSettingValue<float>("ShippingPriceTo", AdvantShop.Module.BuyMore.BuyMore.ModuleStringId).ToString();

            rbDisplayAlways.Checked = ModuleSettingsProvider.GetSettingValue<bool>("DisplayAlways", AdvantShop.Module.BuyMore.BuyMore.ModuleStringId);
            rbDisplayMissing.Checked = !rbDisplayAlways.Checked;

            float missingDiscount = ModuleSettingsProvider.GetSettingValue<float>("MissingDiscount", AdvantShop.Module.BuyMore.BuyMore.ModuleStringId);

            txtMissingDiscount.Text = missingDiscount.ToString();
            lSample.Text = (1000 - 1000 / 100 * missingDiscount).ToString();

            rprProducts.DataSource = BuyMoreService.GetAll();
            rprProducts.DataBind();
        }

        protected void rprProducts_ItemCommand(object source, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                BuyMoreService.Delete(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ModuleSettingsProvider.SetSettingValue("ShippingPriceTo", txtShippingPriceTo.Text.TryParseFloat(), AdvantShop.Module.BuyMore.BuyMore.ModuleStringId);

            var missingDiscount = txtMissingDiscount.Text.TryParseFloat();
            if (missingDiscount > 0 && missingDiscount < 100)
            {
                ModuleSettingsProvider.SetSettingValue("MissingDiscount", missingDiscount, AdvantShop.Module.BuyMore.BuyMore.ModuleStringId);
            }

            ModuleSettingsProvider.SetSettingValue("DisplayAlways", rbDisplayAlways.Checked, AdvantShop.Module.BuyMore.BuyMore.ModuleStringId);

            var excludedShippingsIds = string.Empty;
            foreach (ListItem item in cblExcludedShippings.Items)
            {
                if (item.Selected)
                    excludedShippingsIds += "," + item.Value;
            }
            ModuleSettingsProvider.SetSettingValue("ExcludedShippingsIds", excludedShippingsIds.Trim(','), AdvantShop.Module.BuyMore.BuyMore.ModuleStringId);

            CacheManager.RemoveByPattern(CacheNames.ShippingOptions);
        }

        protected string RenderProduct(string val)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(val))
            {
                foreach (var offerId in val.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var offer = OfferService.GetOffer(Convert.ToInt32(offerId));
                    result += (offer != null ? offer.ArtNo + "," + offer.Product.Name : "deleted") + "<br>";
                }
            }
            return result;

        }

    }
}