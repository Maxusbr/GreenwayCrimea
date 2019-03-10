using System;
using System.Collections.Generic;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.UrlRewriter;

namespace Admin
{
    public partial class m_MasterPage : System.Web.UI.MasterPage
    {
        public void Page_PreRender(object sender, EventArgs e)
        {
            lBase.Text = string.Format("<base href='{0}'/>", UrlService.GetUrl("admin/"));

            JsCssTool.ReCreateIfNotExist();

            headStyle.Text = JsCssTool.MiniCss(new List<string>
                {
                    "~/admin/css/new_admin/buttons.css",
                    "~/admin/css/jquery.tooltip.css",
                    "~/admin/css/AdminStyle.css",
                    "~/admin/css/catalogDataTreeStyles.css",
                    "~/admin/css/exportFeedStyles.css",
                    "~/admin/css/jqueryslidemenu.css",
                    "~/admin/js/plugins/datepicker/css/datepicker.css",
                    "~/admin/css/jq/jquery.autocomplete.css",
                    "~/admin/css/advcss/modal.css"
                },
                "madmincss.css");

            headScript.Text = JsCssTool.MiniJs(new List<string>
            {
                "~/admin/js/jq/jquery-1.7.1.min.js",
                "~/admin/js/jq/jquery.autocomplete.js",
                "~/admin/js/jq/jquery.metadata.js",
                "~/admin/js/advjs/advModal.js",
                "~/admin/js/advjs/advTabs.js",
                "~/admin/js/advjs/advUtils.js",
                "~/admin/js/jquery.cookie.min.js",
                "~/admin/js/jquery.qtip.min.js",
                "~/admin/js/jquery.tooltip.min.js",
                "~/admin/js/slimbox2.js",
                "~/admin/js/jquery.history.js",
                "~/admin/js/jquerytimer.js",
                "~/admin/js/admin.js",
                "~/admin/js/grid.js",
                "~/admin/js/plugins/datepicker/bootstrap-datepicker.js",
                "~/admin/js/plugins/datepicker/locales/bootstrap-datepicker." + SettingsMain.Language.Split('-')[0] + ".js"
            },
            "madmin.js");
        }
    }
}
