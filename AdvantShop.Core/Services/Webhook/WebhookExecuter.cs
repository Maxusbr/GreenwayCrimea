using System;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.Webhook
{
    public class WebhookExecuter
    {
        public static void MakeRequestAsync<TUrlList, TUrl>(Expression<Func<TUrl, bool>> predicate, object data = null) where TUrlList : WebhookUrlList<TUrl>, new() where TUrl : WebhookUrl
        {
            Task.Factory.StartNew(() =>
            {
                MakeRequest<TUrlList, TUrl>(predicate, data);
            });
        }

        public static void MakeRequest<TUrlList, TUrl>(Expression<Func<TUrl, bool>> predicate, object data = null) where TUrlList : WebhookUrlList<TUrl>, new() where TUrl : WebhookUrl
        {
            var urlList = WebhookUrlListProvider<TUrlList, TUrl>.WebhookUrlList;
            var service = WebhookSericeProvider.Services.Get(urlList.WebhookType);
            if (service.Enabled && service.ApiKey.IsNotEmpty())
            {
                foreach (var url in urlList.GetUrlsFiltered(predicate))
                {
                    MakeRequest(string.Format("{0}{1}apikey={2}", url, url.Contains("?") ? "&" : "?", service.ApiKey), data);
                }
            }
        }

        public static void MakeSystemRequest(string url, object data = null, bool async = true)
        {
            url = string.Format("{0}/{1}", UrlService.GetUrl("webhook/" + url.TrimEnd('/')), WebhookRepository.SystemApiKey);
            if (async)
                Task.Factory.StartNew(() => { MakeRequest(url, data); });
            else
                MakeRequest(url, data);
        }

        public static string MakeRequest(string url, object data = null)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";

                if (data != null)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                }
                else
                {
                    request.ContentLength = 0;
                }

                var responseContent = string.Empty;
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                    }
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(string.Format("webhook to url {0}. {1}", url, ex.Message), ex);
                return ex.Message;
            }
        }
    }
}
