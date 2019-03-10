//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Security.Permissions;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Controls
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class PageNumberer2 : WebControl, IPostBackEventHandler
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

        public bool EnableHistory { get; set; }

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

            if (EnableHistory)
            {
                if (! Page.ClientScript.IsStartupScriptRegistered("dummy"))
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "dummy", "AddDummyQueryString();", true);
                }

                var sb = new StringBuilder();

                sb.AppendLine("//Обход бага в Opera для работы Asp.Net Ajax History");
                sb.AppendLine("function AddDummyQueryString() {");
                sb.AppendLine("var browser = navigator.appName;");
                sb.AppendLine("if (browser == \"Opera\") {");
                sb.AppendLine("if (!(window.location.href.indexOf(\"x=y\") != -1))");
                sb.AppendLine(
                    "window.location.href = window.location.href + (window.location.href.indexOf(\"?\") != -1 ? \"&\" : \"?\") + \"x=y\";");
                sb.AppendLine("}");
                sb.AppendLine("}");


                Page.ClientScript.RegisterClientScriptBlock(GetType(), "AddDummyQueryString", sb.ToString(), true);
            }

            BuildControls();

        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            if (PageCount < DisplayedPages)
            {
                int width = 114 + PageCount*34;
                Style.Add(HtmlTextWriterStyle.Width, width + "px");
            }
            else
            {
                Style.Remove("width");
            }

            if (PageCount > 1)
            {
                Style.Add("height", "26px");
            }
            Controls.Clear();
            BuildControls();
        }

        private void ButtonCommandHandler(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "SwitchPage")
            {
                CurrentPageIndex = (int)e.CommandArgument;

                if (_selectedPageChangedEvent != null)
                    _selectedPageChangedEvent(this, new EventArgs());
            }
        }

        private void BuildControls()
        {
            if (PageCount < 2)
            {
                return;
            }

            int startPage;
            int endPage;

            if (PageCount > DisplayedPages)
            {
                var prevnextCount = Math.Abs((DisplayedPages - 1)/2);

                var prevListCount = prevnextCount;
                var nextListCount = prevnextCount;

                if (CurrentPageIndex <= prevnextCount)
                {
                    prevListCount = CurrentPageIndex - 1;
                    nextListCount = DisplayedPages - prevListCount - 1;
                }

                if (PageCount - CurrentPageIndex < prevnextCount)
                {
                    nextListCount = PageCount - CurrentPageIndex;
                    prevListCount = DisplayedPages - nextListCount - 1;
                }

                startPage = CurrentPageIndex - prevListCount;
                endPage = CurrentPageIndex + nextListCount;
            }
            else
            {
                startPage = 1;
                endPage = PageCount;
            }

            if (CurrentPageIndex > 1)
            {
                var prevButton = (LinkButton)MakeControl("&lt; " + LocalizationService.GetResource("Admin.Core.PageNumberer.Previous"),
                                                    CurrentPageIndex - 1);
                prevButton.Attributes["class"] = "_prevnext";
                prevButton.ID = ID + "_prevpage";
                Controls.Add(prevButton);
            }
            else
            {
                var prevBlock = (HtmlGenericControl)MakeControl("&lt; " + LocalizationService.GetResource("Admin.Core.PageNumberer.Previous"), - 1);
                prevBlock.ID = ID + "_prevpage";
                prevBlock.Attributes["class"] = "prevnext";
                Controls.Add(prevBlock);
            }

            for (var i = startPage; i <= endPage; i++)
            {
                string label = i.ToString();
                if (i == CurrentPageIndex)
                {
                    Control c = MakeControl(label, 0);
                    Controls.Add(c);
                }
                else
                {
                    Control c = MakeControl(label, i);
                    c.ID = string.Format("{1}_pager_{0}", i, ID);
                    Controls.Add(c);
                }
            }

            if (CurrentPageIndex < PageCount)
            {
                var c = (LinkButton)MakeControl(LocalizationService.GetResource("Admin.Core.PageNumberer.Next") + " &gt;",
                                           CurrentPageIndex + 1);
                c.Attributes["class"] = "prevnext";
                c.ID = ID + "_nextpage";
                Controls.Add(c);
            }
            else
            {
                var c = (HtmlGenericControl)MakeControl(LocalizationService.GetResource("Admin.Core.PageNumberer.Next") + " &gt;", - 1);
                c.Attributes["class"] = "prevnext";
                c.ID = ID + "_nextpage";
                Controls.Add(c);
            }
        }

        private Control MakeControl(string text, int pageNum)
        {
            if (pageNum == - 1)
            {
                var control = new HtmlGenericControl("div") {InnerHtml = text};

                return control;
            }
            if (pageNum != 0)
            {
                var control = new LinkButton {Text = text, CommandName = "SwitchPage", CommandArgument = pageNum.ToString()};
                control.Attributes["class"] = "pager";
                control.Command += ButtonCommandHandler;
                return control;
            }
            else
            {
                var control = new HtmlGenericControl("div") {InnerHtml = text};
                control.Attributes["class"] = "selected";
                return control;
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