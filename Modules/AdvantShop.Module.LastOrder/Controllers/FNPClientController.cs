using AdvantShop.Catalog;
using AdvantShop.Helpers;
using AdvantShop.Module.LastOrder.Models;
using AdvantShop.Module.LastOrder.Service;
using AdvantShop.Module.LastOrder.ViewModel;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdvantShop.Module.LastOrder.Controllers
{
    [Module(Type = "LastOrder")]
    public class FNPClientController : ModuleController
    {
        string ModuleID = LastOrder.ModuleStringId;
        public ActionResult Index()
        {
            return PartialView("~/modules/" + ModuleID + "/Views/Client/Index.cshtml");
        }

        public string LinkScriptStyle()
        {
            var scripts = "<script src=\"modules/" + ModuleID + "/content/scripts/client-script.js?"+ModuleSettings.Version+"\"></script>";
            var styles = "<link rel=\"stylesheet\" href=\"modules/" + ModuleID + "/content/styles/client-style.css?"+ModuleSettings.Version+"\">";
            return scripts + styles;
        }

        public ActionResult Notify(Product product, Offer offer)
        {
            /*if (!ModuleSettings.AlwaysShow && (offer == null || offer.Amount < 1))
            {
                return new EmptyResult();
            }*/

            ActionResult result = Content("");

            var notify = FNPService.GetFakeNotification(product.ProductId);

            if (notify != null)
            {
                var model = new FNPViewModel(notify);
                model.City = model.City.Trim();
                model.Name = model.Name.Trim();
                return PartialView("~/modules/" + ModuleID + "/Views/Client/Index.cshtml", model);
            }

            var fnp_view = CommonHelper.GetCookie("fnp_view");

            if (fnp_view == null)
            {
                CommonHelper.SetCookie("fnp_view", "1", TimeSpan.Parse("1"),true);
            }
            else
            {
                var countView = Convert.ToInt32(fnp_view.Value);
                if ((countView+1) >= ModuleSettings.ShowPeriod)
                {
                    FNPModel newNotify;

                    var showUserCity = false;

                    var userCityView = CommonHelper.GetCookie("userCityView");

                    if (userCityView != null)
                    {
                        var userCityCountView = Convert.ToInt32(userCityView.Value);

                        var showUserCityCounter = ModuleSettings.ShowUserCity;

                        if (showUserCityCounter > 0 && (userCityCountView+1) >= showUserCityCounter)
                        {
                            showUserCity = true;
                            userCityCountView = -1;
                        }

                        userCityView.Value = (++userCityCountView).ToString();
                        CommonHelper.SetCookie("userCityView", userCityView.Value, TimeSpan.Parse("1"), true);
                    }
                    else
                    {
                        CommonHelper.SetCookie("userCityView", "1", TimeSpan.Parse("1"), true);
                    }

                    newNotify = FNPService.GenerateNotification(product.ProductId, false, showUserCity);
                    var notifyVM = new FNPViewModel(newNotify);
                    result = PartialView("~/modules/" + ModuleID + "/Views/Client/Index.cshtml", notifyVM);
                    countView = -1;
                }

                fnp_view.Value = (++countView).ToString();
                CommonHelper.SetCookie("fnp_view", fnp_view.Value, TimeSpan.Parse("1"), true);
            }
            return result;
        }

        public ActionResult Location()
        {
            if (string.IsNullOrEmpty(ModuleSettings.Location))
            {
                ModuleSettings.Location = ".details-payment";
            }
            return Content(ModuleSettings.Location);
        }

    }
}
