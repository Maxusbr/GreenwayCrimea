using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Services.IPTelephony.Sipuni
{
    public class Sipuni : IPTelephonyOperator
    {
        private string _apiKey;

        public override EOperatorType Type
        {
            get { return EOperatorType.Sipuni; }
        }

        public Sipuni()
        {
            _apiKey = SettingsTelephony.SipuniApiKey;
        }

        public Sipuni(string apiKey)
        {
            _apiKey = apiKey;
        }

        public override CallBack.CallBack CallBack
        {
            get { return new SipuniCallBack(); }
        }

        public override bool WebPhoneActive
        {
            get { return SettingsTelephony.SipuniWebPhoneEnabled; }
        }

        public override string RenderBottomScript()
        {
            var result = string.Empty;
            if (WebPhoneActive)
                result += SettingsTelephony.SipuniWebPhoneWidget +
                    "<script src='//sipuni.com/idealats/js/vendor/webphone2/build/bundle.js'></script>";
                    //"<script src='../admin/js/telephony/sipuni/bundle.js'></script>";
            return result;
        }

        public override string RenderIntoHead()
        {
            return WebPhoneActive 
                ? "<link href='../admin/css/telephony/sipuni/main.css' rel='stylesheet' type='text/css' />"
                : string.Empty;
        }

        public override string GetRecordLink(int callId)
        {
            var call = CallService.GetCall(callId);
            if (call == null || call.RecordLink.IsNullOrEmpty())
                return string.Empty;

            return call.RecordLink;
        }
    }
}
