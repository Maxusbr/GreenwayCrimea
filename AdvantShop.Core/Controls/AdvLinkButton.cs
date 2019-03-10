//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdvantShop.Core.Controls
{
    public class AdvLinkButton : LinkButton
    {
        #region  Properties

        public string CssMain { get; set; }

        public string CssLeftDiv { get; set; }

        public string CssRightDiv { get; set; }

        public string CssInput { get; set; }

        #endregion

        public AdvLinkButton()
        {
            CssMain = "adv_RoundButton_Main";
            CssLeftDiv = "adv_RoundButton_LeftDiv";
            CssRightDiv = "adv_RoundButton_RightDiv";
            CssInput = "adv_RoundButton_Input";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CssMain);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            // -------
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CssLeftDiv);
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
                                string.Format("document.getElementById(\'{0}\').click()", ClientID));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
            // -------
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CssInput);
            base.Render(writer);
            // -------
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CssRightDiv);
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
                                string.Format("document.getElementById(\'{0}\').click()", ClientID));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
            // -------
            writer.RenderEndTag();
        }
    }
}