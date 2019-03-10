using AdvantShop.SEO;
using log4net.Appender;
using log4net.Core;
using System;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdvantShop.Core.Services.Diagnostics
{
    public class Db404Appender : AppenderSkeleton
    {
        public string HttpCodeError { get; set; }

        public override void ActivateOptions()
        {
            HttpCodeError = HttpCodeError ?? ConfigurationManager.AppSettings["Db404Appender.HttpCodeError"];
            base.ActivateOptions();
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (!string.IsNullOrWhiteSpace(HttpCodeError))
            {
                var codes = HttpCodeError.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var hex = loggingEvent.ExceptionObject as HttpException ?? loggingEvent.MessageObject as HttpException;
                var code = hex != null ? hex.GetHttpCode().ToString() : "0";

                if (!codes.Contains(code)) return;
            }

            var context = HttpContext.Current;
            var err404 = new Error404
            {
                Url = context.Request.RawUrl.TrimStart('/'),
                UrlReferer = context.Request.UrlReferrer != null ? context.Request.UrlReferrer.AbsoluteUri : string.Empty,
                IpAddress = context.Request.UserHostAddress,
                UserAgent = context.Request.UserAgent
            };
            Error404Service.AddError404(err404);
        }
    }
}
