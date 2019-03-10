using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Module.OrderNow.ViewModel;
using AdvantShop.Module.OrderNow.Service;
using AdvantShop.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AdvantShop.Orders;
using AdvantShop.Design;


namespace AdvantShop.Module.OrderNow.Controllers
{
    [Module(Type = "OrderNow")]
    public class ONClientController : ModuleController
    {
        string ModuleID = OrderNow.ModuleStringId;
        public ActionResult Index(string url = "", string artno ="")
        {
            var model = new ClientViewmodel();
            try
            {
                model.ProductId = ProductService.GetProductByUrl(url.Substring(url.LastIndexOf('/') + 1)).ProductId;
            }
            catch
            {
                model.ProductId = -1;
            }

            model.Enabled = true;
            switch (ModuleSettings.ShowAt)
            {
                case "Над рейтингом": model.ShowAt = "rate"; break;
                case "Под ценой": model.ShowAt = "prce"; break;
                default: model.ShowAt = "ship"; break;
            }
            if (DesignService.TemplatePath.Contains("Chloe"))
            {
                switch (ModuleSettings.ShowAt)
                {
                    case "Над рейтингом": model.ShowAt = "clrt"; break;
                    case "Под ценой": model.ShowAt = "clpr"; break;
                    default: model.ShowAt = "clsh"; break;
                }

            }
            if (url.Contains("checkout") && ModuleSettings.ShowInOrderConfirm)
            {
                model.Enabled = CheckAmountInShoppingCart();
                model.ShowAt = "chck";
            }
            if (url.Contains("cart") && ModuleSettings.ShowInCart)
            {
                model.Enabled = CheckAmountInShoppingCart();
                model.ShowAt = "cart";
            }

            DateTime ShowStart = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + ModuleSettings.ShowStart + ":00");
            DateTime ShowFinish = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + ModuleSettings.ShowFinish + ":00");


            if (ShowStart >= DateTime.Now || DateTime.Now >= ShowFinish)
            {
                    model.Enabled = false;
                    return PartialView("~/modules/" + ModuleID + "/Views/Client/Index.cshtml", model);
            }
            if (ModuleSettings.IncludeWeekend)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    model.Enabled = false;
                    return PartialView("~/modules/" + ModuleID + "/Views/Client/Index.cshtml", model);
                }
            }
            if (ModuleSettings.LookForAvailability && artno != "")
            {
                if (OfferService.GetOffer(artno).Amount == 0)
                {
                    model.Enabled = false;
                    return PartialView("~/modules/" + ModuleID + "/Views/Client/Index.cshtml", model);
                }
            }

            

            return PartialView("~/modules/" + ModuleID + "/Views/Client/Index.cshtml", model);
        }

        private bool CheckAmountInShoppingCart()
        {
            var shpCrt = ShoppingCartService.GetShoppingCart(ShoppingCartType.ShoppingCart);

            foreach (var item in shpCrt)
            {
                if (item.Offer.Amount == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public string LinkScriptStyle()
        {
            var scripts = "<script src=\"modules/" + ModuleID + "/content/scripts/client-script.js\"></script>";
            var styles = "<link rel=\"stylesheet\" href=\"modules/" + ModuleID + "/content/styles/client-style.css\">";
            return scripts + styles;
        }

    }
}
