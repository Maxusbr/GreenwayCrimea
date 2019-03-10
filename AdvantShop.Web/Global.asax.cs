using System;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.App.Landing;
using AdvantShop.Areas.AdminMobile;
using AdvantShop.Areas.Api;
using AdvantShop.Areas.Mobile;
using AdvantShop.Configuration;
using AdvantShop.Controllers;
using AdvantShop.Core;
using AdvantShop.Core.Services.Helpers;
using AdvantShop.Diagnostics;
using AdvantShop.Web.Admin;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Web.Infrastructure.Templates;
using AdvantShop.Core.UrlRewriter;

namespace AdvantShop
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            MvcHandler.DisableMvcResponseHeader = true;
            ApplicationService.StartApplication(HttpContext.Current);

            // Replace the default RazorViewEngine with our custom RazorThemeViewEngine
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorThemeViewEngine());
            ConfigureAntiForgeryTokens();

            //AreaRegistration.RegisterAllAreas();
            RegisterArea<LandingAreaRegistration>(RouteTable.Routes);
            RegisterArea<AdminMobileAreaRegistration>(RouteTable.Routes);
            RegisterArea<ApiAreaRegistration>(RouteTable.Routes);
            RegisterArea<MobileAreaRegistration>(RouteTable.Routes);
            RegisterArea<AdminAreaRegistration>(RouteTable.Routes);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BinderConfig.Regist();

            //exclude filter if need
            var providers = FilterProviders.Providers.ToArray();
            FilterProviders.Providers.Clear();
            FilterProviders.Providers.Add(new ExcludeFilterProvider(providers));
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            SessionServices.StartSession(HttpContext.Current);

            if (AppServiceStartAction.state == PingDbState.NoError)
            {
                Core.Modules.ModulesService.CallModulesUpdate();

                if (SettingsMain.CurrentFilesStorageSize == 0 || SettingsMain.CurrentFilesStorageLastUpdateTime == DateTime.MinValue)
                    FilesStorageService.RecalcAttachmentsSizeInBackground();
                
                ApplicationService.UpdateRoutes();
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (AppServiceStartAction.state == PingDbState.NoError)
            {
                Localization.Culture.InitializeCulture();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();

            Debug.Log.Error(ex.Message, ex);
            var httpException = ex as HttpException;
            if (httpException != null)
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", "Error");

                switch (httpException.GetHttpCode())
                {
                    case 400:
                        routeData.Values.Add("action", "BadRequest");
                        break;
                    case 403:
                        routeData.Values.Add("action", "Forbidden");
                        break;
                    case 404:
                        routeData.Values.Add("action", "NotFound");
                        break;
                    //case 500:
                    //    routeData.Values.Add("action", "InternalServerError");
                    //    break;
                    default:
                        return;
                }

                Response.Clear();
                Server.ClearError();
                Response.TrySkipIisCustomErrors = true;

                IController errorController = new ErrorController();
                errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }



        private static void RegisterArea<T>(RouteCollection routes) where T : AreaRegistration
        {
            var registration = (AreaRegistration)Activator.CreateInstance(typeof(T));
            var registrationContext = new AreaRegistrationContext(registration.AreaName, routes, null);
            var areaNamespace = registration.GetType().Namespace;

            if (!String.IsNullOrEmpty(areaNamespace))
                registrationContext.Namespaces.Add(areaNamespace + ".*");

            registration.RegisterArea(registrationContext);
        }

        private static void ConfigureAntiForgeryTokens()
        {
            AntiForgeryConfig.CookieName = "f";
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            // AntiForgeryConfig.RequireSsl = true;
        }
    }
}