using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Helpers;
using HtmlHelper = System.Web.Mvc.HtmlHelper;

namespace AdvantShop.Web.Infrastructure.Extensions
{
    public static class PagingExtensions
    {
        public static MvcHtmlString Paging(this HtmlHelper helper, Pager pager, string className = null)
        {
            if (pager.TotalPages < 2 && pager.CurrentPage != 0)
                return new MvcHtmlString("");

            var urlArr = HttpContext.Current.Request.RawUrl.Split('?');
            var rawUrl = urlArr.FirstOrDefault();
            var query = urlArr.Length > 1 ? "?" + urlArr.LastOrDefault() : string.Empty;

            var sb = new StringBuilder();

            sb.AppendFormat("<div class=\"pagenumberer{0}\">\n", className != null ? " " + className : "");

            if (pager.CurrentPage != 0)
            {
                //стрелка влево
                if (pager.DisplayArrows && pager.CurrentPage > 1)
                {
                    sb.Append("<span class=\"key\">Ctrl + &larr;</span>");
                }

                //Предыдущая страница
                if (pager.DisplayPrevNext && pager.CurrentPage > 1)
                {
                    sb.AppendFormat(
                        "<a href=\"{0}\" class=\"pagenumberer-prev\"><span class=\"pagenumberer-prev-text\">{1}</span><span class=\"icon-left-open-after\"></span></a>",
                        rawUrl + (pager.CurrentPage == 2
                            ? QueryHelper.RemoveQueryParam(query, "page")
                            : QueryHelper.ChangeQueryParam(query, "page", (pager.CurrentPage - 1).ToString())),
                        LocalizationService.GetResource("Infrasturcture.Catalog.PagingPrevious"));
                }

                // начальный блок ссылок
                if (pager.CurrentPage - (float)pager.VisiblePages / 2 >= pager.BlockPages)
                {
                    for (var i = 1; i <= pager.BlockPages; i++)
                    {
                        sb.AppendFormat("<a href=\"{0}\" class=\"pagenumberer-item pagenumberer-item-link\">{1}</a>",
                            rawUrl + (i == 1
                                ? QueryHelper.RemoveQueryParam(query, "page")
                                : QueryHelper.ChangeQueryParam(query, "page", i.ToString())),
                            i);
                    }
                }

                var startPage = pager.CurrentPage - (float)pager.VisiblePages / 2 > pager.BlockPages ? pager.CurrentPage - pager.VisiblePages / 2 : 1;
                var endPage = pager.CurrentPage + pager.VisiblePages / 2 < pager.TotalPages - pager.BlockPages ? pager.CurrentPage + pager.VisiblePages / 2 : pager.TotalPages;

                if (startPage > pager.BlockPages + 1)
                {
                    sb.Append("<span>...</span>");
                }

                for (var i = startPage; i <= endPage; i++)
                {
                    if (i == pager.CurrentPage)
                    {
                        sb.AppendFormat("<span class=\"pagenumberer-item pagenumberer-selected cs-br-1\">{0}</span>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<a href=\"{0}\" class=\"pagenumberer-item pagenumberer-item-link\">{1}</a>",
                            rawUrl + (i == 1
                                ? QueryHelper.RemoveQueryParam(query, "page")
                                : QueryHelper.ChangeQueryParam(query, "page", i.ToString())),
                            i);
                    }
                }

                if (endPage + pager.BlockPages < pager.TotalPages)
                {
                    sb.Append("<span>...</span>");
                }

                // конечный блок ссылок
                if (pager.CurrentPage + (float)pager.VisiblePages / 2 <= pager.TotalPages - pager.BlockPages)
                {

                    for (int i = pager.TotalPages - pager.BlockPages + 1; i <= pager.TotalPages; i++)
                    {
                        sb.AppendFormat("<a href=\"{0}\" class=\"pagenumberer-item pagenumberer-item-link\">{1}</a>",
                            rawUrl + QueryHelper.ChangeQueryParam(query, "page", i.ToString()),
                            i);
                    }
                }

                //Следующая страница
                if (pager.DisplayPrevNext && pager.CurrentPage < pager.TotalPages)
                {
                    sb.AppendFormat("<a href=\"{0}\" class=\"pagenumberer-next\"><span class=\"pagenumberer-next-text\">{1}</span><span class=\"icon-right-open-after\"></span></a>",
                            rawUrl + QueryHelper.ChangeQueryParam(query, "page", (pager.CurrentPage + 1).ToString()),
                            LocalizationService.GetResource("Infrasturcture.Catalog.PagingNext"));
                }

                //стрелка вправо
                if (pager.DisplayArrows && pager.CurrentPage < pager.TotalPages)
                {
                    sb.Append("<span class=\"key\">Ctrl + &rarr;</span>");
                }
            }

            // Показать все
            if (pager.DisplayShowAll)
            {
                if (pager.CurrentPage == 0)
                {
                    sb.AppendFormat("<a class=\"page-all\" href=\"{0}\">{1}</a>",
                        (rawUrl + QueryHelper.RemoveQueryParam(query, "page")).TrimEnd('?'),
                        LocalizationService.GetResource("Infrasturcture.Catalog.PagingShowPages"));
                }
                else
                {
                    sb.AppendFormat("<a class=\"page-all\" href=\"{0}\">{1}</a>",
                        rawUrl + QueryHelper.ChangeQueryParam(query, "page", "0"),
                        LocalizationService.GetResource("Infrasturcture.Catalog.PagingShowAll"));
                }
            }

            sb.AppendLine("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}