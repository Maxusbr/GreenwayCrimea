//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Controls
{
    public class AdvDropDownList : DropDownList
    {
        public enum eValidationType
        {
            None = 0,
            Required = 1
        }

        public eValidationType ValidationType { set; get; }
        public string DefaultButtonID { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            if (DefaultButtonID.IsNotEmpty())
            {
                writer.AddAttribute("onkeyup", string.Format("defaultButtonClick('{0}', event)", DefaultButtonID));
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("{0}{1}{2}",
                                                                             CssClass.IsNotEmpty() ? CssClass : string.Empty,
                                                                             ValidationType != eValidationType.None ? " valid-" + ValidationType.ToString().ToLower() : string.Empty,
                                                                             ValidationGroup.IsNotEmpty() ? " group-" + ValidationGroup : string.Empty));
            base.Render(writer);
        }
    }
}