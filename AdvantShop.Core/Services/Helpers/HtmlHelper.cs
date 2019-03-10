//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Text;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Helpers
{
    public class HtmlHelper
    {
        public static string RenderSplitter()
        {
            var str = new StringBuilder();
            str.Append("<td class=\'splitter\'  onclick=\'toggleRightPanel();return false;\' >");
            str.Append("<div class=\'RightPanelTop\'></div>");
            if (LocalizationService.GetResource("Admin.Catalog.Helpers.SplitterLang") == "rus")
            {
                str.Append("<div id=\'right_divHide\' class=\'right_hide_rus\'></div>");
                str.Append("<div id=\'right_divShow\' class=\'right_show_rus\'></div>");
            }
            else if (LocalizationService.GetResource("Admin.Catalog.Helpers.SplitterLang") == "eng")
            {
                str.Append("<div id=\'right_divHide\' class=\'right_hide_en\'></div>");
                str.Append("<div id=\'right_divShow\' class=\'right_show_en\'></div>");
            }
            str.Append("</td>");
            return str.ToString();
        }
    }
}