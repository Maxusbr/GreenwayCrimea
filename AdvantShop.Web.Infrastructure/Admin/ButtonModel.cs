using AdvantShop.Web.Infrastructure.Localization;

namespace AdvantShop.Web.Infrastructure.Admin.Buttons
{
    public enum eButtonType
    {
        Simple,
        Add,
        Save,
    }

    public enum eButtonSize
    {
        Default,
        Large,
        Small,
        XSmall
    }

    public enum eColorType
    {
        Default,
        Primary,
        Success,
        Info,
        Warning,
        Danger,
        Link,
    }


    public class ButtonModel
    {
        public string Name { get; set; }
        public eButtonType Type { get; set; }
        public eColorType ColorType { get; set; }
        public eButtonSize Size { get; set; }
        public LocalizedString Text { get; set; }
        public string Link { get; set; }
        public string CssClass { get; set; }
        public string[] Attributes { get; set; }
        public bool Validation { get; set; }

        public string SizeCssClass
        {
            get {
                switch (Size)
                {
                    case eButtonSize.Large: return "lg";
                    case eButtonSize.Small: return "sm";
                    case eButtonSize.XSmall: return "xs";
                    case eButtonSize.Default:
                    default:
                        return string.Empty;
                }
            }
        }

        public string AttibutesString
        {
            get
            {
                if (Attributes == null)
                    return string.Empty;

                string res = "";
                foreach (var str in Attributes)
                {
                    res += str + " ";
                }
                return res.Trim();
            }
        }

    }
}
