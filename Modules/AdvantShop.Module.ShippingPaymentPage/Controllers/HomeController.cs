using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Core.Modules;
using AdvantShop.Module.ShippingPaymentPage.Models;
using AdvantShop.Shipping;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;


namespace AdvantShop.Module.ShippingPaymentPage.Controllers
{
    [Module(Type = "ShippingPaymentPage")]
    public partial class HomeController : ModuleController
    {
        public ActionResult ShippingPayment()
        {
            var model = new ShippingPaymentModel()
            {
                Zone = Repository.IpZoneContext.CurrentZone,
                TextBlock = ModuleSettingsProvider.GetSettingValue<string>("ShippingTextBlock", ShippingPaymentPage.ModuleID),
                TextBlockBottom = ModuleSettingsProvider.GetSettingValue<string>("ShippingTextBlockBottom", ShippingPaymentPage.ModuleID)
            };

            var meta = new SEO.MetaInfo();

            meta.MetaDescription = ModuleSettingsProvider.GetSettingValue<string>("MetaDescription", ShippingPaymentPage.ModuleID);
            meta.MetaKeywords = ModuleSettingsProvider.GetSettingValue<string>("MetaKeywords", ShippingPaymentPage.ModuleID);
            meta.Title = ModuleSettingsProvider.GetSettingValue<string>("Title", ShippingPaymentPage.ModuleID); ;
            //SetMetaInformation(T("Module.ShippingPaymentPage.PageTitle"));
            SetMetaInformation(meta);

            return View("~/Modules/ShippingPaymentPage/Views/Home/ShippingPayment.cshtml", model);
        }

        public JsonResult GetListProduct()
        {
            var listProduct = new List<PreOrderItem>()
            {
                new PreOrderItem()
                {
                    Name = "TEST_PRODUCT",
                    Amount = 1,
                    Price = ModuleSettingsProvider.GetSettingValue<float>("DefaultPrice", ShippingPaymentPage.ModuleID),
                    ShippingPrice = ModuleSettingsProvider.GetSettingValue<float>("DefaultShippingPrice", ShippingPaymentPage.ModuleID),

                    Weight = ModuleSettingsProvider.GetSettingValue<float>("DefaultWeight", ShippingPaymentPage.ModuleID),
                    Width = ModuleSettingsProvider.GetSettingValue<float>("DefaultWidth", ShippingPaymentPage.ModuleID),
                    Height = ModuleSettingsProvider.GetSettingValue<float>("DefaultHeight", ShippingPaymentPage.ModuleID),
                    Length = ModuleSettingsProvider.GetSettingValue<float>("DefaultLength", ShippingPaymentPage.ModuleID),
                }
            };

            return Json(listProduct, JsonRequestBehavior.AllowGet);
        }
    }
}