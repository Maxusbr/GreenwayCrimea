using System;
using System.ServiceModel;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;

namespace AdvantShop.Module.SmsNotifications.Domain.SmsServices
{
    public class Sms4B : SMSSenderService
    {
        private readonly string _login;
        private readonly string _password;
        private readonly string _sender;

        public override ESMSSenderService Type
        {
            get { return ESMSSenderService.WwwSms4BRu; }
        }

        public Sms4B()
        {
            _login = ModuleSettingsProvider.GetSettingValue<string>("WwwSms4BRuLogin", SmsNotifications.ModuleId);
            _password = ModuleSettingsProvider.GetSettingValue<string>("WwwSms4BRuPassword", SmsNotifications.ModuleId);
            _sender = ModuleSettingsProvider.GetSettingValue<string>("WwwSms4BRuSender", SmsNotifications.ModuleId);
        }

        public override string SendSMS(long phone, string text)
        {
            string result;

            var basicAuthBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            var endPoint = new EndpointAddress("https://sms4b.ru/ws/sms.asmx");

            using (var send = new sms4b.WSSMSoapClient(basicAuthBinding, endPoint))
            {
                result = send.SendSMS(_login, _password, _sender, phone, text);

                if (result == "-1" || result == "0")
                {
                    Debug.Log.Error(String.Format("Не удалось отправить смс:{0} на номер {1}", text, phone));
                }
            }
            return result;
        }
    }
}
