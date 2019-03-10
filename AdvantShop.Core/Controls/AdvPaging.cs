//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Controls
{
    public sealed class AdvPaging : WebControl
    {
        public int CurrentPage { set; get; }
        public int TotalPages { set; get; }
        public int VisiblePages { set; get; }
        public int BlockPages { set; get; }
        public bool DisplayArrows { set; get; }
        public bool DisplayPrevNext { set; get; }
        public bool DisplayShowAll { set; get; }

        public AdvPaging()
        {
            EnableViewState = false;
            VisiblePages = 5;
            BlockPages = 2;
            DisplayShowAll = false;
            DisplayPrevNext = true;
            DisplayArrows = true;
            CurrentPage = 1;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (string.IsNullOrWhiteSpace(Page.Request.QueryString["page"])) return;
            string curPage = Page.Request.QueryString["page"].Split('#')[0];
            CurrentPage = curPage.TryParseInt(-1);
            if (CurrentPage == 1)
                Page.Response.RedirectPermanent(Page.Request.RawUrl.Split('?')[0] + QueryHelper.RemoveQueryParam(Page.Request.RawUrl.Split('?')[1], "page"));
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderContents(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (TotalPages < 2 && CurrentPage != 0)
                return;

            string query = string.Empty;
            string rowUrl = Page.Request.RawUrl.Split('?').FirstOrDefault();

            if (Page.Request.RawUrl.Split('?').Count() > 1)
            {
                query = "?" + Page.Request.RawUrl.Split('?').LastOrDefault();
            }


            writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-number");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);


            if (CurrentPage != 0)
            {
                //стрелка влево
                if (DisplayArrows && CurrentPage > 1)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "key");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write("Ctrl + &larr;");
                    writer.RenderEndTag();
                }


                //Предыдущая страница
                if (DisplayPrevNext && CurrentPage > 1)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, rowUrl + (CurrentPage == 2
                                                                                    ? QueryHelper.RemoveQueryParam(query, "page")
                                                                                    : QueryHelper.ChangeQueryParam(query, "page", (CurrentPage - 1).ToString())));
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "paging-prev");
                    writer.AddAttribute(HtmlTextWriterAttribute.Rel, "prev");
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(LocalizationService.GetResource("Core.Paging.Previous"));
                    writer.RenderEndTag();
                }

                // начальный блок ссылок
                if (CurrentPage - (float)VisiblePages / 2 >= BlockPages)
                {
                    for (int i = 1; i <= BlockPages; i++)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, rowUrl + (i == 1
                                                                                        ? QueryHelper.RemoveQueryParam(query, "page")
                                                                                        : QueryHelper.ChangeQueryParam(query, "page", i.ToString())));
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(i.ToString());
                        writer.RenderEndTag();
                    }
                }

                int startPage = CurrentPage - (float)VisiblePages / 2 > BlockPages ? CurrentPage - VisiblePages / 2 : 1;
                //int endPage = startPage + VisiblePages - 1 < TotalPages ? startPage + VisiblePages - 1 : TotalPages;
                int endPage = CurrentPage + VisiblePages / 2 < TotalPages - BlockPages ? CurrentPage + VisiblePages / 2 : TotalPages;


                if (startPage > BlockPages + 1)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write("...");
                    writer.RenderEndTag();
                }

                for (int i = startPage; i <= endPage; i++)
                {
                    if (i == CurrentPage)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, rowUrl + (i == 1
                                                                                        ? QueryHelper.RemoveQueryParam(query, "page")
                                                                                        : QueryHelper.ChangeQueryParam(query, "page", i.ToString())));
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                    }
                    writer.Write(i.ToString());
                    writer.RenderEndTag();
                }

                if (endPage + BlockPages < TotalPages)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write("...");
                    writer.RenderEndTag();
                }


                // конечный блок ссылок

                if (CurrentPage + (float)VisiblePages / 2 <= TotalPages - BlockPages)
                {

                    for (int i = TotalPages - BlockPages + 1; i <= TotalPages; i++)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href,
                                            rowUrl + QueryHelper.ChangeQueryParam(query, "page", i.ToString()));
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(i.ToString());
                        writer.RenderEndTag();
                    }
                }


                //Следующая страница
                if (DisplayPrevNext && CurrentPage < TotalPages)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href,
                                        rowUrl +
                                        QueryHelper.ChangeQueryParam(query, "page", (CurrentPage + 1).ToString()));
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "paging-next");
                    writer.AddAttribute(HtmlTextWriterAttribute.Rel, "next");
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(LocalizationService.GetResource("Core.Paging.Next"));
                    writer.RenderEndTag();
                }

                //стрелка вправо
                if (DisplayArrows && CurrentPage < TotalPages)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "key");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write("Ctrl + &rarr;");
                    writer.RenderEndTag();
                }

            }
            // Показать все
            if (DisplayShowAll)
            {
                if (CurrentPage == 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-all");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, (rowUrl + QueryHelper.RemoveQueryParam(query, "page")).TrimEnd('?'));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(LocalizationService.GetResource("Core.Paging.ShowPages"));
                    writer.RenderEndTag();
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-all");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, rowUrl + QueryHelper.ChangeQueryParam(query, "page", "0"));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(LocalizationService.GetResource("Core.Paging.ShowAll"));
                    writer.RenderEndTag();
                }
            }

            writer.RenderEndTag();//div class="page-number"
        }

    }
}