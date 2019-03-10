using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Web.Admin.Handlers.AdminNotifications;
using AdvantShop.Web.Admin.Handlers.IPTelephony;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Web.Admin.Controllers.Calls
{
    public class BaseCallController : BaseController
    {
        protected static void SendIncomingCallNotify(Call call)
        {
            var cacheName = string.Format("Telephony_NotifySent_{0}", call.CallId);
            if (CacheManager.Get<bool>(cacheName))
                return;

            var handler = new IPTelephonyHandler(call);
            var notificationsHandler = new AdminNotificationsHandler();
            foreach (var notice in handler.GetWebNotifications())
            {
                notificationsHandler.NotifyAllCustomers(notice);
            }
            foreach (var notice in handler.GetToasterNotifications())
            {
                notificationsHandler.NotifyByToaster(notice);
            }

            CacheManager.Insert(cacheName, true, 0.1);  // 6 seconds
        }

        protected static void SendCallEndedNotify(Call call)
        {
            var notificationsHandler = new AdminNotificationsHandler();
            notificationsHandler.CloseToaster(call.CallId);
        }

        protected void ProcessMissedCall(Call call)
        {
            var cacheName = string.Format("Telephony_CallProcessed_{0}", call.CallId);
            if (CacheManager.Get<bool>(cacheName))
                return;

            BizProcessExecuter.CallMissed(call);
            CacheManager.Insert(cacheName, true, 0.1);  // 6 seconds
        }
    }
}