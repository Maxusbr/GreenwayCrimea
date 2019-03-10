//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Localization;
using AdvantShop.Security;
using AdvantShop.Saas;
using AdvantShop.Trial;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Core.Services.IPTelephony;

namespace Admin
{
    public partial class MasterPageAdmin : MasterPage
    {
        protected int newReviewsCount = 0;
        protected int newAdminMessage = 0;
        //protected string Submenu;
		protected Customer Customers;

        public bool AchievementsHelpVisible
        {
            get
            {
                return AchievementsHelp.Visible;
            }
            set
            {
                AchievementsHelp.isVisible = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SaasFeaturesExclusion.IsAvailblePage(Request.Url.AbsoluteUri))
            {
                Response.Redirect("/admin/notintariff.aspx");
                return;
            }

			Customers = CustomerService.GetCustomer(CustomerContext.CurrentCustomer.Id);

            form1.Action = UrlService.GetAdminBaseUrl(); //GetUrl(Request.Url.PathAndQuery.Replace(Request.ApplicationPath, "").TrimStart('/'));

            lBase.Text = string.Format("<base href='{0}'/>", UrlService.GetUrl("admin/"));

            
            MenuAdmin.CurrentCustomer = CustomerContext.CurrentCustomer;

            //newAdminMessage = AdminMessagesService.GetNotViewedAdminMessagesCount();
            newReviewsCount = StatisticService.GetLastReviewsCount();

            //adminMessages.CssClass = "top-part-right icon-mail-a " + (newAdminMessage > 0 ? "icon-selected" : "");
            //adminMessages.Text = newAdminMessage > 0 ? newAdminMessage.ToString() : "";
            adminMessages.Visible = false;// AdvantshopConfigService.GetLocalization() == "ru-RU";

            adminReviews.CssClass = "top-part-right icon-bubble " + (newReviewsCount > 0 ? "icon-selected" : "");
            adminReviews.Text = newReviewsCount > 0 ? newReviewsCount.ToString() : "";

            var _customer = CustomerContext.CurrentCustomer;
            if (_customer.CustomerRole == Role.Moderator)
            {
                var actions = RoleActionService.GetCustomerRoleActionsByCustomerId(_customer.Id);
                bool visible = actions.Any(item => item.Role == RoleAction.Orders);

                //StoreLanguage.Visible = visible;
                LastAdminMessages.Visible = visible;
                adminMessages.Visible = visible;

                adminReviews.Visible = actions.Any(item => item.Role == RoleAction.Catalog);
            }

            hlEditBindings.Visible = (SaasDataService.IsSaasEnabled || TrialService.IsTrialEnabled) &&
                                     SaasDataService.IsTechDomain && (_customer.CustomerRole == Role.Administrator || _customer.IsVirtual);
            hlEditBindings.NavigateUrl =
                !string.IsNullOrEmpty(CustomerContext.CurrentCustomer.EMail)
                    ? Resources.Resource.Admin_MasterPageAdmin_MyAccountHref +
                      "/login.aspx?email=" + CustomerContext.CurrentCustomer.EMail +
                      "&hash=" + SecurityHelper.EncodeWithHmac(CustomerContext.CurrentCustomer.EMail, CustomerContext.CurrentCustomer.Password ?? "") +
                      "&shopid=" + SettingsLic.LicKey +
                      "&mode=" + (SaasDataService.IsSaasEnabled ? "bindings" : "activate")
                    : "";

            storeClosed.Visible = SettingsMain.IsStoreClosed;

            if (IPTelephonyOperator.Current.Type != EOperatorType.None)
            {
                bodyId.Attributes.Add("class", "webnotifications");
            }
        }

