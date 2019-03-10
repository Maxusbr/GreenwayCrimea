using AdvantShop.Configuration;
using AdvantShop.Core.Services.Loging.Calls;
using AdvantShop.Core.Services.Loging.Emails;
using AdvantShop.Core.Services.Loging.Events;
using AdvantShop.Core.Services.Loging.Smses;
using AdvantShop.Core.Services.Loging.TrafficSource;
using AdvantShop.Saas;

namespace AdvantShop.Core.Services.Loging
{
    public class LogingManager
    {
        public static IEmailLoger GetEmailLoger()
        {
            if (!SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCustomerLog)
                return new ActivityEmailLoger();

            return new ActivityEmailNullLoger();
        }

        public static ICallLoger GetCallLoger()
        {
            if (!SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCustomerLog)
                return new ActivityCallLoger();

            return new ActivityCallNullLoger();
        }

        public static ISmsLoger GetSmsLoger()
        {
            if (!SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCustomerLog)
                return new ActivitySmsLoger();

            return new ActivitySmsNullLoger();
        }

        public static IEventLoger GetEventLoger()
        {
            if (!SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCustomerLog)
                return new ActivityEventLoger();

            return new ActivityEventNullLoger();
        }

        public static ITrafficSourceLoger GetTrafficSourceLoger()
        {
            if (!SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCustomerLog)
                return new ActivityTrafficSourceLoger();

            return new ActivityTrafficSourceNullLoger();
        }
    }
}