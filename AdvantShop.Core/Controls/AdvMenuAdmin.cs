//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Saas;
using AdvantShop.Security;

namespace AdvantShop.Core.Controls
{
    public class AdvMenuAdmin : Menu
    {
        #region  Properties

        public Customer CurrentCustomer { get; set; }

        private readonly string _requestUrl = HttpContext.Current.Request.Path.ToLower(); //Path - get url without parameters for .EndsWith

        private readonly Dictionary<string, bool> _modules = AdvantshopConfigService.GetActivityModules();
        
        private List<MenuItem> Submenu { get; set; } 

        #endregion

        #region Help methods

        /// <summary>
        /// Set enabled of child items
        /// </summary>
        /// <param name="parentItem"></param>
        private void SetChildMenuItemsEnabled(MenuItem parentItem)
        {
            parentItem.Enabled = RoleAccess.Check(CurrentCustomer, parentItem.NavigateUrl.ToLower()) &&
                                 (!_modules.ContainsKey(parentItem.Value) || _modules[parentItem.Value]);

            foreach (MenuItem item in parentItem.ChildItems)
                SetChildMenuItemsEnabled(item);
        }

        /// <summary>
        /// Return true if any child has enabled, else item enabled
        /// </summary>
        /// <param name="parentItem"></param>
        /// <returns></returns>
        private bool GetChildItemsEnabled(MenuItem parentItem)
        {
            foreach (MenuItem item in parentItem.ChildItems)
            {
                if (item.Enabled || item.ChildItems.Cast<MenuItem>().Any(i => i.Enabled))
                    return true;
            }
            return parentItem.Enabled;
        }

        //лишние linq запросы переписать
        private bool IsSelectParent(MenuItem parentItem)
        {
            if (_requestUrl.EndsWith(parentItem.NavigateUrl.ToLower()))
            {
                return true;
            }
            foreach (var childItem in parentItem.ChildItems.Cast<MenuItem>())
            {
                if (_requestUrl.EndsWith(childItem.NavigateUrl.ToLower()) || childItem.ChildItems.Cast<MenuItem>().Any(item => _requestUrl.EndsWith(item.NavigateUrl.ToLower()))
                    || childItem.ChildItems.Cast<MenuItem>().Any(item => HttpContext.Current.Request.Url.AbsoluteUri.ToLower().EndsWith(item.NavigateUrl.ToLower())))
                {
                    return true;
                }
            }
            return false;
        }


        private void RenderChild(MenuItem parent, HtmlTextWriter writer)
        {
            var visible = false;

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "dropdown-menu-wrap");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "dropdown-menu");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            foreach (MenuItem item in parent.ChildItems)
            {
                if (!item.Enabled)
                    continue;

                visible = GetChildItemsEnabled(item);

                if (!visible || string.Equals(item.Value, "notvisible"))
                    continue;

                if (SaasDataService.IsSaasEnabled && item.NavigateUrl.ToLower().Contains("logviewer.aspx") && !CustomerContext.CurrentCustomer.IsVirtual)
                    continue;

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "dropdown-menu-item m-item");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, (item.Enabled ? UrlService.GetAdminAbsoluteLink(item.NavigateUrl) : "#"));

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "main-menu-subitem-lnk");
                writer.RenderBeginTag(HtmlTextWriterTag.A);

                writer.WriteLine(item.Text);

                if (item.ChildItems.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "main-menu-subarrow");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.RenderEndTag();
                }

                writer.RenderEndTag(); //a

                if (item.ChildItems.Count > 0)
                {
                    RenderChild(item, writer);
                }

                writer.RenderEndTag(); //li
            }

            writer.RenderEndTag(); //ul
            writer.RenderEndTag(); // div
        }


        #endregion


        protected override void Render(HtmlTextWriter writer)
        {
            foreach (MenuItem item in this.Items)
                SetChildMenuItemsEnabled(item);

            MenuItemCollection selectedSubmenu = null;

            if (this.ID.IsNotEmpty())
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
            }

            writer.Write("<nav class=\"main-menu\">");

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "main-menu-list");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);


            foreach (MenuItem parent in this.Items)
            {
                if (parent.NavigateUrl.ToLower().Contains("achievements.aspx") && !Trial.TrialService.IsTrialEnabled &&
                    !Saas.SaasDataService.IsSaasEnabled)
                    continue;

                if (!GetChildItemsEnabled(parent))
                    continue;

                var isSelected = IsSelectParent(parent);
                if (isSelected)
                    selectedSubmenu = parent.ChildItems;

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "main-menu-item m-item" +
                    (parent.ChildItems.Count > 0 ? " dropdown-menu-parent" : "") +
                    (isSelected ? " main-menu-item-selected" : ""));

                writer.RenderBeginTag(HtmlTextWriterTag.Li);


                if (parent.Value.IsNotEmpty())
                {
                    writer.AddAttribute("data-value", parent.Value);
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Href, parent.Enabled ? UrlService.GetAdminAbsoluteLink(parent.NavigateUrl) : "#");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "main-menu-item-lnk");
                writer.RenderBeginTag(HtmlTextWriterTag.A);


                writer.AddAttribute(HtmlTextWriterAttribute.Class, "main-menu-icon");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);

                writer.AddAttribute(HtmlTextWriterAttribute.Src, parent.ImageUrl);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();

                writer.RenderEndTag(); // span.main-menu-icon - wrap image

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "main-menu-text");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);

                writer.WriteLine(parent.Text);

                if (parent.ChildItems.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "main-menu-arrow");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.RenderEndTag();
                }

                writer.RenderEndTag(); // span.main-menu-text

                writer.RenderEndTag(); // a

                if (parent.ChildItems.Count > 0)
                {
                    RenderChild(parent, writer);
                }

                writer.RenderEndTag(); // li
            }

            if (selectedSubmenu != null && selectedSubmenu.Count > 0)
            {
                Submenu = selectedSubmenu.Cast<MenuItem>().ToList();
            }

            writer.RenderEndTag(); //ul

            writer.Write("</nav>"); ; //nav
        }


        public string RenderSubMenu()
        {
            if (Submenu == null || Submenu.Count == 0)
                return string.Empty;

            var stringWriter = new StringWriter();

            using (var writer = new HtmlTextWriter(stringWriter))
            {
                foreach (var menuItem in Submenu)
                {
                    if (!menuItem.Enabled ||  string.Equals(menuItem.Value, "notvisible"))
                        continue;

                    writer.RenderBeginTag(HtmlTextWriterTag.Li);

                    writer.WriteLine(menuItem.Text);

                    writer.RenderEndTag(); //li
                }
            }

            return stringWriter.ToString();
        }
    }
}