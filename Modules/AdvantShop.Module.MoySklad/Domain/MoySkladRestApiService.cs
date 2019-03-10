using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using AdvantShop.Diagnostics;

namespace AdvantShop.Module.MoySklad.Domain
{
    public class MoySkladRestApiService
    {
        private const string MoySkladApiUrl = "https://online.moysklad.ru/exchange/rest";
        
        #region Private methods

        internal static string MakeRequest(string url, string data = null, string method = "GET", string contentType = "application/xml")
        {
            try
            {
                var request = WebRequest.Create(MoySkladApiUrl + url) as HttpWebRequest;
                request.Method = method;
                request.Credentials = new NetworkCredential(MoySkladApiSettings.Login, MoySkladApiSettings.Password);
                request.ContentType = contentType;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

                if (!string.IsNullOrEmpty(data))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                }

                var responseContent = "";
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
            catch (WebException ex)
            {
                try
                {
                    var result = "";

                    using (var eResponse = ex.Response)
                    {
                        if (eResponse != null)
                        {
                            if (!string.IsNullOrEmpty(eResponse.Headers["X-Lognex-Auth"]))
                                result += string.Format("Расширенный код ошибки {0}. ",
                                    eResponse.Headers["X-Lognex-Auth"]);

                            if (!string.IsNullOrEmpty(eResponse.Headers["X-Lognex-Auth-Message"]))
                                result += string.Format("{0}. ", eResponse.Headers["X-Lognex-Auth-Message"]);

                            if (!string.IsNullOrEmpty(eResponse.Headers["X-Lognex-API-Version-Deprecated"]))
                                result += string.Format("Дата отключения запрошенной версии API {0}. ",
                                    eResponse.Headers["X-Lognex-API-Version-Deprecated"]);

                            using (var eStream = eResponse.GetResponseStream())
                            {
                                using (var reader = new StreamReader(eStream))
                                {
                                    result += reader.ReadToEnd();
                                    Debug.Log.Error(result);
                                }
                            }
                        }
                    }
                }
                catch (Exception exIn)
                {
                    Debug.Log.Error(exIn);
                }
                Debug.Log.Error(ex);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return null;
        }

        #endregion

        #region Moy sklad api methods

        public static string GetCounterpartyId(string externalCode)
        {
            var response =
                MoySkladApiService.MakeRequest<EntityCounterpartyContactpersonsResponse>(
                    "/entity/counterparty?filter=" + HttpUtility.UrlEncode("externalCode=" + externalCode),
                    method: "GET");

            if (response != null && response.Rows != null && response.Rows.Count > 0 &&
                response.Rows[0].ExternalCode == externalCode)
            {
                return response.Rows[0].Id;
            }

            return null;
        }

        #endregion

    }
}