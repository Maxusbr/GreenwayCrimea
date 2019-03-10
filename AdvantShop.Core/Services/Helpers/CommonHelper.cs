//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;

namespace AdvantShop.Helpers
{
    public class CommonHelper
    {
        public static void DisableBrowserCache()
        {
            if (HttpContext.Current == null) return;
            HttpContext.Current.Response.Cache.SetExpires(new DateTime(1995, 5, 6, 12, 0, 0, DateTimeKind.Utc));
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.AppendCacheExtension("post-check=0,pre-check=0");
        }

        public static void RegCloseScript(Page curpage, string url)
        {
            var jScript = new StringBuilder();
            jScript.Append("<script type=\'text/javascript\' language=\'javascript\'> ");
            if (string.IsNullOrEmpty(url))
                jScript.Append("window.opener.location.reload(true);");//jScript.Append("window.opener.location.href = window.opener.location.href;");
            else
                jScript.Append("window.opener.location =" + url);
            jScript.Append("self.close();");
            jScript.Append("</script>");
            Type csType = curpage.GetType();
            ClientScriptManager clScriptMng = curpage.ClientScript;
            clScriptMng.RegisterClientScriptBlock(csType, "Close_window", jScript.ToString());
        }

        /// <summary>
        /// set to responce any file
        /// </summary>
        /// <param name="filePath">virtual parh</param>
        /// <param name="fileName">file name</param>
        public static void WriteResponseFile(string filePath, string fileName)
        {
            if (String.IsNullOrEmpty(filePath)) return;
            var file = new FileInfo(filePath);
            var response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "utf-8";
            response.ContentType = "application/octet-stream";
            response.AddHeader("Connection", "Keep-Alive");
            response.AddHeader("Content-Length", file.Length.ToString());
            response.AddHeader("content-disposition", string.Format("attachment; filename={0}", file.Name));
            response.WriteFile(filePath);
            response.Flush();
            response.End();
        }

        /// <summary>
        /// Write XML to response
        /// </summary>
        /// <param name="xml">XML</param>
        /// <param name="fileName">Filename</param>
        public static void WriteResponseXml(string xml, string fileName)
        {
            if (String.IsNullOrEmpty(xml)) return;

            var document = new XmlDocument();
            document.LoadXml(xml);
            var decl = document.FirstChild as XmlDeclaration;
            if (decl != null)
            {
                decl.Encoding = "utf-8";
            }
            var response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "utf-8";
            response.ContentType = "text/xml";
            response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
            response.BinaryWrite(Encoding.UTF8.GetBytes(document.InnerXml));
            response.Flush();
            //response.End();
        }

        /// <summary>
        /// Write text to response
        /// </summary>
        /// <param name="txt">text</param>
        /// <param name="fileName">Filename</param>
        public static void WriteResponseTxt(string txt, string fileName)
        {
            if (String.IsNullOrEmpty(txt)) return;
            if (HttpContext.Current == null) return;
            var response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "utf-8";
            response.ContentType = "text/plain";
            response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
            response.BinaryWrite(Encoding.UTF8.GetBytes(txt));
            response.Flush();
            //response.End();
        }

        /// <summary>
        /// Write XLS file to response
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="targetFileName">Target file name</param>
        public static void WriteResponseXlsx(string filePath, string targetFileName)
        {
            if (String.IsNullOrEmpty(filePath)) return;
            if (HttpContext.Current == null) return;
            var response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "utf-8";
            response.ContentType = "text/xlsx";
            response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
            response.BinaryWrite(File.ReadAllBytes(filePath));
            response.Flush();
            //response.End();
        }

        /// <summary>
        /// Write PDF file to response
        /// </summary>
        /// <param name="filePath">File napathme</param>
        /// <param name="targetFileName">Target file name</param>
        /// <remarks>For BeatyStore project</remarks>
        public static void WriteResponsePdf(string filePath, string targetFileName)
        {
            if (String.IsNullOrEmpty(filePath)) return;
            if (HttpContext.Current == null) return;
            var response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "utf-8";
            response.ContentType = "text/pdf";
            response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
            response.BinaryWrite(File.ReadAllBytes(filePath));
            response.Flush();
            //response.End();
        }

