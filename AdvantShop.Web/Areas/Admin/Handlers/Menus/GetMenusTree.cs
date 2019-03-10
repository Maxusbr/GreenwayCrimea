using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.CMS;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Web.Admin.Models.Menus;

namespace AdvantShop.Web.Admin.Handlers.Menus
{
    public class GetMenusTree
    {
        private readonly string _id;
        private readonly int _idSelected;
        private readonly EMenuType _type;
        private readonly bool _showRoot;
        private readonly bool _showActions;
        private readonly int? _excludeId;

        public GetMenusTree(MenusTree model)
        {
            _id = model.Id;
            _idSelected = model.SelectedId != null ? model.SelectedId.Value : -1;
            _type = model.MenuType;
            _showRoot = model.ShowRoot;
            _showActions = model.ShowActions;
            _excludeId = model.ExcludeId;
        }

        public List<AdminMenuTreeViewItem> Execute()
        {
            if (_showRoot)
            {
                return new List<AdminMenuTreeViewItem>()
                    {
                        new AdminMenuTreeViewItem()
                        {
                            id = "0",
                            parent = "#",
                            text = String.Format("<span class=\"jstree-advantshop-name\">{0}</span>", "Корень"),
                                name = "Корень",
                                children = true,
                                state = new AdminMenuTreeViewItemState()
                                {
                                    opened = true,
                                    selected = _idSelected == 0 || _idSelected == -1
                                },
                            li_attr = new Dictionary<string, string>() {
                                { "data-tree-id", "menuItemId_" +  0}
                            },
                        }
                    };
            }


            var menuItems = MenuService.GetAllMenuItems(_id.TryParseInt(), _type);

            if (_excludeId != null && _excludeId != 0)
                menuItems = menuItems.Where(x => x.ItemId != _excludeId).ToList();
            

            var itemOpen = 0;

            if (menuItems.Any(x => x.ItemId != _idSelected))
            {
                var item = MenuService.GetMenuItemById(_idSelected);
                if (item != null)
                {
                    int i = 20;
                    while (item.MenuItemParentID > 0 && i > 0)
                    {
                        if (menuItems.Any(x => x.ItemId == item.MenuItemParentID))
                        {
                            itemOpen = item.MenuItemParentID;
                            break;
                        }
                        item = MenuService.GetMenuItemById(item.MenuItemParentID);
                        i--;
                    }
                }
            }

            return menuItems.Select(x => new AdminMenuTreeViewItem()
            {
                id = x.ItemId.ToString(),
                parent = _id == "#" && x.ItemParentId == 0 ? "#" : x.ItemParentId.ToString(),
                text = _showActions 
                            ? String.Format(
                                "<span class=\"jstree-advantshop-grab\"><icon-move /></span> " +
                                "<span class=\"jstree-advantshop-name\">{0}</span> " +
                                "<span class=\"jstree-advantshop-count\"> " +
                                    "<menu-item-actions data-id='{1}' data-type='{2}'></menu-item-actions> " +
                                "</span>",
                                x.Name, x.ItemId, _type)
                            : String.Format(
                                "<span class=\"jstree-advantshop-grab\"><icon-move /></span> " +
                                "<span class=\"jstree-advantshop-name\">{0}</span> ", x.Name),
                name = x.Name,
                children = _type != EMenuType.Mobile ? x.HasChild : false,
                li_attr = new Dictionary<string, string>() {
                    { "class", "jstree-advantshop-li-grable" },
                    { "data-tree-id", "menuItemId_" +  x.ItemId}
                },
                state = new AdminMenuTreeViewItemState()
                {
                    opened = x.ItemId == itemOpen,
                    selected = x.ItemId == _idSelected
                }
            }).ToList();
        }
    }
}
