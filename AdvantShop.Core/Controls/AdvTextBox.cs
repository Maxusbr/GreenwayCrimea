//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Controls
{
    public sealed class AdvTextBox : WebControl, IPostBackDataHandler
    {
        public enum eTextMode
        {
            Text = 0,
            Password = 1,
            Multiline = 2,
            Search = 3
        }

        public string Placeholder { set; get; }
        public EValidationType ValidationType { set; get; }
        public string ValidationGroup { get; set; }
        public string DefaultButtonId { get; set; }
        public string Text { get; set; }
        public eTextMode TextMode { set; get; }
        public bool ReadOnly { get; set; }
        public bool Disabled { get; set; }
        public int MaxLength { get; set; }
        public bool IsWrap { get; set; }
        public string CssClassWrap { get; set; }

        public AdvTextBox()
        {
            IsWrap = true;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (IsWrap)
            {
                var cssClassWrapDefault = TextMode == eTextMode.Multiline ? "textarea-wrap" : "input-wrap";

                writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClassWrapDefault + (CssClassWrap.IsNotEmpty() ? " " + CssClassWrap : ""));
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }

            if (Placeholder.IsNotEmpty())
            {
                writer.AddAttribute("placeholder", Placeholder);
            }

            if (DefaultButtonId.IsNotEmpty())
            {
                writer.AddAttribute("onkeyup", string.Format("defaultButtonClick('{0}', event)", DefaultButtonId));
            }

            if (ReadOnly)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");
            }

            if (Disabled)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            }

            if (MaxLength != 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, MaxLength.ToString());
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ID);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("{0}{1}{2}",
                CssClass.IsNotEmpty() ? CssClass : string.Empty,
                ValidationType != EValidationType.None ? " valid-" + ValidationType.ToString().ToLower() : string.Empty,
                ValidationGroup.IsNotEmpty() ? " group-" + ValidationGroup : string.Empty));

            if (TextMode == eTextMode.Multiline)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Textarea);
                writer.Write(Text);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Type, TextMode.ToString().ToLower());
                writer.AddAttribute(HtmlTextWriterAttribute.Value, Text ?? string.Empty);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
            }
            writer.RenderEndTag();

            if (IsWrap)
            {
                writer.RenderEndTag();
            }
        }

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            var presentValue = Text;
            var postedValue = postCollection[postDataKey];

            if (presentValue == null || !presentValue.Equals(postedValue))
            {
                Text = postedValue;
                return true;
            }

            return false;
        }

        public void RaisePostDataChangedEvent()
        {
        }
    }
}