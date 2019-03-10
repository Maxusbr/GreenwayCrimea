using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.Models.Error;
using AdvantShop.Saas;
using AdvantShop.Trial;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    [ExcludeFilter(typeof(CompressFilter),typeof(SaasStoreAttribute))]
    public class ErrorController : BaseClientController
    {
        // some error we dont know
        public ActionResult Index()
        {
            SetResponse(HttpStatusCode.InternalServerError);
            return View();
        }

        // 400
        public ActionResult BadRequest()
        {
            SetResponse(HttpStatusCode.BadRequest);
            return View();
        }

        // 404
        public ActionResult NotFound()
        {
            SetResponse(HttpStatusCode.NotFound);
            var ext = VirtualPathUtility.GetExtension(Request.RawUrl);
            if (ext != null)
            {
                var list = new List<string> { ".css", ".js", ".jpg", ".jpeg", ".png", ".map", ".ico", ".gif" };
                if (list.Contains(ext.ToLower()))
                    return new EmptyResult();
            }
            SetMetaInformation(T("Error.NotFound.Title"));
            return View();
        }

        // 403
        public ActionResult Forbidden()
        {
            SetResponse(HttpStatusCode.Forbidden);
            return View();
        }

        // 500
        public ActionResult InternalServerError()
        {
            SetResponse(HttpStatusCode.InternalServerError);
            return View();
        }

        public ActionResult LicCheck()
        {
            if (SettingsLic.ActiveLic && TrialService.IsTrialEnabled)
                return RedirectToRoute("Home");

            if (SettingsLic.ActiveLic && SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.IsCorrect)
                return RedirectToRoute("Home");

            if (!string.IsNullOrEmpty(SettingsLic.LicKey) && ChecLicKey(SettingsLic.LicKey))
            {
                if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.IsCorrect)
                {
                    return View(new LicCheckModel() { Msg = "Check Web.ModeSettings.config" });
                }
                //return RedirectToRoute("Home");
            }

            return View(new LicCheckModel());
        }

        [HttpPost]
        public ActionResult LicCheck(LicCheckModel model)
        {
            if (SettingsLic.ActiveLic && SaasDataService.CurrentSaasData.IsCorrect)
                return RedirectToRoute("Home");

            var viewModel = model;

            if (string.IsNullOrWhiteSpace(model.Key))
                return View(viewModel);

            if (ChecLicKey(model.Key))
            {
                SettingsLic.ActiveLic = true;
                SettingsLic.LicKey = model.Key;
            }
            else
            {
                viewModel.Msg = "key is wrong";
            }
            
            return View(viewModel);
        }

        private void SetResponse(HttpStatusCode httpStatusCode)
        {
            try
            {
                Response.Clear();
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)httpStatusCode;
                Response.StatusDescription = HttpWorkerRequest.GetStatusDescription((int)httpStatusCode);
            }
            catch
            {
                
            }
        }

        private bool ChecLicKey(string key)
        {
            return SettingsLic.Activate(key);
        }

        protected override void OnActionExecuted(ActionExecutedContext context)
        {
            // TODO: log error?
        }
    }
}