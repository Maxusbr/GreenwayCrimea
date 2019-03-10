using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Products;
using AdvantShop.Web.Admin.Models;
using AdvantShop.Web.Admin.Models.Products;
using AdvantShop.Web.Infrastructure.ActionResults;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Core.Caching;
using AdvantShop.Core;
using AdvantShop.Saas;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    public partial class ProductController : BaseAdminController
    {
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Add(string name, int categoryId)
        {
            return ProcessJsonResult(new AddProduct(name, categoryId));
        }

        public ActionResult Edit(int id)
        {
            var model = new GetProduct(id).Execute();
            if (model == null)
                return Error404();

            SetMetaInformation(T("Admin.Catalog.Index.ProductTitle") + model.Name);
            SetNgController(NgControllers.NgControllersTypes.ProductCtrl);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(AdminProductModel model)
        {
            foreach (var key in new List<string>() { "Discount", "Multiplicity", "Weight", "Width", "Length", "Height", "ShippingPrice" })
            {
                if (ModelState.ContainsKey(key))
                    ModelState[key].Errors.Clear();
            }

            if (model.Enabled && SaasDataService.IsSaasEnabled && ProductService.GetProductsCount("[Enabled] = 1") > SaasDataService.CurrentSaasData.ProductsCount)
            {
                ModelState.AddModelError("", "Ограничения тарифа аренды по товарам");
            }
            else if (ModelState.IsValid)
            {
                var result = new UpdateProduct(model).Execute();
                if (result)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));

                    // Нельзя использовать RedirectToAction, потому что у модулей вызывается action с POST. При редиректе он теряется.
                    return Edit(model.ProductId);
                }

                ModelState.AddModelError("", "Произошла ошибка при сохранении");
            }

            ShowErrorMessages();

            return RedirectToAction("Edit", new { id = model.ProductId });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteProduct(int productId)
        {
            ProductService.DeleteProduct(productId, true);
            CategoryService.RecalculateProductsCountManual();

            return Json(new { result = true });
        }

        #region Offers

        [HttpGet]
        public JsonResult GetOffers(int productId)
        {
            var offers = OfferService.GetProductOffers(productId).Select(x => new
            {
                x.OfferId,
                x.ProductId,
                x.ArtNo,
                x.BasePrice,
                x.SupplyPrice,
                x.Amount,
                ColorId = x.ColorID.ToString(),
                Color = x.Color != null ? x.Color.ToString() : "",
                SizeId = x.SizeID.ToString(),
                Size = x.Size != null ? x.Size.ToString() : "",
                x.Main
            });
            return Json(new { DataItems = offers });
        }

        [HttpGet]
        public JsonResult GetOffer(int offerId)
        {
            var offer = OfferService.GetOffer(offerId);
            if (offer == null)
                return Json(null);

            return Json(new
            {
                offer.OfferId,
                offer.ProductId,
                offer.ArtNo,
                offer.BasePrice,
                offer.SupplyPrice,
                offer.Amount,
                ColorId = offer.ColorID,
                SizeId = offer.SizeID,
                offer.Main
            });
        }

        [HttpGet]
        public JsonResult GetOffersValidation(int productId)
        {
            var error = "";
            var offers = OfferService.GetProductOffers(productId);

            if (offers.Any(o => o.ColorID != null) && offers.Any(o => o.ColorID == null))
            {
                error = T("Admin.Product.UpdateOffer.ColorIsNotNull");
            }

            if (offers.Any(o => o.SizeID != null) && offers.Any(o => o.SizeID == null))
            {
                error = T("Admin.Product.UpdateOffer.SizeIsNotNull");
            }

            if (offers.GroupBy(x => new { x.SizeID, x.ColorID }).Any(x => x.Count() > 1))
            {
                error = T("Admin.Product.UpdateOffer.Duplicate");
            }

            return Json(new CommandResult() { Result = error == "", Error = error });
        }

        [HttpGet]
        public JsonResult GetAvailableArtNo(int productId)
        {
            var p = ProductService.GetProduct(productId);
            if (p == null)
                return Json(Guid.NewGuid().ToString());

            var count = p.Offers.Count;
            for (int i = 1; i < 10; i++)
            {
                var artNo = p.ArtNo + "-" + (count + i);
                if (OfferService.GetOffer(artNo) == null)
                    return Json(artNo);
            }

            return Json(Guid.NewGuid().ToString());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddOffer(Offer offer)
        {
            return ProcessJsonResult(new AddUpdateOffer(offer));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateOffer(Offer offer)
        {
            return ProcessJsonResult(new AddUpdateOffer(offer));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteOffer(int offerId)
        {
            var offer = OfferService.GetOffer(offerId);
            if (offer == null)
                return Json(new { result = false });

            if (offer.Main)
            {
                var newMainOffer = OfferService.GetProductOffers(offer.ProductId).FirstOrDefault(x => !x.Main && x.OfferId != offer.OfferId);
                if (newMainOffer != null)
                {
                    newMainOffer.Main = true;
                    OfferService.UpdateOffer(newMainOffer);
                }
            }

            OfferService.DeleteOffer(offerId);

            ProductService.PreCalcProductParams(offer.ProductId);

            return JsonOk();
        }

        [HttpGet]
        public JsonResult GetColors()
        {
            var colors = ColorService.GetAllColors().Select(x => new SelectItemModel(x.ColorName, x.ColorId.ToString()));
            return Json(colors);
        }

        [HttpGet]
        public JsonResult GetSizes()
        {
            var sizes = SizeService.GetAllSizes().Select(x => new SelectItemModel(x.SizeName, x.SizeId.ToString()));
            return Json(sizes);
        }

        #endregion

        #region Photos

        [HttpGet]
        public JsonResult GetPhotos(int productId)
        {
            var photos = PhotoService.GetPhotos<ProductPhoto>(productId, PhotoType.Product).Select(x => new
            {
                x.PhotoId,
                x.Description,
                x.Main,
                x.PhotoSortOrder,
                x.PhotoName,
                ImageSrc = x.ImageSrcSmall(),
                ColorId = x.ColorID ?? 0
            });
            return Json(photos);
        }

        [HttpGet]
        public JsonResult GetPhoto(int photoId)
        {
            var photo = PhotoService.GetPhoto<ProductPhoto>(photoId, PhotoType.Product);

            return Json(new
            {
                photo.PhotoId,
                photo.Description,
                photo.Main,
                photo.PhotoSortOrder,
                photo.PhotoName,
                ImageSrc = photo.ImageSrcSmall(),
                ColorId = photo.ColorID ?? 0
            });
        }

        [HttpGet]
        public JsonResult GetPhotoColors(int productId)
        {
            var colors = SQLDataAccess.Query<Color>(
                "Select Color.ColorID, ColorName From Catalog.Photo inner join Catalog.Color on Color.ColorID=Photo.ColorID where objId=@productId and type='Product' " +
                "union " +
                "Select Color.ColorID, ColorName From Catalog.Color inner join catalog.Offer on offer.ColorID=Color.Colorid where productid=@productId " +
                "Order by ColorName", new { productId }).ToList();

            colors.Insert(0, new Color() { ColorName = "Нет цвета" });

            return Json(colors.Select(x => new { value = x.ColorId, label = x.ColorName }));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeletePhoto(int photoId)
        {
            PhotoService.DeleteProductPhoto(photoId);
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadPhotos(List<HttpPostedFileBase> files, int productId)
        {
            return Json(new UploadProductPictures(files, productId).Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadPictureByLink(int productId, string fileLink)
        {
            if (string.IsNullOrWhiteSpace(fileLink) || productId == 0)
                return Json(false);

            return Json(new UploadProductPicturesByLink(productId, fileLink).Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadPicturesByLink(int objId, List<string> fileLinks)
        {
            if (fileLinks == null || fileLinks.Count == 0 || objId == 0)
                return JsonError();

            foreach (var fileLink in fileLinks)
            {
                var result = new UploadProductPicturesByLink(objId, fileLink).Execute();
            }

            return JsonOk();
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult EditPhoto(int photoId, string alt, int? colorId)
        {
            var photo = PhotoService.GetPhoto(photoId);
            if (photo != null)
            {
                photo.Description = alt;
                photo.ColorID = colorId != 0 ? colorId : default(int?);

                PhotoService.UpdatePhoto(photo);
            }

            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangePhotoColor(int photoId, int colorId)
        {
            var photo = PhotoService.GetPhoto(photoId);
            if (photo != null)
            {
                photo.ColorID = colorId != 0 ? colorId : default(int?);

                PhotoService.UpdatePhoto(photo);
            }

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeMainPhoto(int photoId)
        {
            PhotoService.SetProductMainPhoto(photoId);
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangePhotoSortOrder(int productId, int photoId, int? prevPhotoId, int? nextPhotoId)
        {
            return Json(new ChangeProductPhotoSortOrder(productId, photoId, prevPhotoId, nextPhotoId).Execute());
        }

        #endregion

        #region Photos 360

        [HttpGet]
        public JsonResult GetPhotos360(int productId)
        {
            var photos = PhotoService.GetPhotos<ProductPhoto>(productId, PhotoType.Product360).Select(x => new
            {
                x.PhotoId,
                x.Description,
                x.Main,
                x.PhotoSortOrder,
                x.PhotoName,
                ImageSrc = x.ImageSrcRotate(),
                ColorId = x.ColorID
            }).FirstOrDefault();

            return Json(photos);
        }

        [HttpGet]
        public JsonResult GetActivityPhotos360(int productId)
        {
            var p = ProductService.GetProduct(productId);
            return Json(p.ActiveView360);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeletePhoto360(int productId)
        {
            foreach (var photo in PhotoService.GetPhotos<ProductPhoto>(productId, PhotoType.Product360))
            {
                PhotoService.DeleteProductPhoto(photo.PhotoId);
            }

            var p = ProductService.GetProduct(productId);
            p.ActiveView360 = false;
            ProductService.UpdateProduct(p, false);

            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadPhotos360(List<HttpPostedFileBase> files, int productId)
        {
            return Json(new UploadProductPictures360(files, productId).Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SetActivityPhotos360(int productId, bool isActive)
        {
            var p = ProductService.GetProduct(productId);
            p.ActiveView360 = isActive;
            ProductService.UpdateProduct(p, false);

            return JsonOk();
        }

        #endregion

        #region Categories

        [HttpGet]
        public JsonResult GetCategories(int productId)
        {
            var result = new GetProductCategories(productId).Execute().Select(x => new { label = x.Value, value = x.Key });
            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SetMainCategory(int productId, int categoryId)
        {
            ProductService.SetMainLink(productId, categoryId);
            ProductService.PreCalcProductParams(productId);
            ProductService.SetProductHierarchicallyEnabled(productId);
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCategory(int productId, int categoryId)
        {
            ProductService.DeleteProductLink(productId, categoryId);
            CategoryService.RecalculateProductsCountManual();
            ProductService.SetProductHierarchicallyEnabled(productId);
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddCategory(int productId, int categoryId)
        {
            ProductService.EnableDynamicProductLinkRecalc();
            ProductService.AddProductLink(productId, categoryId, 0, true);
            ProductService.DisableDynamicProductLinkRecalc();
            ProductService.SetProductHierarchicallyEnabled(productId);
            return Json(true);
        }

        #endregion

        #region Videos

        [HttpGet]
        public JsonResult GetVideos(int productId)
        {
            return Json(new { DataItems = ProductVideoService.GetProductVideos(productId) });
        }

        [HttpGet]
        public JsonResult GetVideo(int productVideoId)
        {
            return Json(ProductVideoService.GetProductVideo(productVideoId));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateVideo(ProductVideo productVideo)
        {
            var video = ProductVideoService.GetProductVideo(productVideo.ProductVideoId);
            if (video == null)
                return Json(false);

            video.Name = productVideo.Name.DefaultOrEmpty();
            video.PlayerCode = productVideo.PlayerCode.DefaultOrEmpty();
            video.Description = productVideo.Description.DefaultOrEmpty();
            video.VideoSortOrder = productVideo.VideoSortOrder;

            ProductVideoService.UpdateProductVideo(video);

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddVideo(ProductVideo productVideo, string link)
        {
            if (productVideo.ProductId == 0 || string.IsNullOrWhiteSpace(productVideo.Name))
                return Json(new { result = false });

            var playercode = productVideo.PlayerCode.DefaultOrEmpty();
            if (string.IsNullOrEmpty(playercode))
            {
                string error;
                playercode = ProductVideoService.GetPlayerCodeFromLink(link, out error);
                if (!string.IsNullOrEmpty(error))
                    return Json(new { result = false, error = error });
            }

            ProductVideoService.AddProductVideo(new ProductVideo
            {
                ProductId = productVideo.ProductId,
                Name = productVideo.Name.DefaultOrEmpty(),
                PlayerCode = playercode,
                Description = productVideo.Description.DefaultOrEmpty().Trim(),
                VideoSortOrder = productVideo.VideoSortOrder
            });

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteVideo(int productVideoId)
        {
            ProductVideoService.DeleteProductVideo(productVideoId);
            return JsonOk();
        }

        #endregion

        #region Brand

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeBrand(int productId, int brandId)
        {
            var product = ProductService.GetProduct(productId);
            if (product == null)
                return JsonError();

            var brand = BrandService.GetBrandById(brandId);
            if (brand == null)
                return JsonError();

            product.BrandId = brandId;
            ProductService.UpdateProduct(product, true);
            CacheManager.RemoveByPattern(CacheNames.MenuCatalog);

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteBrand(int productId)
        {
            var product = ProductService.GetProduct(productId);
            if (product == null)
                return JsonError();

            product.BrandId = 0;
            ProductService.UpdateProduct(product, true);
            CacheManager.RemoveByPattern(CacheNames.MenuCatalog);

            return JsonOk();
        }

        #endregion

        #region Tags

        public JsonResult GetTags(int productId)
        {
            return Json(new
            {
                tags = TagService.GetAutocompleteTags().Select(x => new { value = x.Name }),
                selectedTags = TagService.Gets(productId, ETagType.Product, true).Select(x => new { value = x.Name })
            });
        }

        #endregion

        #region Copy product

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CopyProduct(int productId, string name)
        {
            var handler = new CopyProduct(productId, name);
            var newProductId = handler.Execute();
            if (newProductId == 0)
                return Json(new { result = false, error = "При добавлении товара возникла ошибка" });

            return Json(new { result = true, productId = newProductId });
        }

        #endregion

        #region Export options

        [HttpGet]
        public JsonResult GetExportOptions(int productId)
        {
            var product = ProductService.GetProduct(productId);
            if (product == null)
                return Json(null);

            return Json(new ProductExportOptions()
            {
                ProductId = productId,
                SalesNote = product.SalesNote,
                Gtin = product.Gtin,
                GoogleProductCategory = product.GoogleProductCategory,
                YandexMarketCategory = product.YandexMarketCategory,
                YandexTypePrefix = product.YandexTypePrefix,
                YandexModel = product.YandexModel,
                Adult = product.Adult,
                ManufacturerWarranty = product.ManufacturerWarranty,
                YandexSizeUnit = product.YandexSizeUnit,
                Fee = product.Fee,
                Cbid = product.Cbid,
                YandexName = product.YandexName
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveExportOptions(ProductExportOptions exportOptions)
        {
            var product = ProductService.GetProduct(exportOptions.ProductId);
            if (product == null)
                return Json(new { result = false });

            product.SalesNote = exportOptions.SalesNote;
            product.Gtin = exportOptions.Gtin;
            product.GoogleProductCategory = exportOptions.GoogleProductCategory;
            product.YandexMarketCategory = exportOptions.YandexMarketCategory;
            product.YandexTypePrefix = exportOptions.YandexTypePrefix;
            product.YandexModel = exportOptions.YandexModel;
            product.Adult = exportOptions.Adult;
            product.ManufacturerWarranty = exportOptions.ManufacturerWarranty;
            product.Fee = exportOptions.Fee;
            product.Cbid = exportOptions.Cbid;
            product.YandexSizeUnit = exportOptions.YandexSizeUnit;
            product.YandexName = exportOptions.YandexName;

            ProductService.UpdateProduct(product, true);

            return Json(new { result = true });
        }

        #endregion

        #region Properties

        [HttpGet]
        public JsonResult GetProperties(int productId)
        {
            var model = new GetProductProperties(productId).Execute();
            return Json(model);
        }

        [HttpGet]
        public JsonResult GetPropertyValues(int propertyId)
        {
            var items =
                PropertyService.GetValuesByPropertyId(propertyId)
                    .Select(x => new SelectItemModel(x.Value, x.PropertyValueId.ToString()));

            return Json(items);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddPropertyValue(int productId, int propertyId, int? propertyValueId, string value, bool isNew)
        {
            var propValueId = new AddPropertyValue(productId, propertyId, propertyValueId, value, isNew).Execute();

            return Json(new { result = propValueId != 0, propertyValueId = propValueId });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeletePropertyValue(int productId, int propertyValueId)
        {
            PropertyService.DeleteProductPropertyValue(productId, propertyValueId);
            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddPropertyWithValue(int productId, int? propertyId, int? propertyValueId, string propertyName, string propertyValue)
        {
            var propValueId = new AddPropertyWithValue(productId, propertyId, propertyValueId, propertyName, propertyValue).Execute();

            return Json(new { result = propValueId != 0, propertyValueId = propValueId });
        }

        #endregion

        #region Related products

        [HttpGet]
        public JsonResult GetRelatedProducts(int productId, RelatedType type)
        {
            var products = ProductService.GetAllRelatedProducts(productId, type).Select(x => new
            {
                x.ProductId,
                x.Name,
                x.ArtNo,
                Url = Url.Action("Edit", "Product", new { id = x.ProductId }),
                ImageSrc = x.Photo.ImageSrcSmall()
            });

            return Json(products);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteRelatedProduct(int productId, RelatedType type, int relatedProductId)
        {
            ProductService.DeleteRelatedProduct(productId, relatedProductId, type);
            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddRelatedProduct(int productId, RelatedType type, List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return Json(new { result = false });

            foreach (var id in ids)
                ProductService.AddRelatedProduct(productId, id, type);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeRelatedProductSortOrder(int productId, RelatedType type, int relProductId, int? prevProductId, int? nextProductId)
        {
            // TODO: add sorting in related products
            var handler = new ChangeRelatedProductSortOrder(productId, type, relProductId, prevProductId, nextProductId);
            var result = handler.Execute();

            return Json(result);
        }

        #endregion

        #region Gifts

        [HttpGet]
        public JsonResult GetGifts(int productId)
        {
            var products = OfferService.GetProductGifts(productId).Select(x => new
            {
                x.ProductId,
                x.OfferId,
                x.Product.Name,
                x.ArtNo,
                Url = Url.Action("Edit", "Product", new { id = x.ProductId }),
                ImageSrc = x.Photo.ImageSrcSmall()
            });

            return Json(products);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteGift(int productId, int offerId)
        {
            OfferService.DeleteProductGift(productId, offerId);
            ProductService.PreCalcProductParams(productId);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddGifts(int productId, List<int> offerIds)
        {
            if (offerIds == null || offerIds.Count == 0)
                return Json(new { result = false });

            foreach (var offerId in offerIds)
                OfferService.AddProductGift(productId, offerId);

            ProductService.PreCalcProductParams(productId);

            return Json(new { result = true });
        }

        #endregion

        #region AddProductList

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddProductList(int categoryId, List<string> products)
        {
            foreach (var productName in products)
            {
                if (string.IsNullOrWhiteSpace(productName))
                    continue;

                try
                {
                    new AddProduct(productName, categoryId).Execute();
                }
                catch (BlException e)
                {
                    ModelState.AddModelError(e.Property, e.Message);
                    return JsonError();
                }
            }

            return JsonOk();
        }

        #endregion
    }
}
