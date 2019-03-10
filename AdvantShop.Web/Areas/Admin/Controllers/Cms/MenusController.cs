using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Menus;
using AdvantShop.Web.Admin.Models.Menus;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Cms
{
    [Auth(RoleAction.Cms)]
    public partial class MenusController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Menus.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.MenusCtrl);

            return View();
        }
        
        public JsonResult MenusTree(MenusTree model)
        {
            return Json(new GetMenusTree(model).Execute());
        }
        
        public JsonResult GetMenuItem(int menuItemId)
        {
            var item = MenuService.GetMenuItemById(menuItemId);
            if (item == null)
                return Json(null);

            return Json(new MenuItemModel()
            {
                MenuItemId = item.MenuItemID,
                MenuItemParentId = item.MenuItemParentID,
                MenuItemName = item.MenuItemName,
                MenuItemIcon = item.MenuItemIcon,
                MenuItemUrlPath = item.MenuItemUrlPath,
                MenuItemUrlType = item.MenuItemUrlType,
                SortOrder = item.SortOrder,
                ShowMode = item.ShowMode,
                Enabled = item.Enabled,
                Blank = item.Blank,
                NoFollow = item.NoFollow,
                MenuType = item.MenuType,
                HasChild = item.HasChild
            });
        }
        

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddMenuItem(MenuItemModel model)
        {
            return ProcessJsonResult(new SaveMenuItem(model));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateMenuItem(MenuItemModel model)
        {
            return ProcessJsonResult(new SaveMenuItem(model));            
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteMenuItem(int menuItemId, EMenuType menuType)
        {
            DeleteSubMenuItems(menuItemId, menuType);
            return Json(new { result = true });
        }

        private void DeleteSubMenuItems(int menuItemId, EMenuType menuType)
        {
            foreach (var id in MenuService.GetAllChildIdByParent(menuItemId, menuType))
            {
                if (id != menuItemId)
                    DeleteSubMenuItems(id, menuType);

                var menuItem = MenuService.GetMenuItemById(id);

                if (menuItem != null)
                {
                    if (!string.IsNullOrEmpty(menuItem.MenuItemIcon))
                        MenuService.DeleteMenuItemIconById(id, FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, menuItem.MenuItemIcon));

                    MenuService.DeleteMenuItemById(id);
                }
            }
        }

        public JsonResult GetLinkUrl(MenuLinkModel model)
        {
            var url = "";

            switch (model.Type)
            {
                case EMenuItemUrlType.Product:
                    url = UrlService.GetLinkDB(ParamType.Product, model.ProductId);
                    break;
                case EMenuItemUrlType.Category:
                    url = UrlService.GetLinkDB(ParamType.Category, model.CategoryId);
                    break;
                case EMenuItemUrlType.StaticPage:
                    url = UrlService.GetLinkDB(ParamType.StaticPage, model.StaticPageId);
                    break;
                case EMenuItemUrlType.News:
                    url = UrlService.GetLinkDB(ParamType.News, model.NewsId);
                    break;
                case EMenuItemUrlType.Brand:
                    url = UrlService.GetLinkDB(ParamType.Brand, model.BrandId);
                    break;
            }
            return Json(new { result = true, url = url });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeMenuSortOrder(int itemId, int? prevItemId, int? nextItemId, int? parentItemId)
        {
            var handler = new ChangeMenuItemSortOrder(itemId, prevItemId, nextItemId, parentItemId);
            var result = handler.Execute();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadIcon(int? itemId)
        {
            var handler = new UploadMenuIcon(itemId);
            var result = handler.Execute();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteIcon(int? itemId, string menuItemIcon)
        {
            if (itemId == null && string.IsNullOrEmpty(menuItemIcon))
                return Json(new {result = false});

            var isDeleted = false;
            
            if (itemId != null)
            {
                var menuItem = MenuService.GetMenuItemById(itemId.Value);
                if (menuItem != null)
                {
                    MenuService.DeleteMenuItemIconById(itemId.Value, 
                        FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, menuItem.MenuItemIcon));

                    menuItem.MenuItemIcon = null;
                    MenuService.UpdateMenuItem(menuItem);

                    isDeleted = true;
                }
            }

            if (!isDeleted)
                MenuService.DeleteMenuItemIconById(0, FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, menuItemIcon));

            return Json(new { result = true });
        }

    }
}
