using log4net.Appender;
using log4net.Config;
using log4net.Core;
using RollbarSharp;
using RollbarSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AdvantShop.Core.Services.Diagnostics
{
    public class RollbarAppenderConfigurator
    {
        public static void Configure(string accessToken = null, Action<RollbarAppender> configureAppender = null)
        {
            var appender = new RollbarAppender { AccessToken = accessToken };

            if (configureAppender != null)
            {
                configureAppender(appender);
            }

            appender.ActivateOptions();

            BasicConfigurator.Configure(appender);
        }

    }

    public class RollbarAppender : AppenderSkeleton
    {
        private RollbarSharp.Configuration _configuration;

        public string AccessToken { get; set; }
        public string Environment { get; set; }
        public string Endpoint { get; set; }
        public string Framework { get; set; }
        public string GitSha { get; set; }
        public string Language { get; set; }
        public string Platform { get; set; }
        public string ScrubParams { get; set; }

        public string HttpCodeErrorNotStored { get; set; }

        public override void ActivateOptions()
        {
            _configuration = new RollbarSharp.Configuration(GetConfigSetting(AccessToken, "Rollbar.AccessToken"));

            _configuration.Endpoint = GetConfigSetting(Endpoint, "Rollbar.Endpoint", _configuration.Endpoint);
            _configuration.Environment = GetConfigSetting(Environment, "Rollbar.Environment", _configuration.Environment);
            _configuration.Framework = GetConfigSetting(Framework, "Rolllbar.Framework", _configuration.Framework);
            _configuration.GitSha = GetConfigSetting(GitSha, "Rollbar.GitSha");
            _configuration.Language = GetConfigSetting(Language, "Rollbar.CodeLanguage", _configuration.Language);
            _configuration.Platform = GetConfigSetting(Platform, "Rollbar.Platform", _configuration.Platform);

            var scrubParams = GetConfigSetting(ScrubParams, "Rollbar.ScrubParams");
            _configuration.ScrubParams = scrubParams == null ?
                RollbarSharp.Configuration.DefaultScrubParams : scrubParams.Split(',');
        }

        private static string GetConfigSetting(string param, string name, string fallback = null)
        {
            return param ?? ConfigurationManager.AppSettings[name] ?? fallback;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (!string.IsNullOrWhiteSpace(HttpCodeErrorNotStored))
            {
                var codes = HttpCodeErrorNotStored.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var hex = loggingEvent.ExceptionObject as HttpException ?? loggingEvent.MessageObject as HttpException;
                var code = hex != null ? hex.GetHttpCode().ToString() : "0";

                if (codes.Contains(code)) return;
            }


            var client = new RollbarClient(_configuration);

            if (loggingEvent.Level >= Level.Critical)
            {
                Send(loggingEvent, client.SendCriticalMessage, client.SendCriticalException);
            }
            else if (loggingEvent.Level >= Level.Error)
            {
                Send(loggingEvent, client.SendErrorMessage, client.SendErrorException);
            }
            else if (loggingEvent.Level >= Level.Warn)
            {
                Send(loggingEvent, client.SendWarningMessage, client.SendWarningException);
            }
            else if (loggingEvent.Level >= Level.Info)
            {
                client.SendInfoMessage(loggingEvent.RenderedMessage);
            }
            else if (loggingEvent.Level >= Level.Debug)
            {
                client.SendDebugMessage(loggingEvent.RenderedMessage);
            }
        }

        private void Send(LoggingEvent loggingEvent,
                          Func<string, IDictionary<string, object>, Action<DataModel>, object, Task> sendMessage,
                          Func<Exception, string, Action<DataModel>, object, Task> sendException)
        {
            if (loggingEvent.ExceptionObject == null)
            {
                sendMessage(loggingEvent.RenderedMessage, null, null, null);
            }
            else
            {
                sendException(loggingEvent.ExceptionObject, null, null, null);
            }
        }
    }
}
