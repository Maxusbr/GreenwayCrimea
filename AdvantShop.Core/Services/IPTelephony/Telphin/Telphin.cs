using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.IPTelephony.Telphin
{
    public class Telphin : IPTelephonyOperator
    {
        private const string ServiceUrl = "https://apiproxy.telphin.ru";

        private string _appKey;
        private string _appSecret;

        public override EOperatorType Type
        {
            get { return EOperatorType.Telphin; }
        }

        public Telphin()
        {
            _appKey = SettingsTelephony.TelphinAppKey;
            _appSecret = SettingsTelephony.TelphinAppSecret;
        }

        public override CallBack.CallBack CallBack
        {
            get { return new TelphinCallBack(); }
        }

        public override bool WebPhoneActive
        {
            get { return false; }
        }

        public override string RenderBottomScript()
        {
            return string.Empty;
        }

        public override string RenderIntoHead()
        {
            return string.Empty;
        }

        public override string GetRecordLink(int callId)
        {
            var call = CallService.GetCall(callId);
            if (call == null)
                return string.Empty;
            var recId = call.RecordLink;
            if (recId.IsNullOrEmpty())
            {
                var callInfo = GetCallInfo(call.CallId);
                if (callInfo != null && callInfo.Cdr != null && callInfo.Cdr.Any())
                    recId = callInfo.Cdr[0].RecordUuid;
            }
            if (recId.IsNullOrEmpty())
                return string.Empty;

            var result = MakeAuthRequest<TelphinRecordInfoResponse>(
                string.Format("{0}/api/ver1.0/client/@me/record/{1}/storage_url/", ServiceUrl, recId),
                method: "GET");

            return result != null ? result.RecordUrl : string.Empty;
        }

        public TelphinCallInfoResponse GetCallInfo(string callId)
        {
            return MakeAuthRequest<TelphinCallInfoResponse>(
                string.Format("{0}/api/ver1.0/client/@me/call_history/{1}", ServiceUrl, callId),
                method: "GET");
        }

        /// <summary>
        /// Инициировать вызов. После успешного API-запроса, системой вызываются все номера-источники вызова, а после поднятия трубки одним из них начинает вызваться номер назначения.
        /// </summary>
        /// <param name="extensionId">Идентификатор добавочного типа "phone"</param>
        /// <param name="src">Массив номеров-источников вызова, например, список добавочных и/или мобильных номеров. Тут не могут быть указаны добавочные типа "очередь", "ivr"</param>
        /// <param name="dst">Номер назначения вызова</param>
        /// <param name="callerId">Имя звонящего</param>
        public TelphinCallbackResponse MakeCall(string extensionId, string[] src, string dst, string callerId)
        {
            var postData = JsonConvert.SerializeObject(new
            {
                src_num = src,
                dst_num = dst,
                caller_id_name = callerId
            });

            return MakeAuthRequest<TelphinCallbackResponse>(
                string.Format("{0}/api/ver1.0/extension/{1}/callback/", ServiceUrl, extensionId),
                postData, contentType: "application/json");
        }

        #region Extensions

        public List<TelphinExtension> GetExtensions()
        {
            return MakeAuthRequest<List<TelphinExtension>>(
                string.Format("{0}/api/ver1.0/client/@me/extension/?page=1&per_page=30&type=phone", ServiceUrl), // &type=queue
                method: "GET");
        }

        #region Events

        public List<TelphinEvent> GetEvents(string extensionId)
        {
            return MakeAuthRequest<List<TelphinEvent>>(
                string.Format("{0}/api/ver1.0/extension/{1}/event/", ServiceUrl, extensionId), 
                method: "GET");
        }

        public TelphinEvent AddEvent(string extensionId, TelphinEvent @event)
        {
            var postData = JsonConvert.SerializeObject(new
            {
                event_type = @event.EventType,
                method = @event.Method,
                url = @event.Url
            });
            return MakeAuthRequest<TelphinEvent>(
                string.Format("{0}/api/ver1.0/extension/{1}/event/", ServiceUrl, extensionId), 
                postData, contentType: "application/json");
        }

        public void DeleteEvents(string extensionId)
        {
            MakeAuthRequest<List<TelphinEvent>>(
                string.Format("{0}/api/ver1.0/extension/{1}/event/", ServiceUrl, extensionId), 
                method: "DELETE");
        }

        #endregion
        #endregion

        #region Token

        private string GetToken()
        {
            if (_appKey.IsNullOrEmpty() || _appSecret.IsNullOrEmpty())
                return string.Empty;

            var result = GetTokenFromFile();
            if (result == null || result.AccessToken.IsNullOrEmpty() || result.Expires < DateTime.Now)
            {
                result = GetTokenRemote();
            }

            return result != null ? result.AccessToken : string.Empty;
        }

        private TelphinTokenResponse GetTokenRemote()
        {
            var result = MakeRequest<TelphinTokenResponse>(
                string.Format("{0}/oauth/token", ServiceUrl),
                string.Format("client_id={0}&client_secret={1}&grant_type={2}", _appKey, _appSecret, "client_credentials"));
            WriteTokenToFile(result);

            return result;
        }

        private TelphinTokenResponse GetTokenFromFile()
        {
            var filePath = string.Format("{0}App_Data/telphinToken.txt", SettingsGeneral.AbsolutePath);
            if (!File.Exists(filePath))
                return null;
            string fileContent;
            using (var reader = new StreamReader(filePath))
            {
                fileContent = reader.ReadToEnd();
            }
            try
            {
                var result = JsonConvert.DeserializeObject<TelphinTokenResponse>(fileContent);
                if (result != null)
                {
                    var fi = new FileInfo(filePath);
                    result.Expires = fi.LastWriteTime.AddSeconds(result.ExpiresIn);
                    return result;
                }
            }
            catch (Exception e)
            {
                Debug.Log.Error(e);
                File.Delete(filePath);
            }
            return null;
        }

        private bool WriteTokenToFile(TelphinTokenResponse token)
        {
            var filePath = string.Format("{0}App_Data/telphinToken.txt", SettingsGeneral.AbsolutePath);
            if (token == null)
                return false;
            try
            {
                FileHelpers.CreateFile(filePath);
                using (var wr = new StreamWriter(filePath))
                {
                    wr.Write(JsonConvert.SerializeObject(token));
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }

            return true;
        }

        #endregion

        private T MakeAuthRequest<T>(string url, string postData = null, string method = "POST", string contentType = "application/x-www-form-urlencoded", bool retry = true) where T : class
        {
            var token = GetToken();
            if (token.IsNullOrEmpty())
            {
                Debug.Log.Warn("Can't get Telphin auth token");
                return null;
            }

            try
            {
                //AdvantShop.Statistic.CommonStatistic.WriteLog(
                //    string.Format("REQ [{0}]: \r\n url: {1} \r\n data: {2}", DateTime.Now.ToString("dd.MM HH:mm:ss"), url, postData));
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.ContentType = contentType;
                request.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;

                if (postData.IsNotEmpty())
                {
                    byte[] byteArray = Encoding.GetEncoding("windows-1251").GetBytes(postData);
                    request.ContentLength = byteArray.Length;
                    using (var dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();
                    }
                }
                string responseFromServer = "";
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var dataStream = response.GetResponseStream())
                    {
                        if (dataStream != null)
                            using (var reader = new StreamReader(dataStream))
                            {
                                responseFromServer = reader.ReadToEnd();
                            }
                    }
                }
                //AdvantShop.Statistic.CommonStatistic.WriteLog(
                //    string.Format("RESP {2} [{0}]: \r\n data: {1} \r\n", DateTime.Now.ToString("dd.MM HH:mm:ss"), responseFromServer, url));
                return JsonConvert.DeserializeObject<T>(responseFromServer);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null && response.StatusCode == HttpStatusCode.Unauthorized && retry)
                    {
                        GetTokenRemote();
                        return MakeAuthRequest<T>(url, postData, method, contentType, false);
                    }
                }
                //AdvantShop.Statistic.CommonStatistic.WriteLog(
                //    string.Format("ERROR WebException {2} [{0}]: \r\n data: {1} \r\n", DateTime.Now.ToString("dd.MM HH:mm:ss"), ex.Message, url));
                Debug.Log.Error(ex + " URL: " + url, ex);
            }
            catch (Exception ex)
            {
                //AdvantShop.Statistic.CommonStatistic.WriteLog(
                //    string.Format("ERROR Exception {2} [{0}]: \r\n data: {1} \r\n", DateTime.Now.ToString("dd.MM HH:mm:ss"), ex.Message, url));
                Debug.Log.Error(ex + " URL: " + url, ex);
            }
            return null;
        }

        private T MakeRequest<T>(string url, string postData, string method = "POST", string contentType = "application/x-www-form-urlencoded") where T : class
        {
            try
            {
                //AdvantShop.Statistic.CommonStatistic.WriteLog(
                //    string.Format("REQ [{0}]: \r\n url: {1} \r\n data: {2}", DateTime.Now.ToString("dd.MM HH:mm:ss"), url, postData));
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.ContentType = contentType;

                if (postData.IsNotEmpty())
                {
                    byte[] byteArray = Encoding.GetEncoding("windows-1251").GetBytes(postData);
                    request.ContentLength = byteArray.Length;
                    using (var dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();
                    }
                }
                string responseFromServer = "";
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var dataStream = response.GetResponseStream())
                    {
                        if (dataStream != null)
                            using (var reader = new StreamReader(dataStream))
                            {
                                responseFromServer = reader.ReadToEnd();
                            }
                    }
                }
                //AdvantShop.Statistic.CommonStatistic.WriteLog(
                //    string.Format("RESP [{0}]: \r\n data: {1} \r\n", DateTime.Now.ToString("dd.MM HH:mm:ss"), responseFromServer));
                return JsonConvert.DeserializeObject<T>(responseFromServer);
            }
            catch (Exception ex)
            {
                //AdvantShop.Statistic.CommonStatistic.WriteLog(
                //    string.Format("ERROR [{0}]: \r\n data: {1} \r\n", DateTime.Now.ToString("dd.MM HH:mm:ss"), ex.Message));
                Debug.Log.Error(ex.Message + " URL: " + url, ex);
            }
            return null;
        }
    }
}
