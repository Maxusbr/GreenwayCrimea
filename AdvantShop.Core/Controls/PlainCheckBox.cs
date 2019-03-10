//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Web;
using System.Web.UI;

namespace AdvantShop.Core.Controls
{
    /// <summary>
    /// Summary description for PlainCheckBox
    /// </summary>
    public class PlainCheckBox : Control
    {
        public bool Checked { get; set; }
        public bool Enabled { get; set; }
        public string CssClass { get; set; }

        protected override void OnLoad(System.EventArgs e)
        {
            ReadPostBack();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!Visible) return;

            writer.AddAttribute("type", "checkbox");
            if (!string.IsNullOrEmpty(CssClass))
                writer.AddAttribute("class", CssClass);
            if (!Enabled)
                writer.AddAttribute("disabled", "disabled");
            if (Checked)
                writer.AddAttribute("checked", "checked");
            writer.AddAttribute("id", ClientID);
            writer.AddAttribute("name", ClientID);
            writer.RenderBeginTag("input");
            writer.RenderEndTag();
        }
        protected void ReadPostBack()
        {
            var formVal = HttpContext.Current.Request.Form[ClientID];
            if (formVal == null || formVal.ToLower() == "off")
                Checked = false;
            else
                Checked = true;
        }
    }
}