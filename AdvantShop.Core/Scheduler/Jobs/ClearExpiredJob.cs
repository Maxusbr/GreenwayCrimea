//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.CMS;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Security;
using AdvantShop.Statistic;
using Quartz;
using AdvantShop.Diagnostics;
using AdvantShop.SEO;

namespace AdvantShop.Core.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class ClearExpiredJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            System.Threading.Thread.Sleep(new System.Random().Next(20 * 60 * 1000));

            // don't forget about abandoned carts 
            try
            {
                ShoppingCartService.DeleteExpiredShoppingCartItems(DateTime.Today.AddMonths(-3));
                OrderConfirmationService.DeleteExpired();
                InternalServices.DeleteExpiredAppRestartLogData();
                Secure.DeleteExpiredAuthorizeLog();
                ClientCodeService.DeleteExpired(DateTime.Now.AddDays(-7));
                AdminNotificationService.ClearExpiredAdminNotifications();
                Error404Service.DeleteExpired();
                RecentlyViewService.DeleteExpired();
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }
            context.WriteLastRun();
        }
    }
}