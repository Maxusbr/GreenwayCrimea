using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.IPTelephony.Zadarma
{
    public class Zadarma : IPTelephonyOperator
    {
        private const string ServiceUrl = "https://api.zadarma.com";

        private string _key;
        private string _secret;

        public override EOperatorType Type
        {
            get { return EOperatorType.Zadarma; }
        }

        public Zadarma()
        {
            _key = SettingsTelephony.ZadarmaKey;
            _secret = SettingsTelephony.ZadarmaSecret;
        }

        public override CallBack.CallBack CallBack
        {
            get { return new ZadarmaCallBack(); }
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
            if (call == null || call.RecordLink.IsNullOrEmpty())
                return string.Empty;

            var @params = new Dictionary<string, string>();
            if (call.RecordLink.IsNotEmpty())
                @params.Add("call_id", call.RecordLink);
            else
                @params.Add("pbx_call_id", call.CallId);

            var result = MakeRequest<ZadarmaRecordResponse>("/v1/pbx/record/request/", @params);

            if (result == null)
                return string.Empty;
            if (result.Link.IsNullOrEmpty())
                return result.Links.FirstOrDefault();
            return result.Link;
        }

        public ZadarmaCallbackResponse CreateCallBack(string from, string to)
        {
            var @params = new Dictionary<string, string>()
            {
                { "from", from },
                { "to", to }
            };
            return MakeRequest<ZadarmaCallbackResponse>("/v1/request/callback/", @params);
        }

        private T MakeRequest<T>(string queryUrl, Dictionary<string, string> @params, string method = "GET", string contentType = "application/x-www-form-urlencoded") where T : ZadarmaResponse
        {
            if (_key.IsNullOrEmpty() || _secret.IsNullOrEmpty())
                return null;

            var queryParams = @params.OrderBy(key => key.Key).Select(pair => string.Format("{0}={1}", pair.Key, pair.Value)).AggregateString("&");
            var queryParamsMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(queryParams))).Replace("-", "").ToLower();
            var sign = StringHelper.EncodeTo64(
                SecurityHelper.EncodeWithHmacSha1(StringHelper.AggregateStrings("", queryUrl, queryParams, queryParamsMd5), _secret));

            var url = ServiceUrl + queryUrl;
            if (method == "GET" && queryParams.IsNotEmpty())
                url += "?" + queryParams;

            try
            {
                //AdvantShop.Statistic.CommonStatistic.WriteLog(
                //    string.Format("REQ [{0}]: \r\n url: {1} \r\n data: {2}", DateTime.Now.ToString("dd.MM HH:mm:ss"), url, postData));
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.ContentType = contentType;
                request.Headers[HttpRequestHeader.Authorization] = _key + ":" + sign;

                if (method != "GET" && queryParams.IsNotEmpty())
                {
                    byte[] byteArray = Encoding.GetEncoding("windows-1251").GetBytes(queryParams);
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
