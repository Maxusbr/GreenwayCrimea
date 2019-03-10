using System.Linq;
using AdvantShop.CMS;

namespace AdvantShop.Web.Admin.Handlers.Menus
{
    public class ChangeMenuItemSortOrder
    {
        private readonly int _itemId;
        private readonly int? _prevItemId;
        private readonly int? _nextItemId;
        private readonly int? _parentItemId;

        public ChangeMenuItemSortOrder(int itemId, int? prevItemId, int? nextItemId, int? parentItemId)
        {
            _itemId = itemId;
            _prevItemId = prevItemId;
            _nextItemId = nextItemId;
            _parentItemId = parentItemId;
        }

        public bool Execute()
        {
            var item = MenuService.GetMenuItemById(_itemId);
            if (item == null)
                return false;

            AdvMenuItem prevItem = null;
            AdvMenuItem nextItem = null;

            if (_prevItemId != null)
                prevItem = MenuService.GetMenuItemById(_prevItemId.Value);

            if (_nextItemId != null)
                nextItem = MenuService.GetMenuItemById(_nextItemId.Value);

            if (_parentItemId != null && _parentItemId != item.MenuItemParentID && _parentItemId >= 0)
            {
                item.MenuItemParentID = _parentItemId.Value;
                MenuService.UpdateMenuItem(item);
            }
            
            if (prevItem != null && nextItem != null)
            {
                if (nextItem.SortOrder - prevItem.SortOrder > 1)
                {
                    item.SortOrder = prevItem.SortOrder + 1;
                    MenuService.UpdateMenuItem(item);
                }
                else
                {
                    UpdateSortOrderForAll(item, prevItem, nextItem);
                }
            }
            else if (prevItem != null || nextItem != null)
            {
                UpdateSortOrderForAll(item, prevItem, nextItem);
            }

            return true;
        }

        private void UpdateSortOrderForAll(AdvMenuItem item, AdvMenuItem prevItem, AdvMenuItem nextItem)
        {
            var menus =
                MenuService.GetChildMenuItemsByParentId(item.MenuItemParentID, item.MenuType)
                    .Where(x => x.MenuItemID != item.MenuItemID)
                    .OrderBy(x => x.SortOrder)
                    .ToList();

            if (prevItem != null)
            {
                var index = menus.FindIndex(x => x.MenuItemID == prevItem.MenuItemID);
                menus.Insert(index + 1, item);
            }
            else if (nextItem != null)
            {
                var index = menus.FindIndex(x => x.MenuItemID == nextItem.MenuItemID);
                menus.Insert(index > 0 ? index - 1 : 0, item);
            }

            for (int i = 0; i < menus.Count; i++)
            {
                menus[i].SortOrder = i * 10 + 10;
                MenuService.UpdateMenuItem(menus[i]);
            }
        }
    }
}
