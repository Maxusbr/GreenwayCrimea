using System;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.IPTelephony.CallBack;

namespace AdvantShop.Core.Services.IPTelephony.Telphin
{
    public class TelphinCallBack : CallBack.CallBack
    {
        private readonly string _extensionId;
        private readonly string[] _localNumbers;

        public TelphinCallBack()
        {
            _extensionId = SettingsTelephony.CallBackTelphinExtensionId;
            if (!string.IsNullOrEmpty(SettingsTelephony.CallBackTelphinLocalNumber))
            {
                _localNumbers = SettingsTelephony.CallBackTelphinLocalNumber.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled && SettingsTelephony.CurrentIPTelephonyOperatorType == EOperatorType.Telphin; }
        }

        public override CallBackAnswer CreateCallBack(string phone)
        {
            if (_extensionId.IsNullOrEmpty() || !_localNumbers.Any())
                return new CallBackAnswer(false, "Callback not configured");
            if (phone.IsNotEmpty() && phone.Length == 10)
                phone = "+7" + phone;
            var telphin = new Telphin();
            var response = telphin.MakeCall(_extensionId, _localNumbers, phone, string.Format("Web Call <{0}>", phone));
            if (response != null)
                return new CallBackAnswer(true, string.Empty);
            return new CallBackAnswer(false, "service error");
        }
    }
}
