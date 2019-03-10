using System;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Web.Infrastructure.Filters.Headers;

namespace AdvantShop
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CompressFilter());
            //filters.Add(new UnhandleExceptionAttribute());
            //filters.Add(new ExceptionFilter());

#if DEBUG
            if (SettingProvider.GetConfigSettingValue("Profiling") == "true")
                filters.Add(new ProfilingActionFilter());
#endif
            //add security headers
            //filters.Add(new XContentTypeOptionsAttribute());
            //filters.Add(new XDownloadOptionsAttribute());
            //filters.Add(new XFrameOptionsAttribute { Policy = XFrameOptionsPolicy.Disabled });
            //filters.Add(new XXssProtectionAttribute(XXssProtectionPolicy.FilterEnabled, true));
            //filters.Add(new HttpStrictTransportSecurityAttribute { MaxAge = new TimeSpan(365, 0, 0, 0), IncludeSubdomains = true, Preload = true });
        }
    }
}