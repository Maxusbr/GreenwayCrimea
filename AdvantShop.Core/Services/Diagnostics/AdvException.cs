//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace AdvantShop.Diagnostics
{
    public class AdvException
    {
        public class CommonExceptionData
        {

            public string ExceptionMessage { get; set; }
            public string ManualMessage { get; set; }

            public string ExceptionStackTrace { get; set; }

            public string InnerExceptionMessage { get; set; }
            public string InnerExceptionStackTrace { get; set; }

            public Dictionary<string, string> Parameters { get; set; }


            public CommonExceptionData()
            {
                Parameters = new Dictionary<string, string>();
            }

            public CommonExceptionData(Exception exception)
            {
                ExceptionMessage = exception.Message;
                ExceptionStackTrace = exception.StackTrace;
                var innerEx = exception.InnerException;
                while (innerEx != null)
                {
                    InnerExceptionMessage = exception.InnerException.Message;
                    InnerExceptionStackTrace = exception.InnerException.StackTrace;
                    innerEx = innerEx.InnerException;
                }

                Parameters = new Dictionary<string, string>();
                foreach (var key in exception.Data.Keys)
                {
                    Parameters.Add(key.ToString(), exception.Data[key].ToString());
                }
            }
        }

        public class RequestExceptionData
        {
            public Dictionary<string, string> ColectionData;

            public RequestExceptionData()
            {
                ColectionData = new Dictionary<string, string>();
            }

            public RequestExceptionData(HttpRequest httpRequest)
            {
                ColectionData = new Dictionary<string, string>
                {
                    {"IsLocal", httpRequest.IsLocal.ToString(CultureInfo.InvariantCulture)},
                    {"HttpMethod", httpRequest.HttpMethod},
                    {"RequestType", httpRequest.RequestType},
                    {"ContentLength", httpRequest.ContentLength.ToString(CultureInfo.InvariantCulture)},
                    {"ContentEncoding", httpRequest.ContentEncoding.ToString()},
                    {"FilePath", httpRequest.FilePath},
                    {"UserAgent", httpRequest.UserAgent},
                    {"UserHostName", httpRequest.UserHostName},
                    {"UserHostAddress", httpRequest.UserHostAddress},
                    {"RawUrl", httpRequest.RawUrl},
                    {"Url", httpRequest.Url.ToString()},
                    {"UrlReferrer", httpRequest.UrlReferrer != null ? httpRequest.UrlReferrer.ToString() : "N/A"}
                };

                for (int i = 0; i < httpRequest.Headers.Count; i++)
                {
                    ColectionData.Add(httpRequest.Headers.GetKey(i), httpRequest.Headers[i]);
                }

                for (int i = 0; i < httpRequest.ServerVariables.Count; i++)
                {
                    ColectionData.Add(httpRequest.ServerVariables.GetKey(i), httpRequest.ServerVariables[i]);
                }

                if (httpRequest.RequestType == "POST")
                {
                    var postData = httpRequest.Form.ToString();
                    if (!string.IsNullOrEmpty(postData))
                    {
                        ColectionData.Add("Post data", postData);
                    }
                }

                //var properties = httpRequest.GetType().GetProperties();
                //foreach (var p in properties)
                //{

                //    if (p.Name == "Params" || p.Name == "Item" || p.Name == "HttpChannelBinding")
                //    {
                //        continue;
                //    }

                //    var o = p.GetValue(httpRequest, null);
                //    if (o is NameValueCollection)
                //    {
                //        foreach (string key in ((NameValueCollection)o))
                //        {
                //            if (key != "ALL_HTTP" && key != "ALL_RAW")
                //            {
                //                ColectionData.Add(key, httpRequest.Params[key]);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        ColectionData.Add(p.Name, o != null ? o.ToString() : "n\a");
                //    }
                //}
            }
        }

        public class BrowserExceptionData
        {
            public Dictionary<string, string> ColectionData;

            public BrowserExceptionData()
            {
                ColectionData = new Dictionary<string, string>();
            }

            public BrowserExceptionData(HttpBrowserCapabilities browser)
            {
                ColectionData = new Dictionary<string, string>
                {
                    {"Type", browser.Type},
                    {"Browser", browser.Browser},
                    {"Version", browser.Version},
                    {"Platform", browser.Platform},
                    {"EcmaScriptVersion", browser.EcmaScriptVersion.ToString()},
                    {"MSDomVersion", browser.MSDomVersion.ToString()},
                    {"Beta", browser.Beta.ToString(CultureInfo.InvariantCulture)},
                    {"Crawler", browser.Crawler.ToString(CultureInfo.InvariantCulture)},
                    {"Win32", browser.Win32.ToString(CultureInfo.InvariantCulture)},
                    {"Win16", browser.Win16.ToString(CultureInfo.InvariantCulture)},
                    {"Frames", browser.Frames.ToString(CultureInfo.InvariantCulture)},
                    {"Tables", browser.Tables.ToString(CultureInfo.InvariantCulture)},
                    {"Cookies", browser.Cookies.ToString(CultureInfo.InvariantCulture)},
                    {"VBScript", browser.VBScript.ToString(CultureInfo.InvariantCulture)},
                    {"JavaScript", browser.EcmaScriptVersion.ToString()},
                    {"JavaApplets", browser.JavaApplets.ToString(CultureInfo.InvariantCulture)},
                    {"JScriptVersion", browser.JScriptVersion.ToString()},
                    {"ActiveXControls", browser.ActiveXControls.ToString(CultureInfo.InvariantCulture)},
                    {"BackgroundSounds", browser.BackgroundSounds.ToString(CultureInfo.InvariantCulture)},
                    {"CDF", browser.CDF.ToString(CultureInfo.InvariantCulture)},
                    {"IsMobileDevice", browser.IsMobileDevice.ToString(CultureInfo.InvariantCulture)},
                    {"MobileDeviceManufacturer", browser.MobileDeviceManufacturer},
                    {"MobileDeviceModel", browser.MobileDeviceModel},
                    {"ScreenPixelsHeight", browser.ScreenPixelsHeight.ToString(CultureInfo.InvariantCulture)},
                    {"ScreenPixelsWidth", browser.ScreenPixelsWidth.ToString(CultureInfo.InvariantCulture)},
                    {"ScreenBitDepth", browser.ScreenBitDepth.ToString(CultureInfo.InvariantCulture)},
                    {"IsColor", browser.IsColor.ToString(CultureInfo.InvariantCulture)},
                    {"InputType", browser.InputType}
                };

                //var properties = browser.GetType().GetProperties();
                //foreach (var p in properties)
                //{
                //    if (p.Name == "Item" || p.Name == "HtmlTextWriter" || p.Name == "PreferredRequestEncoding" || p.Name == "PreferredResponseEncoding" || p.Name == "RequiredMetaTagNameValue")
                //    {
                //        continue;
                //    }
                //    var o = p.GetValue(browser, null);
                //    ColectionData.Add(p.Name, o.ToString());
                //}
            }
        }

        public class SessionExceptionData
        {
            public Dictionary<string, string> ColectionData;

            public SessionExceptionData()
            {
                ColectionData = new Dictionary<string, string>();
            }

            public SessionExceptionData(HttpSessionState session)
            {
                ColectionData = new Dictionary<string, string>
                {
                    {"SessionID", session.SessionID},
                    {"Timeout", session.Timeout.ToString(CultureInfo.InvariantCulture)},
                    {"IsNewSession", session.IsNewSession.ToString(CultureInfo.InvariantCulture)},
                    {"IsCookieless", session.IsCookieless.ToString(CultureInfo.InvariantCulture)},
                    {"Mode", session.Mode.ToString()},
                    {"CookieMode", session.CookieMode.ToString()}
                };

                //var properties = session.GetType().GetProperties();
                //foreach (var p in properties)
                //{
                //    //skip error
                //    if (p.Name == "Item")
                //        continue;

                //    var o = p.GetValue(session, null);
                //    ColectionData.Add(p.Name, o.ToString());
                //}
                foreach (string key in session.Keys)
                {
                    ColectionData.Add(key, session[key] != null ? session[key].ToString() : string.Empty);
                }
            }
        }

        public CommonExceptionData ExceptionData { get; set; }
        public RequestExceptionData RequestData { get; set; }
        public BrowserExceptionData BrowserData { get; set; }
        public SessionExceptionData SessionData { get; set; }

        public AdvException()
        {
            ExceptionData = new CommonExceptionData();
            RequestData = new RequestExceptionData();
            BrowserData = new BrowserExceptionData();
            SessionData = new SessionExceptionData();
        }

        public AdvException(Exception exception)
        {
            ExceptionData = new CommonExceptionData(exception);

            try
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    RequestData = new RequestExceptionData(context.Request);
                    BrowserData = new BrowserExceptionData(context.Request.Browser);
                    if (context.Session != null)
                        SessionData = new SessionExceptionData(context.Session);
                }
            }
            catch (Exception)
            {
            }
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static AdvException GetFromJsonString(string str)
        {
            return JsonConvert.DeserializeObject<AdvException>(str);
        }
    }
}