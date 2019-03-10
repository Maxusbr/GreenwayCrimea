using System.Net;
using System.Text;
using System.Web;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.SmsNotifications.Domain.SmsServices
{
    public class SmsLab : SMSSenderService
    {
        private readonly string _login;
        private readonly string _password;
        private readonly string _sender;

        public SmsLab()
        {
            _login = ModuleSettingsProvider.GetSettingValue<string>("SmslabRuLogin", SmsNotifications.ModuleId);
            _password = ModuleSettingsProvider.GetSettingValue<string>("SmslabRuPassword", SmsNotifications.ModuleId);
            _sender = ModuleSettingsProvider.GetSettingValue<string>("SmslabRuSender", SmsNotifications.ModuleId);
        }

        public override ESMSSenderService Type
        {
            get { return ESMSSenderService.SmslabRu; }
        }

        public override string SendSMS(long phone, string text)
        {
            string result;
            using (var client = new WebClient())
            {
                result =
                    Encoding.UTF8.GetString(
                        client.DownloadData(
                            string.Format("https://{0}:{1}/sendsms?user={2}&pwd={3}&sadr={4}&dadr={5}&text={6}",
                                "web.smslab.ru", "12778",
                                HttpUtility.UrlEncode(_login),
                                HttpUtility.UrlEncode(_password),
                                HttpUtility.UrlEncode(_sender),
                                phone,
                                HttpUtility.UrlEncode(text))));
            }
            return result;
        }
    }
}
