using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Modules;
using AdvantShop.Helpers;
using AdvantShop.Module.BuyInTime.Domain;

namespace AdvantShop.Module.BuyInTime
{
    public partial class Admin_BuyInTimeModule : UserControl
    {
        private const string _moduleName = "BuyInTime";

        private void MsgErr(string msg)
        {
            lblMessage.Visible = true;
            lblMessage.Text = msg;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ckeActionTitle.Text = ModuleSettingsProvider.GetSettingValue<string>("BuyInTimeActionTitle", _moduleName);
                txtLabelCode.Text = ModuleSettingsProvider.GetSettingValue<string>("BuyInTimeLabel", _moduleName);
            }

            rprProducts.DataSource = BuyInTimeService.GetProductsTable();
            rprProducts.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ModuleSettingsProvider.SetSettingValue("BuyInTimeActionTitle", ckeActionTitle.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("BuyInTimeLabel", txtLabelCode.Text, _moduleName);

            CacheManager.RemoveByPattern(BuyInTimeService.CacheKey);
        }

        protected void rprProducts_ItemCommand(object source, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                BuyInTimeService.Delete(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }
    }
}