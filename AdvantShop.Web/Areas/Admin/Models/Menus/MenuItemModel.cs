using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.CMS;
using AdvantShop.Core.UrlRewriter;

namespace AdvantShop.Web.Admin.Models.Menus
{
    public class MenuItemModel : IValidatableObject
    {
        public int MenuItemId { get; set; }
        public int MenuItemParentId { get; set; }
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

        public string MenuItemIconPath
        {
            get { return !string.IsNullOrEmpty(MenuItemIcon) ? UrlService.GetAbsoluteLink("/pictures/icons/" + MenuItemIcon) : null; }
        }

        public string MenuItemParentName
        {
            get
            {
                if (MenuItemParentId == 0)
                    return "Корневой элемент";

                var parent = MenuService.GetMenuItemById(MenuItemParentId);
                if (parent != null)
                    return parent.MenuItemName;

                return null;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MenuItemName))
            {
                yield return new ValidationResult("Укажите название", new[] { "MenuItemName" });
            }
        }
    }
}
