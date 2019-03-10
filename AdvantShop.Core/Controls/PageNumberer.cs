//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Controls
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class PageNumberer : WebControl, IPostBackEventHandler
    {
        private EventHandler _selectedPageChangedEvent;
        private int _count = - 1;
        private int _selectedPage;
        private int _displayedPages;

        public int CurrentPageIndex
        {
            get
            {
                if (_selectedPage == 0)
                {
                    object o = ViewState["SelectedPage"];
                    _selectedPage = (o != null) ? (SQLDataHelper.GetInt(o)) : 1;
                }
                return _selectedPage;
            }
            set
            {
                if (value > 0)
                {
                    ViewState["SelectedPage"] = value;
                    _selectedPage = value;
                }
            }
        }

        public bool UseHref { get; set; }

        public bool UseHistory { get; set; }

        public string Anchor { get; set; }
        
        public int PageCount
        {
            get
            {
                if (_count == - 1)
                {
                    object o = ViewState["Count"];
                    _count = (o != null) ? (SQLDataHelper.GetInt(o)) : 1;
                }
                return _count;
            }
            set
            {
                ViewState["Count"] = value;
                _count = value;
            }
        }

        public int DisplayedPages
        {
            get
            {
                if (_displayedPages == 0)
                {
                    object o = ViewState["DisplayedPages"];
                    _displayedPages = (o != null) ? (SQLDataHelper.GetInt(o)) : 1;
                }
                return _displayedPages;
            }
            set
            {
                ViewState["DisplayedPages"] = value;
                _displayedPages = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                // uncomment for a table
                // return HtmlTextWriterTag.Table;
                return HtmlTextWriterTag.Div;
            }
        }

        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            int newPage;
            if (!int.TryParse(eventArgument, out newPage)) return;
            CurrentPageIndex = newPage;
            OnSelectedPageChanged(EventArgs.Empty);
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Page.Request.QueryString["Page"] == null) return;

            string curPage = Page.Request.QueryString["Page"].Split('#')[0];

            if (ValidationHelper.IsValidPositiveIntNumber(curPage))
            {
                CurrentPageIndex = SQLDataHelper.GetInt(curPage);
            }
            else
            {
                return;
            }

            OnSelectedPageChanged(new EventArgs());
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (UseHistory)
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "pageloadfunction",
                    "<script type=\"text/javascript\">function histcallback(hash) {if (hash) {" +
                    Page.ClientScript.GetPostBackEventReference(this, "{0}").Replace("\'{0}\'", "hash") + "}else{" +
                    Page.ClientScript.GetPostBackClientHyperlink(this, "1") + "}}</script>");

                Page.ClientScript.RegisterClientScriptBlock(GetType(), "documentready", "<script type=\"text/javascript\">$(document).ready(function() {$.historyInit(histcallback);});</script>");
            }
            //if (PageCount < DisplayedPages)
            //{
            //    var width = 114 + PageCount*34;
            //    Style.Add(HtmlTextWriterStyle.Width, width + "px");
            //}
            //else
            //{
            //    Style.Remove("width");
            //}

            //Style.Add("height", "26px");
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            int startPage;
            int endPage;

            if (PageCount <= 1)
                return;

            if (_count > _displayedPages)
            {
                int prevnextCount = Math.Abs((_displayedPages - 1)/2);

                int prevListCount = prevnextCount;
                int nextListCount = prevnextCount;

                if (CurrentPageIndex <= prevnextCount)
                {
                    prevListCount = CurrentPageIndex - 1;
                    nextListCount = _displayedPages - prevListCount - 1;
                }

                if (_count - CurrentPageIndex < prevnextCount)
                {
                    nextListCount = _count - CurrentPageIndex;
                    prevListCount = _displayedPages - nextListCount - 1;
                }

                startPage = CurrentPageIndex - prevListCount;
                endPage = CurrentPageIndex + nextListCount;
            }
            else
            {
                startPage = 1;
                endPage = _count;
            }
            //single page number don't render

            // uncomment for a table
            // writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            //uncomment if needed "First" link
            //if (startPage > 1)
            //{
            //    renderItem(writer, "&laquo; First", 1);
            //}
            //writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:550px;");
            //writer.RenderBeginTag(HtmlTextWriterTag.Div);
            
            if (CurrentPageIndex > 1)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "prevpage");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "pagenumberer-prev");

                RenderItem(writer, "&lt; " + LocalizationService.GetResource("Admin.Core.PageNumberer.Previous"), CurrentPageIndex - 1, isPrevious:true);
            }

            for (int i = startPage; i <= endPage; i++)
            {
                string label = i.ToString();
                //uncomment if needed ','
                //if (i != endPage)
                //    label = count.ToString() + ",";
                //else
                //    label = count.ToString();
                RenderItem(writer, label, i == CurrentPageIndex ? 0 : i);
            }
            
            if (CurrentPageIndex < _count)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "nextpage");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "pagenumberer-next");

                RenderItem(writer, LocalizationService.GetResource("Admin.Core.PageNumberer.Next") + " &gt;", CurrentPageIndex + 1, true);
            }
        }

        private void RenderItem(HtmlTextWriter writer, string text, int pageNum, bool isNext = false, bool isPrevious = false)
        {
            if (pageNum == - 1)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(text);
                writer.RenderEndTag();
                return;
            }

            if (pageNum == 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "pagenumberer-item pagenumberer-selected");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(text);
                writer.RenderEndTag();
                return;
            }

            if (UseHref)
            {
                string[] @params = (Page.Request.RawUrl.Split('?').Length > 1) ? Page.Request.RawUrl.Split('?')[1].Split('&'):null;
                bool isExistPageParam = false;
                if (@params != null)
                {
                    for (int i = 0; i <= @params.Length - 1; i++)
                    {
                        if (string.Compare(@params[i], 0, "Page", 0, 4, true) == 0)
                        {
                            @params[i] = "Page=" + pageNum;
                            isExistPageParam = true;
                            break;
                        }
                    }
                }

                if (isExistPageParam)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href,
                        Page.Request.RawUrl.Split('?')[0] + "?" + string.Join("&", @params) +
                        (Anchor != null ? "#" + Anchor : ""));
                }
                else
                {
                    if (@params == null)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href,
                            Page.Request.RawUrl.Split('?')[0] + "?Page=" + pageNum +
                            (Anchor != null ? "#" + Anchor : ""));
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href,
                            Page.Request.RawUrl.Split('?')[0] + "?" + string.Join("&", @params) + "&Page=" + pageNum +
                            (Anchor != null ? "#" + Anchor : ""));
                    }
                }
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href,
                                    Page.ClientScript.GetPostBackClientHyperlink(this, pageNum.ToString()));
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "pagenumberer-item pagenumberer-item-link");
            if (UseHistory)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("javascript:CreateHistory({0})", pageNum));
            }

            writer.RenderBeginTag(HtmlTextWriterTag.A);

            if (isNext || isPrevious)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "pagenumberer-next-text");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }

            writer.Write(text);
            writer.RenderEndTag();

            if (isNext)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "icon-right-open-after");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.RenderEndTag();
            }

            if (isPrevious)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "icon-left-open-after");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.RenderEndTag();
            }
        }
        

        public event EventHandler SelectedPageChanged
        {
            add { _selectedPageChangedEvent = (EventHandler) Delegate.Combine(_selectedPageChangedEvent, value); }
            remove { _selectedPageChangedEvent = (EventHandler) Delegate.Remove(_selectedPageChangedEvent, value); }
        }

        protected virtual void OnSelectedPageChanged(EventArgs e)
        {
            if (_selectedPageChangedEvent != null)
                _selectedPageChangedEvent(this, e);
        }
    }
}