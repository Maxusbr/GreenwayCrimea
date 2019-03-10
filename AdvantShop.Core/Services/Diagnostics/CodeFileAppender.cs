using log4net.Appender;
using log4net.Core;
using System;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdvantShop.Core.Services.Diagnostics
{
    public class CodeFileAppender : RollingFileAppender
    {
        public string HttpCodeError { get; set; }

        public override void ActivateOptions()
        {
            HttpCodeError = HttpCodeError ?? ConfigurationManager.AppSettings["CodeFileAppender.HttpCodeError"];
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
            base.Append(loggingEvent);           
        }       
    }
}
