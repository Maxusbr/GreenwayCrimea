using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.SEO;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class AdminErrorLog404FilterModel : BaseFilterModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string UrlReferer { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string RedirectTo { get; set; }
    }

    public class PageError404 : Error404
    {
        public string RedirectTo { get; set; }
    }
}