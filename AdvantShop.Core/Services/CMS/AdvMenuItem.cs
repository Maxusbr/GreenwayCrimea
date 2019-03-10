//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.CMS
{
    public enum EMenuType
    {
        [Localize("Core.Cms.EMenuType.Top")]
        Top = 0,
        [Localize("Core.Cms.EMenuType.Bottom")]
        Bottom = 1,
        [Localize("Core.Cms.EMenuType.Mobile")]
        Mobile = 2
    }

    public enum EMenuItemUrlType
    {
        Product = 0,
        Category = 1,
        StaticPage = 2,
        News = 3,
        Brand = 4,
        Custom = 5
    }

    public enum EMenuItemShowMode
    {
        All = 0,
        Authorized = 1,
        NotAuthorized = 2
    }

    public class AdvMenuItem
    {
        public AdvMenuItem()
        {
            SubItems = new List<AdvMenuItem>();
        }

        public int MenuItemID { get; set; }
        public int MenuItemParentID { get; set; }
        public string MenuItemName { get; set; }
        public string MenuItemIcon { get; set; }
        public string MenuItemUrlPath { get; set; }
        public EMenuItemUrlType MenuItemUrlType { get; set; }
        public int SortOrder { get; set; }
        public EMenuItemShowMode ShowMode { get; set; }
        public bool Enabled { get; set; }
        public bool Blank { get; set; }
        public bool NoFollow { get; set; }
        public EMenuType MenuType { get; set; }
        public bool HasChild { get; set; }
        public List<AdvMenuItem> SubItems { get; set; } 
    }
}