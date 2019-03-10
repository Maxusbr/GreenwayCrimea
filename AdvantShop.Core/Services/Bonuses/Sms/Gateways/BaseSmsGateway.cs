using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

namespace AdvantShop.Core.Services.Bonuses.Sms.Gateways
{
    public abstract class BaseSmsGateway : ISmsGateway
    {
        public virtual SmsProviderType Type { get { return SmsProviderType.None; } }

        protected BaseSmsGateway() { }

        public abstract string Send(long destination, string message, string from);


        protected string GetPhonePrepared(long phone)
        {
            var result = phone.ToString(CultureInfo.InvariantCulture);
            if (result.StartsWith("8"))
                result ='7' + result.Remove(0, 1);
            return result;
        }

        protected string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", key, value))
                .ToArray();
            return string.Join("&", array);
        }
    }
}
