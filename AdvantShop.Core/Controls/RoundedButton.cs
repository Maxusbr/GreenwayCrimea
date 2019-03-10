//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Web.UI;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Controls
{
    public enum RoundedButtonType
    {
        Gray,
        Orange,
        GrayBig
    }

    public class RoundedButton : System.Web.UI.WebControls.Button
    {
        public RoundedButtonType Type { get; set; }


        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "margin: 0 0 0 0;");

            if (CssClass.IsNotEmpty())
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
            }
            
            writer.RenderBeginTag("div");

            

            RenderRightBlock(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, Type == RoundedButtonType.Orange ? "orbtnctd" : Type == RoundedButtonType.GrayBig ? "rbtnctd_big" : "rbtnctd");

            writer.RenderBeginTag("div");

            writer.AddAttribute(HtmlTextWriterAttribute.Class,
                                Type == RoundedButtonType.Orange ? "orbtninput" : Type == RoundedButtonType.GrayBig ? "rbtninput_big" : "rbtninput");

            base.Render(writer);

            writer.RenderEndTag();

            RenderLeftBlock(writer);

            writer.RenderEndTag();
        }

        private void RenderRightBlock(HtmlTextWriter writer)
        {

            writer.AddAttribute(HtmlTextWriterAttribute.Style, Type == RoundedButtonType.Orange || Type == RoundedButtonType.GrayBig
                                                                          ? "height:27px;"
                                                                          : "height:23px;");

            writer.AddAttribute(HtmlTextWriterAttribute.Class, Type == RoundedButtonType.Orange ? "orbtnrtd" : Type == RoundedButtonType.GrayBig ? "rbtnrtd_big" : "rbtnrtd");


            writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
                                string.Format("document.getElementById(\'{0}\').click()", ClientID));
            writer.RenderBeginTag("div");
            writer.RenderEndTag();
        }

        private void RenderLeftBlock(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Style, Type == RoundedButtonType.Orange || Type == RoundedButtonType.GrayBig
                                                                          ? "height:27px;"
                                                                          : "height:23px;");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, Type == RoundedButtonType.Orange ? "orbtnltd" : Type == RoundedButtonType.GrayBig ? "rbtnltd_big" : "rbtnltd");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
                                string.Format("document.getElementById(\'{0}\').click()", ClientID));
            writer.RenderBeginTag("div");
            writer.RenderEndTag();
        }
    }
}