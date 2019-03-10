using System;
using System.Web.UI;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.MegaPlat
{
    public partial class Admin_MegaPlatModule : UserControl
    {
        private const string _moduleName = "MegaPlat";

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lblApiKey.Text = ModuleSettingsProvider.GetSettingValue<string>("MegaPlatApiKey", _moduleName);

            lblCheckApiKey.Text = "~/Modules/Megaplat/CheckApiKey.ashx";
            linkCheckApiKey.NavigateUrl = "~/Modules/Megaplat/CheckApiKey.ashx?apikey=" + lblApiKey.Text;
            
            lblDownloadCatalog.Text = "~/Modules/Megaplat/DownloadCatalog.ashx";
            linkDownloadCatalog.NavigateUrl = "~/Modules/Megaplat/DownloadCatalog.ashx?apikey=" + lblApiKey.Text;

            lblDownloadCategories.Text = "~/Modules/Megaplat/DownloadCategories.ashx";
            linkDownloadCategories.NavigateUrl = "~/Modules/Megaplat/DownloadCategories.ashx?apikey=" + lblApiKey.Text;

            lblUploadCatalog.Text = "~/Modules/Megaplat/UploadCatalog.ashx";
            linkUploadCatalog.NavigateUrl = "~/Modules/Megaplat/UploadCatalog.ashx?apikey=" + lblApiKey.Text;

            lblDownloadOrders.Text = "~/Modules/Megaplat/DownloadOrders.ashx";
            linkDownloadOrders.NavigateUrl = "~/Modules/Megaplat/DownloadOrders.ashx?apikey=" + lblApiKey.Text;
        }
    }
}