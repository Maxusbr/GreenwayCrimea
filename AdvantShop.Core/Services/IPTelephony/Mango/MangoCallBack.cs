using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using Newtonsoft.Json;
using AdvantShop.Core.Services.IPTelephony.CallBack;

namespace AdvantShop.Core.Services.IPTelephony.Mango
{
    public class MangoCallBack : CallBack.CallBack
    {
        private readonly string _extension;
        private readonly string _apiKey;
        private readonly string _secretKey;
        private readonly string _apiUrl;

        public MangoCallBack()
        {
            _extension = SettingsTelephony.CallBackMangoExtension;
            _apiKey = SettingsTelephony.MangoApiKey;
            _secretKey = SettingsTelephony.MangoSecretKey;
            _apiUrl = SettingsTelephony.MangoApiUrl;
        }

        public override bool Enabled
        {
            get { return base.Enabled && SettingsTelephony.CurrentIPTelephonyOperatorType == EOperatorType.Mango; }
        }

        public override CallBackAnswer CreateCallBack(string phone)
        {
            if (phone.IsNotEmpty() && phone.Length == 10)
                phone = "7" + phone;
            return CreateCallBack("commands/callback", GetParams(phone));
        }

        private string GetParams(string phone)
        {
            var json = JsonConvert.SerializeObject(new
            {
                // по command_id можно  в дальнейшем отслеживать звонок
                command_id = "cbck" + DateTime.Now.ToString("yyyyMMddHHmmssFFF"),
                from = new
                {
                    extension = _extension
                },
                to_number = phone
            });
            var sign = (_apiKey + json + _secretKey).Sha256();
                    
            var sb = new StringBuilder();
            sb.AppendFormat("vpbx_api_key={0}", _apiKey);
            sb.AppendFormat("&sign={0}", sign);
            sb.AppendFormat("&json={0}", json);

            return sb.ToString();
        }

        private CallBackAnswer CreateCallBack(string path, string postData)
        {
            ServicePointManager.Expect100Continue = false;
            var request = WebRequest.Create(_apiUrl.TrimEnd('/') + "/" + path);
            request.Method = "POST";

            byte[] byteArray = Encoding.GetEncoding("windows-1251").GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }
            using (var response = request.GetResponse())
            {
                using (var dataStream = response.GetResponseStream())
                {
                    if (dataStream == null) 
                        return new CallBackAnswer(false, string.Empty);

                    using (var reader = new StreamReader(dataStream))
                    {
                        string responseFromServer = reader.ReadToEnd();
                        if (responseFromServer.IsNotEmpty())
                        {
                            var res = JsonConvert.DeserializeObject<MangoCallBackAnswer>(responseFromServer);
                            if (res != null && res.result != 1000)
                            {
                                if (res.result >= 2000 && res.result < 3000)
                                    return new CallBackAnswer(false, "Service temporarily unavailable");
                                if (res.result == 3100)
                                    return new CallBackAnswer(false, "Wrong data");
                                if (res.result == 4001)
                                    return new CallBackAnswer(false, "Wrong command");
                                if (res.result >= 5000)
                                    return new CallBackAnswer(false, "Server error");
                                return new CallBackAnswer(false, "Unidentified error");
                            }
                        }
                        return new CallBackAnswer(true, string.Empty);
                    }
                }
            }
        }
    }
}