        public static HttpCookie GetCookie(string cookieName)
        {
            return HttpContext.Current == null ? null : HttpContext.Current.Request.Cookies[cookieName];
        }

        public static void SetCookie(string cookieName, string cookieValue, bool httpOnly = false, bool crossSubDomains = true)
        {
            SetCookie(cookieName, cookieValue, new TimeSpan(7, 0, 0, 0), httpOnly, crossSubDomains);
        }

        public static void SetCookie(string cookieName, string cookieValue, TimeSpan ts, bool httpOnly, bool crossSubDomains = true)
        {
            try
            {
                var cookie = new HttpCookie(cookieName)
                {
                    Value = HttpUtility.UrlEncode(cookieValue),
                    Expires = DateTime.Now.Add(ts),
                    HttpOnly = httpOnly,
                };

                if (!isLocalUrl() && crossSubDomains)
                {
                    cookie.Domain = "." + GetParentDomain();
                }

                if (HttpContext.Current.Response.Cookies[cookieName] != null)
                {
                    HttpContext.Current.Response.Cookies.Remove(cookieName);
                }

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception exc)
            {
                Debug.Log.Error(exc);
            }
        }

        public static void SetCookieCollection(string cookieName, NameValueCollection cookieValue, TimeSpan ts)
        {
            try
            {
                var newCookie = new HttpCookie(cookieName);
                if (!CommonHelper.isLocalUrl())
                {
                    newCookie.Domain = "." + GetParentDomain();
                }
                for (int i = 0; i < cookieValue.Count; i++)
                {
                    newCookie.Values.Add(cookieValue.GetKey(i), cookieValue.Get(i));
                }
                newCookie.Expires = DateTime.Now.Add(ts);
                newCookie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(newCookie);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

        }

        /// <summary>
        /// Gets cookie string
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <returns>Cookie string</returns>
        public static string GetCookieString(string cookieName)//, bool decode)
        {
            /*
            var cookies = new List<HttpCookie>();
            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
            {
                cookies.Add(HttpContext.Current.Request.Cookies[i]);
            }
            //if (decode)
            var cookie = cookies.OrderBy(p => p.Expires).LastOrDefault(p => p.Name == cookieName);
            */
            var cookie = HttpContext.Current.Request.Cookies[cookieName];

            return cookie != null ? cookie.Value : string.Empty;
        }

        public static NameValueCollection GetCookieCollection(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] == null)
                return null;
            try
            {
                NameValueCollection tmp = HttpContext.Current.Request.Cookies[cookieName].Values;
                return tmp;
            }
            catch (Exception e)
            {
                Debug.Log.Error(e);
                return null;
            }
        }

        public static void DeleteCookie(string cookieName, bool crossSubDomains = true)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                var myCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1D) };
                HttpContext.Current.Response.Cookies.Add(myCookie);

                if (!isLocalUrl() && crossSubDomains)
                {
                    myCookie.Domain = "." + GetParentDomain();
                }
                    
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }

        public static string GetParentDomain()
        {
            return
                GetParentDomain(HttpContext.Current != null
                    ? HttpContext.Current.Request.Url.Host.ToLower()
                    : SettingsGeneral.AbsoluteUrlPath);
        }

        public static string GetParentDomain(string baseUrl)
        {
            var subStrings = new[] { "http://", "https://", "www.", "m.", "fb.", "vk." };

            foreach (string s in subStrings)
            {
                baseUrl = baseUrl.IndexOf(s, StringComparison.InvariantCultureIgnoreCase) == 0 ? baseUrl.Remove(0, s.Length) : baseUrl;
            }

            return baseUrl;
        }

        public static bool isLocalUrl()
        {
            var ip = new IPAddress(0);
            var isLocal = HttpContext.Current != null && 
                            (HttpContext.Current.Request.Url.ToString().Contains("localhost")
                            || HttpContext.Current.Request.Url.ToString().Contains("http://server")
                            || IPAddress.TryParse(HttpContext.Current.Request.Url.Host, out ip));

            return isLocal;
        }
    }
}