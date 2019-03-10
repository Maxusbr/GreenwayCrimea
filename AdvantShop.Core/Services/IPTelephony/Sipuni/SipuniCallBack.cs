using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using Newtonsoft.Json;
using AdvantShop.Core.Services.IPTelephony.CallBack;

namespace AdvantShop.Core.Services.IPTelephony.Sipuni
{
    public class SipuniCallBack : CallBack.CallBack
    {
        private const string UrlCallNumber = "http://sipuni.com/api/callback/call_number";
        private const string UrlCallTree = "http://sipuni.com/api/callback/call_tree";

        /// <summary>
        /// номер аккаунта
        /// </summary>
        private readonly string _user;
        /// <summary>
        /// Внутренний номер
        /// </summary>
        private readonly string _sipnumber;
        /// <summary>
        /// Схема
        /// </summary>
        private readonly string _tree;
        /// <summary>
        /// Направление вызова:
        /// 0 - звонок идет сначала на внутренний номер 
        /// 1 - звонок идет сначала на номер, указанный в параметре phone
        /// </summary>
        private readonly bool _reverse;
        /// <summary>
        /// Скрывать городской номер:
        /// 0 - не скрывать городской номер 
        /// 1 - скрывать городской номер
        /// </summary>
        private readonly bool _antiaon;
        /// <summary>
        /// ключ интеграции
        /// </summary>
        private readonly string _apiKey;

        public SipuniCallBack()
        {
            _user = SettingsTelephony.CallBackSipuniAccount;
            _sipnumber = SettingsTelephony.CallBackSipuniShortNumber;
            _tree = SettingsTelephony.CallBackSipuniTree;
            _reverse = false;
            _antiaon = false;
            _apiKey = SettingsTelephony.SipuniApiKey;
        }

        public override bool Enabled
        {
            get { return base.Enabled && SettingsTelephony.CurrentIPTelephonyOperatorType == EOperatorType.Sipuni; }
        }

        public override CallBackAnswer CreateCallBack(string phone)
        {
            if (phone.IsNotEmpty() && phone.Length == 10)
                phone = "7" + phone;
            return SettingsTelephony.CallBackSipuniType == 0 
                ? CreateCallBack(UrlCallNumber, GetParamsCallNumber(phone)) 
                : CreateCallBack(UrlCallTree, GetParamsCallTree(phone));
        }

        private string GetParamsCallTree(string phone)
        {
            var hashParams = new List<string>
            {
                phone,
                _sipnumber,
                _tree,
                _user,
                _apiKey
            };
            var hash = hashParams.Where(s => s.IsNotEmpty()).AggregateString('+').Md5(false);

            var sb = new StringBuilder();
            sb.AppendFormat("phone={0}", phone);
            sb.AppendFormat("&sipnumber={0}", _sipnumber);
            sb.AppendFormat("&tree={0}", _tree);
            sb.AppendFormat("&user={0}", _user);
            sb.AppendFormat("&hash={0}", hash);

            return sb.ToString();
        }

        private string GetParamsCallNumber(string phone)
        {
            var hashParams = new List<string>
            {
                _antiaon ? "1" : "0",
                phone,
                _reverse ? "1" : "0",
                _sipnumber,
                _user,
                _apiKey
            };
            var hash = hashParams.Where(s => s.IsNotEmpty()).AggregateString('+').Md5(false);
                    
            var sb = new StringBuilder();
            sb.AppendFormat("antiaon={0}", _antiaon ? "1" : "0");
            sb.AppendFormat("&phone={0}", phone);
            sb.AppendFormat("&reverse={0}", _reverse ? "1" : "0");
            sb.AppendFormat("&sipnumber={0}", _sipnumber);
            sb.AppendFormat("&user={0}", _user);
            sb.AppendFormat("&hash={0}", hash);

            return sb.ToString();
        }

        private CallBackAnswer CreateCallBack(string url, string postData)
        {
            ServicePointManager.Expect100Continue = false;
            var request = WebRequest.Create(url);
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
                            return JsonConvert.DeserializeObject<CallBackAnswer>(responseFromServer);
                        return new CallBackAnswer(true, string.Empty);
                    }
                }
            }
        }
    }
}
