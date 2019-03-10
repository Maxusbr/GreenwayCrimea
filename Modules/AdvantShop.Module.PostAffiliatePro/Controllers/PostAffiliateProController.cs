using System.Web.Mvc;
using AdvantShop.Core.Modules;
using AdvantShop.Orders;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Module.PostAffiliatePro.Controllers
{
    public class PostAffiliateProController : ModuleController
    {
        private const string ModuleId = "PostAffiliatePro";

        public ActionResult RenderBeforeBodyEnd()
        {
            if (string.IsNullOrEmpty(ModuleSettingsProvider.GetSettingValue<string>("PostAffiliateProLogin", ModuleId)))
                return Content("");

            return
                Content(
                    string.Format(
                        "<script type=\"text/javascript\">document.write(unescape(\"%3Cscript id='pap_x2s6df8d' src='\" + ((\"https:\" == document.location.protocol) ? \"https://\" : \"http://\") + \"{0}/scripts/trackjs.js' type='text/javascript'%3E%3C/script%3E\"));</script>" +
                        "<script type=\"text/javascript\">PostAffTracker.setAccountId('{1}');try {{PostAffTracker.track();}} catch (err) {{ console.log(err); }}</script>",
                        ModuleSettingsProvider.GetSettingValue<string>("PostAffiliateProLogin", ModuleId),
                        ModuleSettingsProvider.GetSettingValue<string>("PostAffiliateProProfile", ModuleId)));
        }

        public ActionResult OrderFinalStep(IOrder order)
        {
            if (string.IsNullOrEmpty(ModuleSettingsProvider.GetSettingValue<string>("PostAffiliateProLogin", ModuleId)))
                return new EmptyResult();

            //var productsHtml = order.OrderItems.Aggregate(string.Empty, (current, item) => current + string.Format(registerProductHtml, (item.Price) * item.Amount, order.OrderID, item.ProductID));
            var productsArtNo = string.Empty;
            for (int i = 0; i < order.OrderItems.Count; ++i)
            {
                productsArtNo += order.OrderItems[i].ArtNo + (i != order.OrderItems.Count - 1 ? "," : string.Empty);
            }
            var orderCustomer = order.GetOrderCustomer();

            const string mainHtmlBlock =
                "<script type=\"text/javascript\">document.write(unescape(\"%3Cscript id='pap_x2s6df8d' src='\" + ((\"https:\" == document.location.protocol) ? \"https://\" : \"http://\") + \"{0}/scripts/trackjs.js' type='text/javascript'%3E%3C/script%3E\"));</script>" +
                "<script type=\"text/javascript\">PostAffTracker.setAccountId('{1}');{2}PostAffTracker.register();</script>";

            const string registerProductHtml = "var sale = PostAffTracker.createSale();" +
                                                "sale.setTotalCost('{0}');" +
                                                "sale.setOrderID('{1}');" +
                                                "sale.setProductID('{2}');" +
                                                "sale.setData1('{3}');" +
                                                "sale.setData2('{4}');";

            return Content(
                string.Format(mainHtmlBlock,
                    ModuleSettingsProvider.GetSettingValue<string>("PostAffiliateProLogin", ModuleId),
                    ModuleSettingsProvider.GetSettingValue<string>("PostAffiliateProProfile", ModuleId),
                    string.Format(registerProductHtml,
                        (order.Sum - order.ShippingCost).ToString("F2").Replace(",", "."),
                        order.OrderID,
                        productsArtNo,
                        orderCustomer != null ? orderCustomer.Email : string.Empty,
                        order.Coupon != null ? order.Coupon.Code : string.Empty
                        )));
        }
    }
}
