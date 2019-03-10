using System.Linq;
using AdvantShop.CMS;
using AdvantShop.Web.Admin.Models.Menus;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Menus
{
    public class SaveMenuItem : AbstractCommandHandler
    {
        private readonly MenuItemModel _model;

        public SaveMenuItem(MenuItemModel model)
        {
            _model = model;
        }

        protected override void Validate()
        {

        }

        protected override void Handle()
        {
            var item = _model.MenuItemId != 0
               ? MenuService.GetMenuItemById(_model.MenuItemId)
               : new AdvMenuItem();

            item.MenuItemName = _model.MenuItemName;
            item.MenuItemParentID = _model.MenuItemParentId;
            item.MenuItemUrlPath = _model.MenuItemUrlPath ?? "";
            item.SortOrder = _model.SortOrder;
            item.Blank = _model.Blank;
            item.Enabled = _model.Enabled;
            item.MenuItemUrlType = _model.MenuItemUrlType;
            item.NoFollow = _model.NoFollow;
            item.MenuType = _model.MenuType;
            item.ShowMode = _model.ShowMode;
            item.MenuItemIcon = _model.MenuItemIcon;

            if (item.MenuItemID != 0)
            {
                MenuService.UpdateMenuItem(item);
            }
            else
            {
                var items = MenuService.GetChildMenuItemsByParentId(item.MenuItemParentID, item.MenuType);
                if (items != null && items.Count > 0)
                {
                    item.SortOrder = items.Max(x => x.SortOrder) + 10;
                }

                MenuService.AddMenuItem(item);
            }
        }
    }
}
