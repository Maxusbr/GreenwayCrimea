//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Web;
using AdvantShop.Core.Caching;

namespace AdvantShop.Security
{
    public static class ActionValidator
    {
        private const int Duration = 5; // 5-минутный период

        public enum ActionTypeEnum
        {
            FirstVisit = 100, // Самый значимый параметр, подбирать его значение надо осторожно. 
            ReVisit = 1000,  // Повторные посещения особо не ограничиваем
            Postback = 5000,    // Это тем более не проблема
            AddNewWidget = 100,
            AddNewPage = 100,
        }

        private class HitInfo
        {
            public int Hits;
        }

        public static bool IsValid(ActionTypeEnum actionType)
        {
            HttpContext context = HttpContext.Current;
            if (context.Request.Browser.Crawler) return false;

            string key = actionType + context.Request.UserHostAddress;
            //var hit = (HitInfo)(context.Cache[key] ?? new HitInfo());
            var hit = CacheManager.Get<HitInfo>(key) ?? new HitInfo();

            if (hit.Hits > (int)actionType) return false;
            hit.Hits++;

            if (hit.Hits == 1)
                CacheManager.Insert(key, hit, Duration);
            //context.Cache.Add(key, hit, null, DateTime.Now.AddMinutes(Duration),System.Web.Caching.Cache.NoSlidingExpiration,System.Web.Caching.CacheItemPriority.Normal, null);
            return true;
        }
    }
}