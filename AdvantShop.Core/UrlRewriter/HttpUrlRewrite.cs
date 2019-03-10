//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Trial;
using System.IO;

namespace AdvantShop.Core.UrlRewriter
{
    public class HttpUrlRewrite : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }

        #endregion

        private static void OnBeginRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            string strCurrentUrl = app.Request.RawUrl.ToLower();
            app.StaticFile304();

            app.Response.Headers.Remove("Server");

            // Check cn
            if (AppServiceStartAction.state != PingDbState.NoError)
            {
                // Nothing here
                // just return
                return;
            }

            if (UrlService.IsIpBanned(app.Request.UserHostAddress))
            {
                app.Response.Clear();
                app.Response.Write("error: " + app.Request.UserHostAddress);
                app.Response.End();
                return;
            }

            if (UrlService.IsDebugUrl(strCurrentUrl) || strCurrentUrl.Contains("/adminv2/"))
            {
                return;
            }

            // Check original pictures
            if (strCurrentUrl.Contains("/pictures/product/original/"))
            {
                //throw new HttpException(404, "Not Found");
                app.RewriteTo404();
                return;
            }

            if (strCurrentUrl.Contains("/content/price_temp"))
            {
                var customer = CustomerContext.CurrentCustomer;
                if (customer == null ||
                    !(customer.IsAdmin || customer.IsVirtual || TrialService.IsTrialEnabled ||
                      (customer.IsModerator &&
                       RoleActionService.GetCustomerRoleActionsByCustomerId(customer.Id).Any(x => x.Role == RoleAction.Orders || x.Role == RoleAction.Catalog))))
                {
                    //throw new HttpException(404, "Not Found");
                    app.RewriteTo404();
                    return;
                }
            }

            string path = strCurrentUrl;
            if (app.Request.ApplicationPath != "/")
            {
                if (app.Request.ApplicationPath != null)
                    path = path.Replace(app.Request.ApplicationPath.ToLower(), "");
            }

            if (strCurrentUrl.Contains("robots.txt") && MobileHelper.IsMobileByUrl())
            {
                app.Context.RewritePath("areas/mobile/robots.txt");
                return;
            }

            var index = path.IndexOf('?');
            var pathWhitoutQuery = index > 0 ? path.Substring(0, index) : path;

            if (pathWhitoutQuery.Contains("/content/attachments/"))
            {
                var customer = CustomerContext.CurrentCustomer;
                if (customer == null || !(customer.IsAdmin || customer.IsVirtual || customer.IsModerator || TrialService.IsTrialEnabled))
                {
                    //throw new HttpException(404, "Not Found");
                    app.Response.Redirect(UrlService.GetAdminUrl("login") + "?from=" + strCurrentUrl);
                    return;
                }
            }

            var extention = FileHelpers.GetExtension(pathWhitoutQuery);
            if (UrlService.ExtentionNotToRedirect.Contains(extention))
            {
                if (app.Request["OpenInBrowser"] == "true")
                {
                    app.Response.Clear();
                    app.Response.ContentType = "text/plain";
                    app.Response.AddHeader("content-disposition", "inline;filename=" + Path.GetFileName(app.Request.FilePath));
                    app.Response.WriteFile(System.Web.Hosting.HostingEnvironment.MapPath(app.Request.FilePath));
                    app.Response.End();
                }
                return;
            }

            if (path.Contains("myaccount") && MobileHelper.IsMobileForced())
            {
                app.Response.RedirectToRoute("Home", null);
            }

            //301 redirect if need
            if (SettingsSEO.Enabled301Redirects && !path.Contains("/admin/") && !path.Contains("/api/") && !path.Contains("paymentnotification"))
            {
                var newUrl = UrlService.GetRedirect301(path.Trim('/'), app.Request.Url.AbsoluteUri.Trim('/'));
                if (newUrl.IsNotEmpty())
                {
                    app.Response.RedirectPermanent(newUrl);
                    return;
                }

                var dirPath = path.Split("?").FirstOrDefault();
                if (dirPath != "/" && dirPath.EndsWith("/"))
                {
                    app.Response.RedirectPermanent(app.Request.Url.AbsoluteUri.Split("?").FirstOrDefault().Trim('/') + app.Request.Url.Query);
                    return;
                }

                // checking doble slashes
                string requestUrl = app.Request.ServerVariables["REQUEST_URI"];
                string rewriteUrl = app.Request.ServerVariables["UNENCODED_URL"];

                int startIndex = rewriteUrl.IndexOf('?');
                if (startIndex > 0)
                    rewriteUrl = rewriteUrl.Substring(0, startIndex);

                if (rewriteUrl.Contains("//") && !requestUrl.Contains("//"))
                {
                    app.Response.RedirectPermanent(requestUrl);
                    return;
                }
            }

            UrlService.ESocialType socialType;
            
            if (MobileHelper.IsMobileEnabled())
            {
                if (SettingsMobile.RedirectToSubdomain && !MobileHelper.IsMobileByUrl())
                {
                    //app.Response.Redirect(app.Request.Url.AbsoluteUri.Replace("http://", "http://m.").Replace("https://", "https://m."));
                    MobileHelper.RedirectToMobile(app.Context);
                }

                SettingsDesign.IsMobileTemplate = SettingsMobile.IsMobileTemplateActive;
                
                if (!SettingsDesign.IsMobileTemplate && MobileHelper.IsMobileByUrl())
                {
                    app.Context.RewritePath("error/forbidden");
                    return;
                }
            }

            if ((socialType = UrlService.IsSocialUrl(app.Context.Request.Url.ToString().ToLower())) != UrlService.ESocialType.none)
            {
                if ((socialType == UrlService.ESocialType.vk && !SettingsDesign.IsVkTemplateActive) ||
                    (socialType == UrlService.ESocialType.fb && !SettingsDesign.IsFbTemplateActive))
                {
                    app.Context.RewritePath("error/forbidden");
                    return;
                }                
            }

            var modules = AttachedModules.GetModules<IModuleUrlRewrite>();
            foreach (var moduleType in modules)
            {
                var moduleObject = (IModuleUrlRewrite)Activator.CreateInstance(moduleType);
                var newUrl = path;
                if (moduleObject != null && moduleObject.RewritePath(path, ref newUrl))
                {
                    app.Context.RewritePath(newUrl);
                    return;
                }
            }
        }
    }
}