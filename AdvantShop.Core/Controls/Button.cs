//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Controls
{
    public enum eType
    {
        None = 0,
        Action = 1,
        Add = 2,
        Buy = 3,
        Confirm = 4,
        Submit = 5
    }

    public enum eSize
    {
        None = 0,
        XSmall = 1,
        Small = 2,
        Middle = 3,
        Big = 4
    }

    public sealed class Button : WebControl, IPostBackEventHandler
    {
        #region Fields
        
        public string Text { set; get; }
        public eType Type { set; get; }
        public eSize Size { set; get; }
        public string OnClientClick { set; get; }
        public string Href { set; get; }
        public bool DisableValidation { set; get; }
        public string Rel { set; get; }
        public string Target { set; get; }
        public string ValidationGroup { set; get; }
        public string CssSpan { set; get; }
        public string CssStyle { set; get; }
        public event EventHandler Click;

        #endregion

        public Button()
        {
            EnableViewState = false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn-c" + (CssSpan.IsNotEmpty() ? " " + CssSpan : ""));
            writer.RenderBeginTag(HtmlTextWriterTag.Span);

            if (Href.IsNotEmpty() && Click != null)
                throw new Exception("Href and Server Event can't be used together");

            if (Click != null)
            {
                //Uncomment if POSTBACK is not rising
                //writer.AddAttribute(HtmlTextWriterAttribute.Name, ClientID);
                if (DisableValidation)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + Page.ClientScript.GetPostBackEventReference(this, ClientID));
                }
                else
                {
                    //Uncomment if POSTBACK is not rising
                    //writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + string.Format("__doPostBackJQ('{0}','{1}')", UniqueID, ClientID));
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + string.Format("__doPostBackJQ('{0}','')", UniqueID));
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, Href.IsNotEmpty() ? Href : "javascript:void(0);");
            }

            if (OnClientClick.IsNotEmpty())
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, OnClientClick);
            }

            if (Rel.IsNotEmpty())
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Rel, Rel);
            }

            if (Target.IsNotEmpty())
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Target, Target);
            }

            if (CssStyle.IsNotEmpty())
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, CssStyle);
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("btn{0}{1}{2}{3}",
                                                                             Type != eType.None ? " btn-" + Type.ToString().ToLower() : string.Empty,
                                                                             Size != eSize.None ? " btn-" + Size.ToString().ToLower() : string.Empty,
                                                                             CssClass.IsNotEmpty() ? " " + CssClass : string.Empty,
                                                                             ValidationGroup.IsNotEmpty() ? " group-" + ValidationGroup : string.Empty));
            if (ID.IsNotEmpty())
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ID);
            }

            IEnumerator keys = this.Attributes.Keys.GetEnumerator();
            while (keys.MoveNext())
            {
                var key = (String)keys.Current;
                writer.AddAttribute(key, this.Attributes[key]);
            }

            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(Text);
            writer.RenderEndTag(); // a

            writer.RenderEndTag(); // span
        }

        public static string RenderHtml(string text, eType type, eSize size, string cssClass = "",
            string onClientClick = "", string href = "javascript:void(0);", string rel = "", string target = "")
        {
            var res = new StringBuilder();
            res.Append("<span class=\"btn-c\">");

            cssClass = string.Format("btn{0}{1}{2}",
                                        type != eType.None ? " btn-" + type.ToString().ToLower() : string.Empty,
                                        size != eSize.None ? " btn-" + size.ToString().ToLower() : string.Empty,
                                        cssClass.IsNotEmpty() ? " " + cssClass : string.Empty);

            res.AppendFormat("<a class=\"{0}\" href=\"{1}\" {2}{3}{4}>{5}</a>",
                                cssClass, href,
                                onClientClick.IsNotEmpty() ? "onclick=\"" + onClientClick + "\"" : string.Empty,
                                rel.IsNotEmpty() ? "rel=\"" + rel + "\"" : string.Empty,
                                target.IsNotEmpty() ? "target='" + target : string.Empty,
                                text);
            res.Append("</span>");

            return res.ToString();
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (Click != null) Click(this, EventArgs.Empty);
        }
    }
}