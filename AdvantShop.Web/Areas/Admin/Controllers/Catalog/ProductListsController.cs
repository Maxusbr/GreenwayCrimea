using System;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Catalog;
using AdvantShop.Web.Admin.Handlers.ProductLists;
using AdvantShop.Web.Admin.Models.Catalog;
using AdvantShop.Web.Admin.Models.ProductLists;
using AdvantShop.Web.Admin.ViewModels.ProductLists;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    public class ProductListsController :  BaseAdminController
    {
        #region Product lists

        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.ProductLists.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.ProductListsCtrl);

            return View();
        }

        public JsonResult GetProductLists(ProductListsFilterModel model)
        {
            return Json(new GetProductListsHandler(model).Execute());
        }

        public JsonResult GetProductList(int id)
        {
            return Json(ProductListService.Get(id));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddProductList(ProductList list)
        {
            if (string.IsNullOrWhiteSpace(list.Name))
                return JsonError();

            ProductListService.Add(list);

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateProductList(ProductList list)
        {
            if (string.IsNullOrWhiteSpace(list.Name) || list.Id == 0)
                return JsonError();

            var productList = ProductListService.Get(list.Id);
            if (productList == null)
                return JsonError();

            ProductListService.Update(list);

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteProductList(int id)
        {
            ProductListService.Delete(id);
            return JsonOk();
        }

        private void ListsCommand(ProductListsFilterModel command, Action<int, ProductListsFilterModel> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                    func(id, command);
            }
            else
            {
                var ids = new GetProductListsHandler(command).GetItemsIds("Id");
                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteProductLists(ProductListsFilterModel command)
        {
            ListsCommand(command, (id, c) => ProductListService.Delete(id));
            return JsonOk();
        }


        #endregion


        #region Products of list

        public ActionResult Products(int id)
        {
            var list = ProductListService.Get(id);
            if (list == null)
                return RedirectToAction("Index");

            var model = new PropductsByListViewModel() {ListId = list.Id, ListName = list.Name};

            SetMetaInformation(T("Admin.ProductLists.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.ProductListsCtrl);

            return View(model);
        }

        public JsonResult GetProductsByList(CatalogFilterModel model)
        {
            var handler = new GetProductsByList(model);
            var result = handler.Execute();

            return Json(result);
        }


        private void Command(CatalogFilterModel command, Action<int, CatalogFilterModel> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                    func(id, command);
            }
            else
            {
                var ids = new GetProductsByList(command).GetItemsIds("[Product].[ProductID]");
                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteProductsFromList(CatalogFilterModel command)
        {
            Command(command, (id, c) =>
            {
                if (command.ListId != null)
                    ProductListService.DeleteProduct(command.ListId.Value, id);
            });
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteFromList(int listId, int productId)
        {
            ProductListService.DeleteProduct(listId, productId);
            return Json(true);
        }
        

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddProducts(CatalogFilterModel model)
        {
            var listId = model.ListId != null ? model.ListId.Value : 0;

            if (ProductListService.Get(listId) == null)
                return Json(new {result = false});

            if (model.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in model.Ids)
                    ProductListService.AddProduct(listId, id, 0);
            }
            else
            {
                if (model.CategoryId == 0)
                    model.ShowMethod = ECatalogShowMethod.AllProducts;

                var handler = new GetCatalog(model);
                var ids = handler.GetItemsIds<int>("[Product].[ProductID]");

                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        ProductListService.AddProduct(listId, id, 0);
                }
            }
            
            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult InplaceProduct(ProductListItemModel model)
        {
            ProductListService.UpdateProduct(model.ListId, model.ProductId, model.SortOrder);
            return Json(new { result = true });
        }

        #endregion

    }
}
