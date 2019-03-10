using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Handlers.Install;
using AdvantShop.Helpers;
using AdvantShop.Models.Install;
using AdvantShop.ViewModel.Install;

namespace AdvantShop.Controllers
{
    public partial class InstallController : BaseClientController
    {
        public ActionResult Menu()
        {
            var model = new InstallHandler().Execute();
            return PartialView(model);
        }

        public ActionResult Index(InstallTrialSelectModel model)
        {
            if (System.IO.File.Exists(SettingsGeneral.InstallFilePath))
                return new RedirectToRouteResult("Home", null);

            var viewModel = new InstallTrialSelectHandler().Execute();
            viewModel.IsExpressInstall = model.IsExpressInstall ?? true;

            return PartialView(viewModel);
        }

        [HttpPost]
        public ActionResult IndexPost(InstallTrialSelectModel model)
        {
            if (model.IsExpressInstall == null)
                return RedirectToAction("Index");

            if (model.IsExpressInstall == true)
            {
                FileHelpers.CreateFile(SettingsGeneral.InstallFilePath);
                return new RedirectToRouteResult("Home", null);
            }

            var step = (InstallStep)((int)InstallStep.TrialSelect + 1);
            return new RedirectResult(UrlService.GetUrl() + "install/" + step.ToString().ToLower());
        }


        public ActionResult ShopInfo()
        {
            if (System.IO.File.Exists(SettingsGeneral.InstallFilePath))
                return new RedirectToRouteResult("Home", null);

            var model = new InstallShopInfoHandler().Get();

            model.BackUrl = UrlService.GetUrl() + "install/";

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShopInfo(InstallShopInfoModel model)
        {
            var handler = new InstallShopInfoHandler();

            if (!ModelState.IsValid)
            {
                var m = handler.Get();

                model.FaviconUrl = m.FaviconUrl;
                model.LogoUrl = m.LogoUrl;
                model.Countries = m.Countries;

                return PartialView("ShopInfo", model);
            }

            handler.Update(model);

            var step = (InstallStep)((int)InstallStep.Shopinfo + 1);
            return new RedirectResult(UrlService.GetUrl() + "install/" + step.ToString().ToLower());
        }


        public ActionResult Finance()
        {
            if (System.IO.File.Exists(SettingsGeneral.InstallFilePath))
                return new RedirectToRouteResult("Home", null);

            var model = new InstallFinanceHandler().Get();

            var step = (InstallStep)((int)InstallStep.Finance - 1);
            model.BackUrl = UrlService.GetUrl() + "install/" + step.ToString().ToLower();

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finance(InstallFinanceModel model)
        {
            var handler = new InstallFinanceHandler();

            if (!ModelState.IsValid)
            {
                var m = handler.Get();
                model.ShowBankSettings = m.ShowBankSettings;
                //model.StampUrl = m.StampUrl;

                return PartialView("Finance", model);
            }

            var step = (InstallStep)((int)InstallStep.Finance + 1);
            return new RedirectResult(UrlService.GetUrl() + "install/" + step.ToString().ToLower());
        }


        public ActionResult Payment()
        {
            if (System.IO.File.Exists(SettingsGeneral.InstallFilePath))
                return new RedirectToRouteResult("Home", null);

            var model = new InstallPaymentHandler().Get();

            var step = (InstallStep)((int)InstallStep.Payment - 1);
            model.BackUrl = UrlService.GetUrl() + "install/" + step.ToString().ToLower();

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(InstallPaymentModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Payment");
            }

            var handler = new InstallPaymentHandler();
            handler.Update(model);
            
            var step = (InstallStep)((int)InstallStep.Payment + 1);
            return new RedirectResult(UrlService.GetUrl() + "install/" + step.ToString().ToLower());
        }


        public ActionResult Shipping()
        {
            if (System.IO.File.Exists(SettingsGeneral.InstallFilePath))
                return new RedirectToRouteResult("Home", null);

            var model = new InstallShippingHandler().Get();

            var step = (InstallStep)((int)InstallStep.Shipping - 1);
            model.BackUrl = UrlService.GetUrl() + "install/" + step.ToString().ToLower();

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Shipping(InstallShippingModel model)
        {
            var handler = new InstallShippingHandler();

            if (!ModelState.IsValid)
            {
                var m = handler.Get();
                m.EDostNumer = model.EDostNumer;
                m.EDostPass = model.EDostPass;
                m.Courier = m.Courier;

                var stepback = (InstallStep)((int)InstallStep.Shipping - 1);
                m.BackUrl = UrlService.GetUrl() + "install/" + stepback.ToString().ToLower();

                return PartialView("Shipping", m);
            }
            
            handler.Update(model);

            var step = (InstallStep)((int)InstallStep.Shipping + 1);
            return new RedirectResult(UrlService.GetUrl() + "install/" + step.ToString().ToLower());
        }


        public ActionResult OpenId()
        {
            if (System.IO.File.Exists(SettingsGeneral.InstallFilePath))
                return new RedirectToRouteResult("Home", null);

            var model = new InstallOpenIdHandler().Get();

            var step = (InstallStep)((int)InstallStep.OpenId - 1);
            model.BackUrl = UrlService.GetUrl() + "install/" + step.ToString().ToLower();

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpenId(InstallOpenIdModel model)
        {
            var handler = new InstallOpenIdHandler();

            if (!ModelState.IsValid)
                return RedirectToAction("OpenId");

            handler.Update(model);

            var step = (InstallStep)((int)InstallStep.OpenId + 1);
            return new RedirectResult(UrlService.GetUrl() + "install/" + step.ToString().ToLower());
        }


        public ActionResult Notify()
        {
            if (System.IO.File.Exists(SettingsGeneral.InstallFilePath))
                return new RedirectToRouteResult("Home", null);

            var model = new InstallNotifyHandler().Get();

            var step = (InstallStep)((int)InstallStep.Notify - 1);
            model.BackUrl = UrlService.GetUrl() + "install/" + step.ToString().ToLower();

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Notify(InstallNotifyModel model)
        {
            var handler = new InstallNotifyHandler();

            if (!ModelState.IsValid)
            {
                var stepback = (InstallStep)((int)InstallStep.Notify - 1);
                model.BackUrl = UrlService.GetUrl() + "install/" + stepback.ToString().ToLower();

                return PartialView(model);
            }

            handler.Update(model);

            var step = (InstallStep)((int)InstallStep.Notify + 1);
            return new RedirectResult(UrlService.GetUrl() + "install/" + step.ToString().ToLower());
        }


        public ActionResult Final()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult FinalPost()
        {
            FileHelpers.CreateFile(SettingsGeneral.InstallFilePath);
            return new RedirectToRouteResult("Home", null);
        }

    }
}