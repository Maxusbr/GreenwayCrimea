using System.Collections.Concurrent;
using System.ComponentModel;

namespace AdvantShop.Web.Infrastructure.Controllers
{
    public class NgControllers
    {
        public enum NgControllersTypes
        {
            // client
            [Description("app")] AppCtrl,
            [Description("catalog")] CatalogCtrl,
            [Description("checkout")] CheckOutCtrl,
            [Description("checkoutSuccess")] CheckOutSuccessCtrl,
            [Description("product")] ProductCtrl,
            [Description("feedback")] FeedbackCtrl,
            [Description("home")] HomeCtrl,
            [Description("myaccount")] MyAccountCtrl,
            [Description("preorder")] PreorderCtrl,
            [Description("giftcertificate")] GiftCertificateCtrl,
            [Description("brand")] BrandCtrl,
            [Description("staticPage")] StaticPageCtrl,
            [Description("brandsList")] BrandsListCtrl,
            [Description("wishlistPage")] WishlistPageCtrl,
            [Description("managers")] ManagersCtrl,
            [Description("checkout")] BillingCtrl,


            // admin panel
            [Description("settingsCrm")] SettingsCrmCtrl,
            [Description("bonuses")] BonusesCtrl,
            [Description("calls")] CallsCtrl,
            [Description("category")] CategoryCtrl,
            [Description("lead")] LeadCtrl,
            [Description("leads")] LeadsCtrl,
            [Description("modules")] ModulesCtrl,
            [Description("module")] ModuleCtrl,
            [Description("mainpageproducts")] MainPageProductsCtrl,
            [Description("mailSettings")] MailSettingsCtrl,
            [Description("orders")] OrdersCtrl,
            [Description("order")] OrderCtrl,
            [Description("orderstatuses")] OrderStatusesCtrl,
            [Description("ordersources")] OrderSourcesCtrl,
            [Description("customers")] CustomersCtrl,
            [Description("customer")] CustomerCtrl,
            [Description("customergroups")] CustomerGroupsCtrl,
            [Description("customerSegments")] CustomerSegmentsCtrl,
            [Description("customerSegment")] CustomerSegmentCtrl,
            [Description("tasks")] TasksCtrl,
            [Description("taskgroups")] TaskGroupsCtrl,
            [Description("properties")] PropertiesCtrl,
            [Description("propertyvalues")] PropertyValuesCtrl,
            [Description("productlists")] ProductListsCtrl,
            [Description("priceregulation")] PriceRegulationCtrl,
            [Description("design")] DesignCtrl,
            [Description("csseditor")] CssEditorCtrl,
            [Description("sizes")] SizesCtrl,
            [Description("colors")] ColorsCtrl,
            [Description("tags")] TagsCtrl,
            [Description("reviews")] ReviewsCtrl,
            [Description("discountsPriceRange")] DiscountsPriceRangeCtrl,
            [Description("coupons")] CouponsCtrl,
            [Description("tariffs")] TariffsCtrl,
            [Description("menus")] MenusCtrl,
            [Description("news")] NewsCtrl,
            [Description("newsItem")] NewsItemCtrl,
            [Description("newsCategory")] NewsCategoryCtrl,
            [Description("carousel")] CarouselCtrl,
            [Description("files")] FilesCtrl,
            [Description("staticPages")] StaticPagesCtrl,
            [Description("staticBlock")] StaticBlockCtrl,
            [Description("certificates")] CertificatesCtrl,
            [Description("subscription")] SubscriptionCtrl,
            [Description("shippingMethod")] ShippingMethodCtrl,
            [Description("paymentMethod")] PaymentMethodCtrl,
            [Description("voting")] VotingCtrl,
            [Description("grades")] GradesCtrl,
            [Description("cards")] CardsCtrl,
            [Description("smstemplates")] SmsTemplatesCtrl,
            [Description("rules")] RulesCtrl,
            [Description("landingPages")] LandingPagesCtrl,


            // landing
            [Description("landings")] LandingsAdminCtrl,

            [Description("exportFeeds")] ExportFeedsCtrl,
            [Description("exportCategories")] ExportCategoriesCtrl,
            [Description("analytics")] AnalyticsCtrl,
            [Description("analyticsReport")] AnalyticsReportCtrl,
            [Description("analyticsFilter")] AnalyticsFilterCtrl,
            [Description("import")] ImportCtrl,

            //settings                                   
            [Description("settings")] SettingsCtrl,
            [Description("settingsCheckout")] SettingsCheckoutCtrl,
            [Description("settingsSeo")] SettingsSeoCtrl,
            [Description("settingsNews")] SettingsNewsCtrl,
            [Description("settingsSystem")] SettingsSystemCtrl,
            [Description("settingsSocial")] SettingsSocialCtrl,
            [Description("settingsCatalog")] SettingsCatalogCtrl,
            [Description("settingsCustomers")] SettingsCustomersCtrl,
            [Description("settingsUsers")] SettingsUsersCtrl,
            [Description("settingsSearch")] SettingsSearchCtrl,
            [Description("settingsTasks")] SettingsTasksCtrl,
            [Description("settingsTelephony")] SettingsTelephonyCtrl,
            [Description("settingsBonus")] SettingsBonusCtrl
        }
        
        private static ConcurrentDictionary<string, string> _controllerlDescriptions = null;

        public static string GetNgControllerInitString(NgControllersTypes controllerType)
        {
            if (_controllerlDescriptions == null)
                _controllerlDescriptions = new ConcurrentDictionary<string, string>();

            string controller = controllerType.ToString();
            string description = null;

            if (!_controllerlDescriptions.TryGetValue(controller, out description))
            {
                var type = typeof(NgControllersTypes);
                var memInfo = type.GetMember(controllerType.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                description = ((DescriptionAttribute) attributes[0]).Description;

                _controllerlDescriptions.TryAdd(controller, description);            
            }
            
            return string.Format("data-ng-controller=\"{0} as {1}\"", controller, description);
        }
    }
}
