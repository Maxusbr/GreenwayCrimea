using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony.CallBack;

namespace AdvantShop.Core.Services.IPTelephony.Zadarma
{
    public class ZadarmaCallBack : CallBack.CallBack
    {
        private readonly string _from;

        public ZadarmaCallBack()
        {
            _from = SettingsTelephony.CallBackZadarmaPhone;
        }

        public override bool Enabled
        {
            get { return base.Enabled && SettingsTelephony.CurrentIPTelephonyOperatorType == EOperatorType.Zadarma; }
        }

        public override CallBackAnswer CreateCallBack(string phone)
        {
            if (_from.IsNullOrEmpty())
                return new CallBackAnswer(false, "Callback not configured");
            if (phone.IsNotEmpty() && phone.Length == 10)
                phone = "7" + phone;
            var zadarma = new Zadarma();
            var response = zadarma.CreateCallBack(_from, phone);
            if (response != null && response.Success)
                return new CallBackAnswer(true, string.Empty);
            return new CallBackAnswer(false, response != null ? response.Message : "service error");
        }
    }
}
