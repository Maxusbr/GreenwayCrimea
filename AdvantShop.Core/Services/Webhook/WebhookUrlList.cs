using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AdvantShop.Core.Services.Crm.BusinessProcesses;

namespace AdvantShop.Core.Services.Webhook
{
    public abstract class WebhookUrl
    {
        public string Url { get; set; }
    }

    public abstract class WebhookUrlList<TWebhookUrl> where TWebhookUrl : WebhookUrl
    {
        public WebhookUrlList()
        {
            UrlList = new List<TWebhookUrl>().AsQueryable();
        }

        public virtual EWebhookType WebhookType { get { return EWebhookType.None; } }
        public IQueryable<TWebhookUrl> UrlList { get; set; }

        public List<string> GetUrlsFiltered(Expression<Func<TWebhookUrl, bool>> predicate)
        {
            return UrlList.Where(predicate).Select(x => x.Url).ToList();
        }
    }


    public class BizProcessWebhookUrl : WebhookUrl
    {
        public EBizProcessEventType EventType { get; set; }
    }

    public class BizProcessWebhookUrlList : WebhookUrlList<BizProcessWebhookUrl>
    {
        public override EWebhookType WebhookType { get { return EWebhookType.BizProcess; } }
    }
}
