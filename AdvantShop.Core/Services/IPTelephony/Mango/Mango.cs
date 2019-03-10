using System;
using System.Collections.Generic;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Services.IPTelephony.Mango
{
    public class Mango : IPTelephonyOperator
    {
        private const string ServiceUrl = "https://app.mango-office.ru/vpbx";

        private string _apiKey;
        private string _secretKey;
        private string _apiUrl;

        public override EOperatorType Type
        {
            get { return EOperatorType.Mango; }
        }

        public Mango()
        {
            _apiKey = SettingsTelephony.MangoApiKey;
            _secretKey = SettingsTelephony.MangoSecretKey;
            _apiUrl = SettingsTelephony.MangoApiUrl;
        }

        public override CallBack.CallBack CallBack
        {
            get { return new MangoCallBack(); }
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

            // /queries/recording/link/[recording_id]/[action]/[vpbx_api_key]/[timestamp]/[sign]
            // - recording_id: идентификатор записи разговора.
            // - action: разрешенные значения download, play
            // - vpbx_api_key: ключ API
            // - timestamp: timestamp UTC, время до которого действует ссылка
            // - sign: подпись, рассчитанная по формуле sign = sha256(vpbx_api_key + timestamp + recording_id + vpbx_api_salt)
            var timestamp = DateTime.Now.AddHours(1).ToUnixTime();
            return string.Format("{0}/queries/recording/link/{1}/{2}/{3}/{4}/{5}",
                _apiUrl.IsNullOrEmpty() ? ServiceUrl : _apiUrl.TrimEnd('/'), 
                call.RecordLink, "play", _apiKey, timestamp,
                new List<string>
                {
                    _apiKey,
                    timestamp.ToString(),
                    call.RecordLink,
                    _secretKey
                }.AggregateString().Sha256());
        }
    }
}
