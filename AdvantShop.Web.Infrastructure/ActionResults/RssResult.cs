using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace AdvantShop.Web.Infrastructure.ActionResults
{
    public class RssResult : FileResult
    {
        private readonly SyndicationFeed _feed;
        public RssResult(SyndicationFeed feed)
            : base("application/rss+xml")
        {
            _feed = feed;
        }

        public RssResult(string title, IEnumerable<SyndicationItem> feedItems)
            : base("application/rss+xml")
        {
            _feed = new SyndicationFeed(title, title, HttpContext.Current.Request.Url) { Items = feedItems };
            // self link (Required) - The URL for the syndication feed.
            _feed.Links.Add(SyndicationLink.CreateSelfLink(new System.Uri(HttpContext.Current.Request.Url.ToString())));
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            using (var writer = XmlWriter.Create(response.OutputStream))
            {
                _feed.GetRss20Formatter().WriteTo(writer);
            }
        }
    }
}
