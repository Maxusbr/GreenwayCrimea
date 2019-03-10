//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web;
using AdvantShop.Core.Services.Configuration.Settings;

namespace AdvantShop.Helpers
{
    public class MobileHelper
    {

        private static readonly string[] Mobiles = 
                {
                    "midp", "j2me", "avant", "docomo", 
                    "novarra", "palmos", "palmsource", 
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/", 
                    "blackberry", "mib/", "symbian", 
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio", 
                    "sie-", "sec-", "samsung", "HTC", 
                    "mot-", "mitsu", "sagem", "sony",
                    "alcatel", "lg", "eric", "vx", 
                    "nec", "philips", "mmm", "xx", 
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", 
                    "pt", "pg", "vox", "amoi", 
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo", 
                    "sgh", "gradi", "jb", "dddi", 
                    "moto", "iphone" , "mobile", "lumia"
                };

        private static readonly string[] Tablets = 
                {
                    "tablet", "ipad", "playbook", "bb10",
                    "z30", "nexus 10", "nexus 7", "gt-p",
                    "sch-i800", "xoom", "kindle", "silk" 
                };

        public static bool IsMobileEnabled()
        {
          return (IsMobileByUrl() || IsMobileForced() || IsMobileBrowser()) && !IsDesktopForced();
        }

        public static void RedirectToMobile(HttpContext context)
        {
            //if (((IsMobileBrowser() && !IsDesktopForced() && !IsMobileByUrl()) || 
            //    (context.Items["RedirectToMobile"] != null && context.Items["RedirectToMobile"].ToString() == "true")) && 
            //    !CommonHelper.isLocalUrl())
            {
                context.Items.Remove("RedirectToMobile");
                context.Response.Redirect("http://m." + (CommonHelper.GetParentDomain() + context.Request.RawUrl));
            }
        }

        public static void RedirectToDesktop(HttpContext context)
        {
            SetDesktopForcedCookie();
            context.Response.Redirect("http://" + CommonHelper.GetParentDomain() + context.Request.RawUrl);
        }
        
        /// <summary>
        /// Detect mobile browsers by USER_AGENT context. Mobile != Tablets
        /// </summary>
        public static bool IsMobileBrowser()
        {
            var context = HttpContext.Current;

            if (context.Items["IsMobileBrowser"] != null)
                return Convert.ToBoolean(context.Items["IsMobileBrowser"]);


            //default check 
            //if (context.Request.Browser.IsMobileDevice)
            //{
            //    return true;
            //}

            var ua = context.Request.ServerVariables["HTTP_USER_AGENT"];
            if (String.IsNullOrEmpty(ua))
                return false;

            var currnetUserAgent = ua.ToLower();
            var isMobile = Mobiles.Any(currnetUserAgent.Contains) && !Tablets.Any(currnetUserAgent.Contains);

            context.Items["IsMobileBrowser"] = isMobile;

            return isMobile;
        }


        /// <summary>
        /// Go to mobile version forced by domain
        /// </summary>
        public static bool IsMobileByUrl()
        {
            HttpContext context = HttpContext.Current;
            return context.Request.Url.AbsoluteUri.StartsWith("http://m.") || context.Request.Url.AbsoluteUri.StartsWith("https://m.");
        }

        public static bool IsDesktopForced()
        {
            var isDesktopForced = CommonHelper.GetCookie("ForcedDesktop");
            var result = isDesktopForced != null && 
                         isDesktopForced.Value == "true" &&
                         HttpContext.Current.Items["_ForcedMobile"] == null;

            if (!result && HttpContext.Current.Request.QueryString["ForcedDesktop"] == "true")
            {
                SetDesktopForcedCookie();
                RedirectToDesktop(HttpContext.Current);
                result = true;
            }

            return result;
        }

        public static bool IsMobileForced()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ForcedDesktop"]) || !SettingsMobile.IsMobileTemplateActive) {
                SetDesktopForcedCookie();
                return false;
            }

            var forcedCookie = CommonHelper.GetCookie("ForcedMobile");
            var result = forcedCookie != null && forcedCookie.Value == "true";

            if (!result && HttpContext.Current.Request.QueryString["ForcedMobile"] == "true")
            {
                SetMobileForcedCookie();
                result = true;
                HttpContext.Current.Items["_ForcedMobile"] = true;
            }
            return result;
        }
        
        public static void SetDesktopForcedCookie()
        {
            CommonHelper.SetCookie("ForcedDesktop", "true", false);
            CommonHelper.DeleteCookie("ForcedMobile");
        }

        public static void SetMobileForcedCookie()
        {
            CommonHelper.SetCookie("ForcedMobile", "true", false);
            CommonHelper.DeleteCookie("ForcedDesktop");
        }

        public static void DeleteDesktopForcedCookie()
        {
            HttpContext.Current.Items.Add("RedirectToMobile", "true");
            CommonHelper.DeleteCookie("ForcedDesktop");
        }

    }
}