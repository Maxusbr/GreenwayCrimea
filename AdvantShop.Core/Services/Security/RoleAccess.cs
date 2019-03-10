//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AdvantShop.Customers;

namespace AdvantShop.Security
{
    public class RoleAccess
    {
        private static readonly Dictionary<string, RoleAction> dictionary = new Dictionary<string, RoleAction>
        {
            // Catalog
            {"catalog.aspx",                RoleAction.Catalog},
            {"product.aspx",                RoleAction.Catalog},
            {"m_category.aspx",             RoleAction.Catalog},
            {"m_categorysortorder.aspx",    RoleAction.Catalog},
            {"m_productvideos.aspx",        RoleAction.Catalog},
            {"searchstatistic.aspx",        RoleAction.Catalog},
            {"uploadphoto.ashx",            RoleAction.Catalog},
            {"uploadphotos360.ashx",        RoleAction.Catalog},
            {"uploadcolorpicture.ashx",     RoleAction.Catalog},
            {"deletecolorpicture.ashx",     RoleAction.Catalog},
            {"filemanager.ashx",            RoleAction.Catalog},
            {"main.ashx",                   RoleAction.Catalog},
            {"copyproduct.ashx",            RoleAction.Catalog},
            {"addproduct.ashx",             RoleAction.Catalog},
            {"loadproperties.ashx",         RoleAction.Catalog},
            {"searchphotos.ashx",           RoleAction.Catalog},

            {"exportfeed.aspx",             RoleAction.Catalog},
            {"importcsv.aspx",              RoleAction.Catalog},
            {"commonstatisticdata.ashx",    RoleAction.Catalog},
            {"uploadzipphoto.ashx",         RoleAction.Catalog},
            {"exportfeeddet.aspx",          RoleAction.Catalog},
            {"exportfeedprogress.aspx",     RoleAction.Catalog},

            {"properties.aspx",     RoleAction.Catalog},
            {"m_property.aspx",     RoleAction.Catalog},
            {"propertyvalues.aspx", RoleAction.Catalog},
            {"colors.aspx",         RoleAction.Catalog},
            {"sizes.aspx",          RoleAction.Catalog},
            {"tags.aspx",           RoleAction.Catalog},
            {"tag.aspx",            RoleAction.Catalog},

            {"m_propertygroup.aspx",            RoleAction.Catalog},
            {"m_propertygroupssortorder.aspx",  RoleAction.Catalog},

            {"productsonmain.aspx?type=best", RoleAction.Catalog},
            {"productsonmain.aspx?type=new",  RoleAction.Catalog},
            {"productsonmain.aspx?type=sale", RoleAction.Catalog},
            {"productlists.aspx", RoleAction.Catalog},
            {"productlistmapping.aspx", RoleAction.Catalog},

            {"reviews.aspx", RoleAction.Catalog},

            {"priceregulation.aspx", RoleAction.Catalog},
            {"discountsbydatetime.aspx", RoleAction.Catalog},

            {"brands.aspx",  RoleAction.Catalog},
            {"m_brand.aspx", RoleAction.Catalog},

            // Order
            {"ordersearch.aspx",        RoleAction.Orders},
            {"editorder.aspx",          RoleAction.Orders},
            {"vieworder.aspx",          RoleAction.Orders},
            {"orderbyrequest.aspx",     RoleAction.Orders},
            {"editorderbyrequest.aspx", RoleAction.Orders},
            {"productssearch.ashx",     RoleAction.Orders},
            {"sendmailorderstatus.ashx",RoleAction.Orders},
            {"getnoticestatistic.ashx", RoleAction.Orders},
            {"setorderstatus.ashx",     RoleAction.Orders},
            {"setorderpaid.ashx",       RoleAction.Orders},
            {"updateorderfield.ashx",   RoleAction.Orders},
			{"exportorderexcel.ashx",   RoleAction.Orders},
			{"setmanagerorder.ashx",    RoleAction.Orders},
            {"sendbillinglink.ashx",    RoleAction.Orders},
            {"export1c.aspx",           RoleAction.Orders},
            {"exportordersexcel.aspx",  RoleAction.Orders},
            {"statisticsordersexportcsv.aspx", RoleAction.Orders},
            {"exportordersstatisticdata.ashx", RoleAction.Orders},
            {"changeusein1c.ashx",      RoleAction.Orders},
            {"setmanagerconfirm.ashx",      RoleAction.Orders},

			{"yandexdeliverysendorder.ashx", RoleAction.Orders},
            {"sdekcallcourier.ashx",       RoleAction.Orders},
            {"sdekcallcustomer.ashx",      RoleAction.Orders},
            {"sdekdeleteorder.ashx",       RoleAction.Orders},
            {"sdekorderprintform.ashx",    RoleAction.Orders},
            {"sdekreportorderinfo.ashx",   RoleAction.Orders},
            {"sdekreportorderstatus.ashx", RoleAction.Orders},
            {"sdeksendorder.ashx",         RoleAction.Orders},
            {"checkoutsendorder.ashx", RoleAction.Orders},

            {"orderstatuses.aspx",      RoleAction.Orders},
            
            // Customer
            {"customersearch.aspx",     RoleAction.Customers},
            {"createcustomer.aspx",     RoleAction.Customers},
            {"customersgroups.aspx",    RoleAction.Customers},
            {"viewcustomer.aspx",       RoleAction.Customers},
            {"editcustomer.aspx",       RoleAction.Customers},

            {"subscription.aspx",                  RoleAction.Customers},
            {"subscription_unreg.aspx",            RoleAction.Customers}, 
            {"subscription_deactivatereason.aspx", RoleAction.Customers},

            // CRM
            {"managers.aspx",       RoleAction.Crm},
            {"m_managers.aspx",     RoleAction.Crm},
            {"departments.aspx",    RoleAction.Crm},
            {"managerstasks.aspx",  RoleAction.Crm},
            {"managertask.aspx",    RoleAction.Crm},
            {"leads.aspx",          RoleAction.Crm},
            {"editlead.aspx",       RoleAction.Crm},
            {"calls.aspx",          RoleAction.Crm},
            {"getrecordlink.ashx",  RoleAction.Crm},
            {"findrelatedcustomers.ashx",  RoleAction.Crm},
            
            
            // CMS
            {"menu.aspx",   RoleAction.Cms},
            {"m_menu.aspx", RoleAction.Cms},

            {"newsadmin.aspx",    RoleAction.Cms},
            {"newscategory.aspx", RoleAction.Cms},
            {"m_news.aspx",       RoleAction.Cms},
            {"editnews.aspx",     RoleAction.Cms},

            {"carousel.aspx", RoleAction.Cms},

            {"staticpages.aspx", RoleAction.Cms},
            {"staticpage.aspx",  RoleAction.Cms},

            {"staticblocks.aspx", RoleAction.Cms},
            {"m_staticblock.aspx",  RoleAction.Cms},

            // Marketing
            {"discount_pricerange.aspx", RoleAction.Marketing},
            {"coupons.aspx",             RoleAction.Marketing },
            {"m_coupon.aspx",            RoleAction.Marketing},

            {"certificates.aspx",  RoleAction.Marketing},
            {"certificatesoptions.aspx",  RoleAction.Marketing},
            {"m_certificate.aspx", RoleAction.Marketing},

            {"voting.aspx",         RoleAction.Marketing},
            {"votinghistory.aspx",  RoleAction.Marketing},
            {"answers.aspx",        RoleAction.Marketing},
            
            {"sendmessage.aspx",       RoleAction.Marketing},

            {"sitemapgenerate.aspx",    RoleAction.Settings},
            {"sitemapgeneratexml.aspx", RoleAction.Settings},
            // Common
            {"commonsettings.aspx", RoleAction.Settings},

            {"module.aspx",         RoleAction.Modules},
            {"modulesmanager.aspx", RoleAction.Modules},
            {"installmodule.ashx",  RoleAction.Modules},
            {"setmoduleactive.ashx",RoleAction.Modules},

            {"designconstructor.aspx", RoleAction.Design},
            {"styleseditor.aspx", RoleAction.Design},
            {"templatesettings.aspx", RoleAction.Design},
            {"savetemplatesettings.ashx", RoleAction.Design},


            {"country.aspx", RoleAction.Settings},
            {"regions.aspx", RoleAction.Settings},
            {"cities.aspx",  RoleAction.Settings},

            {"currencies.aspx", RoleAction.Settings},

            {"paymentmethod.aspx", RoleAction.Settings},

            {"shippingmethod.aspx", RoleAction.Settings},

            {"taxes.aspx", RoleAction.Settings},
            {"tax.aspx",   RoleAction.Settings},

            {"mailformat.aspx",       RoleAction.Settings},
            {"mailformatdetail.aspx", RoleAction.Settings},
            
            {"logviewer.aspx",         RoleAction.Settings},
            {"logviewerdetailed.aspx", RoleAction.Settings},
            {"logerror404.aspx",       RoleAction.Settings},
            
            {"301redirects.aspx", RoleAction.Settings},
        };

        public static bool Check(Customer customer, string currentPage)
        {
            if (customer.CustomerRole != Role.Moderator || currentPage.Contains("default.aspx") ||
                currentPage.Contains("imagebrowser.aspx") || currentPage.Contains("linkbrowser.aspx"))
                return true;

            var page = currentPage.Contains("productsonmain.aspx")
                           ? currentPage.Split(new[] {'/'}).Last()
                           : currentPage.Split(new[] {'?'}).First().Split(new[] {'/'}).Last();

            if (dictionary.ContainsKey(page))
            {
                RoleAction key = dictionary[page];
                return
                    RoleActionService.GetCustomerRoleActionsByCustomerId(customer.Id)
                                     .Any(item => item.Role == key);
            }

            return false;
        }
    }
}