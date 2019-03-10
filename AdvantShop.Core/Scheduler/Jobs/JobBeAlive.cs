//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Net;
using Quartz;
using System.Collections.Generic;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Catalog;
using AdvantShop.CMS;

namespace AdvantShop.Core.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class JobBeAlive : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            System.Threading.Thread.Sleep(new System.Random().Next(5 * 60 * 1000));

            var urls = new List<string>
            {
                UrlService.GetUrl(),
                UrlService.GetUrl("cart"), 
                UrlService.GetUrl("checkout")
            };

            var category = CategoryService.GetFirstCategory();
            if (category != null)
            {
                urls.Add(UrlService.GetUrl(UrlService.GetLink(ParamType.Category, category.UrlPath, category.ID)));
            }

            var product = ProductService.GetFirstProduct();
            if (product != null)
            {
                urls.Add(UrlService.GetUrl(UrlService.GetLink(ParamType.Product, product.UrlPath, product.ProductId)));
            }

            var page = StaticPageService.GetFirstPage();
            if (page != null)
            {
                urls.Add(UrlService.GetUrl(UrlService.GetLink(ParamType.StaticPage, page.UrlPath, page.ID)));
            }

            foreach (var url in urls)
            {

                try
                {
                    using (var wc = new WebClient())
                    {
                        string response = wc.DownloadString(url);
                    }
                }
                catch
                {
                    // empty catch: we no need to log this error
                    break;
                }
            }
        }
    }
}