        public void Page_PreRender(object sender, EventArgs e)
        {
            var cultureName = Culture.Language != Culture.SupportLanguage.Other ? SettingsMain.Language : "ru-RU";

            JsCssTool.ReCreateIfNotExist();

            headStyle.Text = JsCssTool.MiniCss(new List<string>{
                                    "~/admin/css/validator.css"
                                   ,"~/admin/css/normalize.css"
                                   ,"~/admin/css/advcss/modal.css"
                                   ,"~/admin/css/jq/jquery.autocomplete.css"
                                   ,"~/admin/css/modalAdmin.css"

                                   ,"~/admin/js/plugins/intlTelInput/css/intlTelInput.css"
                                   ,"~/admin/js/plugins/progress/css/progress.css"
                                   ,"~/admin/js/plugins/jpicker/css/jpicker.css"
                                   ,"~/admin/js/plugins/tabs/css/tabs.css"
                                   ,"~/admin/js/plugins/bubble/css/bubble.css"
                                   ,"~/admin/css/jquery.tooltip.css"
                                   ,"~/admin/css/AdminStyle.css"
                                   ,"~/admin/css/advcss/notify.css"
                                   ,"~/admin/css/catalogDataTreeStyles.css"
                                   ,"~/admin/css/exportFeedStyles.css"
                                   ,"~/admin/js/plugins/tooltip/css/tooltip.css"
                                   ,"~/admin/js/plugins/placeholder/css/placeholder.css"
                                   ,"~/admin/js/plugins/radiolist/css/radiolist.css"
                                   ,"~/admin/js/plugins/chart/css/chart.css"
                                   ,"~/admin/js/plugins/noticeStatistic/css/noticeStatistic.css"
                                   ,"~/admin/js/plugins/help/css/help.css"
                                   ,"~/admin/js/plugins/datepicker/css/datepicker.css"
                                   ,"~/admin/js/plugins/transformer/css/transformer.css"

                                   ,"~/admin/js/jspage/adminmessages/css/styles.css"
                                   ,"~/admin/css/new_admin/buttons.css"
                                   ,"~/admin/css/new_admin/dropdown-menu.css"
                                   ,"~/admin/css/new_admin/icons.css"
                                   ,"~/admin/css/new_admin/admin.css"
                                   ,"~/admin/css/new_admin/pagenumber.css"
                                   ,"~/admin/css/new_admin/achievements.css"
                                   ,"~/admin/css/new_admin/achievementsHelp.css"
                                   ,"~/admin/css/nv.d3.css"
                                   ,"~/admin/css/new_admin/modules.css"},
                                   "admincss.css");

            headScript.Text = JsCssTool.MiniJs(new List<string>{
                                    "~/admin/js/jq/jquery-1.7.1.min.js"
                                    ,"~/admin/js/modernizr.custom.js"
                                    ,"~/admin/js/localization/" + cultureName + "/lang.js"
                                    ,"~/admin/js/ejs_fulljslint.js"
                                    ,"~/admin/js/ejs.js"
                                    ,"~/admin/js/plugins/intlTelInput/js/intlTelInput.js"
                                    ,"~/admin/js/plugins/intlTelInput/js/1.js"
                                    //,"~/admin/js/plugins/d3/d3.js"
                                    //,"~/admin/js/plugins/d3/nv.d3.js"
                                    },
                                    "adminlib.js");

            bottomScript.Text = JsCssTool.MiniJs(new List<string>{
                                      "~/admin/js/webnotification/webnotification.js"
                                      ,"~/admin/js/jq/jquery.validate.js"
                                      ,"~/admin/js/validateInit.js"
                                      ,"~/admin/js/string.format-1.0.js"
                                      ,"~/admin/js/jq/jquery.autocomplete.js"
                                      ,"~/admin/js/jq/jquery.metadata.js"
                                      ,"~/admin/js/advjs/advNotify.js"
                                      ,"~/admin/js/advjs/advModal.js"
                                      ,"~/admin/js/advjs/advTabs.js"
                                      ,"~/admin/js/advjs/advUtils.js"
                                      ,"~/admin/js/advantshop.js"
                                      ,"~/admin/js/services/Utilities.js"
                                      ,"~/admin/js/services/scriptsManager.js"
                                      ,"~/admin/js/services/jsuri-1.1.1.js"
                                      ,"~/admin/js/plugins/progress/progress.js"
                                      ,"~/admin/js/plugins/jpicker/jpicker.js"
                                      ,"~/admin/js/plugins/tabs/tabs.js"
                                      ,"~/admin/js/plugins/bubble/bubble.js"
                                      ,"~/admin/js/customValidate.js"
                                      ,"~/admin/js/smallThings.js"
                                      ,"~/admin/js/smallThings.js"
                                      ,"~/admin/js/modalOwnerContacts.js"

                                      ,"~/admin/js/jspage/adminmessages/adminmessages.js"
                                      ,"~/admin/js/jspage/achievements/achievements.js"
                                      ,"~/admin/js/jspage/vieworder.js"
                                      ,"~/admin/js/jspage/viewcustomer.js"
                                      ,"~/admin/js/jspage/product.js"
                                      ,"~/admin/js/jspage/modulesmanager.js"
                                      ,"~/admin/js/jspage/default.js"
                                      ,"~/admin/js/jspage/theme.js"
                                      ,"~/admin/js/jspage/settingspage.js"
                                      ,"~/admin/js/jspage/styleeditor.js"
                                      ,"~/admin/js/jspage/lead.js"
                                      ,"~/admin/js/jspage/catalog.js"
                                      ,"~/admin/js/jspage/statistics.js"

                                      ,"~/admin/js/jq/jquery.raty.js"

                                      ,"~/admin/js/jquery.cookie.min.js"
                                      ,"~/admin/js/jquery.qtip.min.js"
                                      ,"~/admin/js/jquery.tooltip.min.js"
                                      ,"~/admin/js/slimbox2.js"
                                      ,"~/admin/js/jquery.history.js"
                                      ,"~/admin/js/jquerytimer.js"
                                      ,"~/admin/js/admin.js"
                                      ,"~/admin/js/grid.js"
                                      ,"~/admin/js/plugins/tooltip/tooltip.js"
                                      ,"~/admin/js/plugins/placeholder/placeholder.js"
                                      ,"~/admin/js/plugins/chart/jquery.flot.js"
                                      ,"~/admin/js/plugins/chart/jquery.flot.pie.js"
                                      ,"~/admin/js/plugins/chart/jquery.flot.time.js"
                                      ,"~/admin/js/plugins/chart/jquery.flot.categories.js"
                                      ,"~/admin/js/plugins/chart/chart.js"
                                      ,"~/admin/js/plugins/radiolist/radiolist.js"
                                      ,"~/admin/js/plugins/help/help.js"
                                      ,"~/admin/js/plugins/transformer/transformer.js"
                                      

                                      ,"~/admin/js/plugins/datepicker/bootstrap-datepicker.js"
                                      ,"~/admin/js/plugins/datepicker/locales/bootstrap-datepicker." + cultureName.Split('-')[0] + ".js"

                                      // TODO: dublicate client side js (different versions)
                                      ,"~/admin/js/plugins/jqfileupload/vendor/jquery.ui.widget.js"
                                      ,"~/admin/js/plugins/jqfileupload/jquery.iframe-transport.js"
                                      ,"~/admin/js/plugins/jqfileupload/jquery.fileupload.js"

                                      ,"~/admin/js/plugins/noticeStatistic/noticeStatistic.js"
                                      ,"~/admin/js/masterpage/adminsearch.js"
                                      ,"~/admin/js/masterpage/saasIndicator.js"
                                      ,"~/admin/js/masterpage/ordersCount.js"
                                      ,"~/admin/js/masterpage/share.js"
                                      ,"~/admin/js/masterpage/ordersCount.js"
                                      ,"~/admin/js/masterpage/achievementsHelp.js"
                                      ,"~/admin/js/masterpage/achievementsPopup.js"
                                      ,"~/admin/js/masterpage/showcase.js"},
                                      "adminall.js");
        }

        //protected void Page_Complete(object sender, EventArgs e)
        //{
        //    Submenu = "New" + MenuAdmin.RenderSubMenu();
        //}

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            AuthorizeService.SignOut();
            //CustomerContext.IsDebug = false;
            Response.Redirect("~/");
        }

        protected void lbNewAdmin_Click(object sender, EventArgs e)
        {
            CommonHelper.DeleteCookie("oldadmin");
            Response.Redirect("~/adminv2/");
        }
    }
}