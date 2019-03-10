using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Catalog;
using AdvantShop.Web.Admin.Handlers.Categories;
using AdvantShop.Web.Admin.Models.Catalog;
using AdvantShop.Web.Admin.ViewModels.Catalog;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Saas;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using AdvantShop.Core.Services.FullSearch;
using AdvantShop.Diagnostics;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    public partial class CatalogController : BaseAdminController
    {
        // GET: Admin/Catalog
        public ActionResult Index(CatalogFilterModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                var product = ProductService.GetProduct(model.Search, true);
                if (product != null)
                    return RedirectToAction("Edit", "Product", new { id = product.ProductId });
            }

            SetMetaInformation(T("Admin.Catalog.Index.CatalogTitle"));
            SetNgController(NgControllers.NgControllersTypes.CatalogCtrl);

            var viewModel = new GetCatalogIndexHandler(model).Execute();
            if (viewModel == null)
                return Error404();

            return View(viewModel);
        }


        public JsonResult GetCatalog(CatalogFilterModel model)
        {
            return Json(new GetCatalog(model).Execute());
        }

        [ChildActionOnly]
        public ActionResult CatalogLeftMenu(string ngCallbackOnInit)
        {
            var model = new GetCatalogLeftMenu().Execute();
            model.NgCallbackOnInit = ngCallbackOnInit;

            return PartialView(model);
        }

        public JsonResult GetDataProducts()
        {
            return Json(new GetCatalogLeftMenu().Execute());
        }

        [ChildActionOnly]
        public ActionResult CatalogTreeView(AdminCatalogTreeView model)
        {
            return PartialView(model);
        }

        #region CategoriesTree
        [Auth(RoleAction.Catalog, RoleAction.Orders, RoleAction.Crm)]
        public JsonResult CategoriesTree(CategoriesTree model)
        {
            return Json(new GetCategoriesTree(model).Execute());
        }
        [Auth(RoleAction.Catalog, RoleAction.Orders, RoleAction.Crm)]
        public JsonResult GetSelectedCategoriesTree(List<int> selectedIds)
        {
            return Json(new GetSelectedCategoriesTree(selectedIds).Execute());
        }

        #endregion

        #region Categories

        public JsonResult CategoryListJson(int categoryId, string categorySearch)
        {
            return Json(new GetCategoryList(categoryId, categorySearch).Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeCategorySortOrder(int categoryId, int? prevCategoryId, int? nextCategoryId, int? parentCategoryId)
        {
            var result = new ChangeCategorySortOrder(categoryId, prevCategoryId, nextCategoryId, parentCategoryId).Execute();
            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeParentCategory(int categoryId, int parentId)
        {
            var result = new ChangeParentCategory(categoryId, parentId).Execute();
            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCategories(List<int> categoryIds)
        {
            foreach (var categoryId in categoryIds.Where(x => x != 0))
            {
                CategoryService.DeleteCategoryAndPhotos(categoryId);
            }
            CategoryService.RecalculateProductsCountManual();

            return JsonOk();
        }

        #endregion

        #region Inplace

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceProduct(string price, string amount, CatalogProductModel model, int categoryId, ECatalogShowMethod showMethod)
        {
            if (SaasDataService.IsSaasEnabled && ProductService.GetProductsCount("[Enabled] = 1") > SaasDataService.CurrentSaasData.ProductsCount)
            {
                return JsonError("Ограничения тарифа аренды по товарам");
            }

            var product = ProductService.GetProduct(model.ProductId);

            bool enabledChanged = product.Enabled != model.Enabled;
            product.Enabled = model.Enabled;
            product.ModifiedBy = CustomerContext.CustomerId.ToString();

            if (categoryId != 0 && showMethod == ECatalogShowMethod.Normal)
            {
                ProductService.UpdateProductLinkSort(product.ProductId, model.SortOrder, categoryId);
            }

            if (product.Offers.Count == 1)
            {
                product.Offers[0].Amount = model.Amount = amount.Replace(" ", "").TryParseFloat();
                product.Offers[0].BasePrice = model.Price = price.Replace(" ", "").TryParseFloat();
            }

            ProductService.UpdateProduct(product, true);

            var reloadCatalogTree = false;
            if (enabledChanged)
            {
                CategoryService.RecalculateProductsCountManual();
                reloadCatalogTree = true;
            }

            return Json(new { result = true, entity = model, reloadCatalogTree });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult DeleteProduct(int productId)
        {
            ProductService.DeleteProduct(productId, true);
            CategoryService.RecalculateProductsCountManual();

            return Json(new { result = true, reloadCatalogTree = true });
        }

        #endregion

        #region Comands

        private void Command(CatalogFilterModel command, Action<int, CatalogFilterModel> func)
        {
            var exceptions = new ConcurrentQueue<Exception>();

            if (command.SelectMode == SelectModeCommand.None)
            {
                Parallel.ForEach(command.Ids, new ParallelOptions { MaxDegreeOfParallelism = 10 },  (id) =>
                {
                    try
                    {
                        func(id, command);
                    }
                    catch (Exception e)
                    {
                        exceptions.Enqueue(e);
                    }
                });

                //foreach (var id in command.Ids)
                //    func(id, command);
            }
            else
            {
                var ids = new GetCatalog(command).GetItemsIds<int>("[Product].[ProductID]");

                Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = 10 }, (id) =>
                {
                    try
                    {
                        if (command.Ids == null || !command.Ids.Contains(id))
                            func(id, command);
                    }
                    catch (Exception e)
                    {
                        exceptions.Enqueue(e);
                    }
                });
                //foreach (int id in ids)
                //{
                //    if (command.Ids == null || !command.Ids.Contains(id))
                //        func(id, command);
                //}
            }

            if (exceptions.Any())
            {
                Debug.Log.Error(exceptions.AggregateString("<br/>^^^<br/>"));
            }

            CategoryService.RecalculateProductsCountManual();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteProducts(CatalogFilterModel command)
        {
            Command(command, (id, c) => ProductService.DeleteProduct(id, false));
            ProductWriter.CreateIndexFromDbInTask();
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteFromCategoryProducts(CatalogFilterModel command)
        {
            Command(command, (id, c) =>
            {
                if (c.CategoryId != null)
                {
                    CategoryService.DeleteCategoryAndLink(id, c.CategoryId.Value);
                    ProductService.SetProductHierarchicallyEnabled(id);
                }
            });

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ActivateProducts(CatalogFilterModel command)
        {
            if (SaasDataService.IsSaasEnabled && ProductService.GetProductsCount("[Enabled] = 1") >= SaasDataService.CurrentSaasData.ProductsCount)
            {
                return JsonError("Ограничения тарифа аренды по товарам");
            }
            Command(command, (id, c) => ProductService.SetActive(id, true));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DisableProducts(CatalogFilterModel command)
        {
            Command(command, (id, c) => ProductService.SetActive(id, false));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeProductCategory(CatalogFilterModel command, int newCategoryId, bool removeFromCurrentCategories)
        {
            Command(command, (id, c) =>
            {
                if (removeFromCurrentCategories)
                {
                    foreach (var catId in ProductService.GetCategoriesIDsByProductId(id))
                        ProductService.DeleteProductLink(id, catId);
                }
                ProductService.AddProductLink(id, newCategoryId, 0, false);
            });

            CategoryService.SetCategoryHierarchicallyEnabled(newCategoryId);
            ProductService.PreCalcProductParamsMassInBackground();
            CategoryService.ClearCategoryCache();

            return JsonOk();
        }

        #endregion

        public JsonResult GetPriceRangeForPaging(CatalogFilterModel command)
        {
            var handler = new GetCatalog(command);
            var price =
                handler.GetItemsIds<CatalogRangeModel>("Max(Price) as Max, Min(Price) as Min")
                    .FirstOrDefault();

            return Json(new { from = price != null ? price.Min : 0, to = price != null ? price.Max : 10000000 });
        }

        public JsonResult GetAmountRangeForPaging(CatalogFilterModel command)
        {
            var handler = new GetCatalog(command);
            var amounts =
                handler.GetItemsIds<float>("(Select sum (Amount) from catalog.Offer where Offer.ProductID=Product.productID) as Amount");

            var min = amounts != null ? amounts.Min() : 0;
            var max = amounts != null ? amounts.Max() : 10000000;

            return Json(new { from = min, to = max });
        }

        public JsonResult GetBrandList(CatalogFilterModel command)
        {
            var brands = BrandService.GetBrands(true);
            return Json(brands.Select(x => new { label = x.Name, value = x.BrandId }));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult RecalculateProductsCount()
        {
            CategoryService.RecalculateProductsCountManual();
            CategoryService.SetCategoryHierarchicallyEnabled(0);
            ProductService.PreCalcProductParamsMassInBackground();
            CategoryService.ClearCategoryCache();

            return JsonOk();
        }

        [HttpGet]
        [Auth(RoleAction.Catalog, RoleAction.Orders, RoleAction.Crm)]
        public JsonResult GetOffersCatalog(CatalogFilterModel model)
        {
            return Json(new GetCatalogOffers(model).Execute());
        }

        [HttpGet]
        [Auth(RoleAction.Catalog, RoleAction.Orders, RoleAction.Crm)]
        public JsonResult GetCatalogIds(CatalogFilterModel command)
        {
            return Json(new { ids = new GetCatalog(command).GetItemsIds<int>("[Product].[ProductID]") });
        }

        [HttpGet]
        [Auth(RoleAction.Catalog, RoleAction.Orders, RoleAction.Crm)]
        public JsonResult GetCatalogOfferIds(CatalogFilterModel command)
        {
            return Json(new { ids = new GetCatalogOffers(command).GetItemsIds<int>("[Offer].[OfferId]") });
        }

        [HttpGet]
        public JsonResult GetCategoryName(int categoryId)
        {
            var category = CategoryService.GetCategory(categoryId);
            return Json(new { name = (category != null ? category.Name : null) });
        }
    }
}