//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdvantShop.Core.Controls
{
    /// <summary>
    /// Summary description for AdvAsyncTreeNode
    /// </summary>
    public class AdvAsyncTreeNode : TreeNode
    {
        private bool _enabled = true;
        public bool Bold { get; set; }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        protected override void RenderPreText(HtmlTextWriter writer)
        {
            if (Bold)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.B);
            }
            base.RenderPreText(writer);

            if (Enabled)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "ATreeView_Select(this,\'" + Value + "\')");
            }
        }

        protected override void RenderPostText(HtmlTextWriter writer)
        {
            base.RenderPostText(writer);
            if (Bold)
            {
                writer.RenderEndTag();
            }
        }

        protected override object Clone()
        {
            return MemberwiseClone();
        }
    }
}