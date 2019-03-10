using AdvantShop.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.FilePath;
using System.Linq;
using System;
using AdvantShop.Diagnostics;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.SEO;

namespace AdvantShop.Web.Admin.Handlers.Products
{
    public class CopyProduct
    {
        private readonly int _productId;
        private readonly string _name;

        public CopyProduct(int productId, string name)
        {
            _productId = productId;
            _name = name;
        }

        public int Execute()
        {
            var sourceProduct = ProductService.GetProduct(_productId);
            if (sourceProduct == null)
                return 0;

            var meta = MetaInfoService.GetMetaInfo(sourceProduct.ProductId, MetaType.Product);
            if (meta != null)
                meta.ObjId = 0;

            var product = new Product()
            {
                ProductId = 0,
                ArtNo = "",
                UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Product, StringHelper.Translit(_name ?? "")),

                Name = _name ?? "",
                BriefDescription = sourceProduct.BriefDescription,
                Description = sourceProduct.Description,
                Weight = sourceProduct.Weight,
                Height = sourceProduct.Height,
                Width = sourceProduct.Width,
                Length = sourceProduct.Length,

                Discount = new Discount(sourceProduct.Discount.Percent, sourceProduct.Discount.Amount, sourceProduct.Discount.Type),
                ShippingPrice = sourceProduct.ShippingPrice,
                Unit = sourceProduct.Unit,
                Multiplicity = sourceProduct.Multiplicity,
                MaxAmount = sourceProduct.MaxAmount,
                MinAmount = sourceProduct.MinAmount,

                Enabled = sourceProduct.Enabled,
                AllowPreOrder = sourceProduct.AllowPreOrder,

                BestSeller = sourceProduct.BestSeller,
                Recomended = sourceProduct.Recomended,
                New = sourceProduct.New,
                OnSale = sourceProduct.OnSale,
                BrandId = sourceProduct.BrandId,

                SalesNote = sourceProduct.SalesNote,
                Gtin = sourceProduct.Gtin,
                GoogleProductCategory = sourceProduct.GoogleProductCategory,
                Adult = sourceProduct.Adult,
                
                HasMultiOffer = sourceProduct.HasMultiOffer,

                TaxId = sourceProduct.TaxId,

                CurrencyID = sourceProduct.CurrencyID,
                Meta = meta,
                ModifiedBy = CustomerContext.CustomerId.ToString()
            };

            product.ProductId = ProductService.AddProduct(product, true);
            if (product.ProductId == 0)
                return 0;

            for (int i = 0; i < sourceProduct.Offers.Count; i++)
            {
                var offer = sourceProduct.Offers[i];
                offer.ArtNo = product.ArtNo + "-" + i;
                offer.ProductId = product.ProductId;

                if (OfferService.GetOffer(offer.ArtNo) == null)
                {
                    OfferService.AddOffer(offer);
                }
                else
                {
                    offer.ArtNo += "-" + i;

                    int count = 0;
                    while (count++ < 10)
                    {
                        if (OfferService.GetOffer(offer.ArtNo) == null)
                            break;

                        offer.ArtNo += "_" + i;
                    }
                    OfferService.AddOffer(offer);
                }
            }


            var categories = ProductService.GetCategoriesByProductId(sourceProduct.ProductId);
            if (categories != null && categories.Count > 0)
            {
                ProductService.EnableDynamicProductLinkRecalc();
                foreach (var category in categories)
                {
                    ProductService.AddProductLink(product.ProductId, category.CategoryId, 0, true, sourceProduct.MainCategory.CategoryId == category.CategoryId ? true : false);
                }
                ProductService.DisableDynamicProductLinkRecalc();
                ProductService.SetProductHierarchicallyEnabled(product.ProductId);
            }

            var propertyValues = PropertyService.GetPropertyValuesByProductId(sourceProduct.ProductId);
            foreach (var propertyValue in propertyValues)
            {
                PropertyService.AddProductProperyValue(propertyValue.PropertyValueId, product.ProductId);
            }

            var photos = PhotoService.GetPhotos(sourceProduct.ProductId, PhotoType.Product).ToList();
            if (photos.Count > 0)
            {
                try
                {
                    foreach (var photo in photos)
                    {
                        var tempPhotoName = PhotoService.AddPhoto(new Photo(0, product.ProductId, PhotoType.Product)
                        {
                            Description = photo.Description,
                            OriginName =
                                !string.IsNullOrWhiteSpace(photo.OriginName) ? photo.OriginName : photo.PhotoName,
                            ColorID = photo.ColorID,
                            Main = photo.Main,
                            PhotoSortOrder = photo.PhotoSortOrder
                        });

                        if (string.IsNullOrWhiteSpace(tempPhotoName)) continue;

                        var photoPath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Original, photo.PhotoName);
                        if (!System.IO.File.Exists(photoPath))
                        {
                            photoPath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, photo.PhotoName);
                        }

                        using (System.Drawing.Image image = System.Drawing.Image.FromFile(photoPath))
                        {
                            FileHelpers.SaveProductImageUseCompress(tempPhotoName, image);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            }

            var videos = ProductVideoService.GetProductVideos(sourceProduct.ProductId);
            foreach (var video in videos)
            {
                video.ProductId = product.ProductId;
                ProductVideoService.AddProductVideo(video);
            }

            var customOptions = CustomOptionsService.GetCustomOptionsByProductId(sourceProduct.ProductId);
            if (customOptions != null)
            {
                foreach (var customOption in customOptions)
                {
                    customOption.ProductId = product.ProductId;
                    CustomOptionsService.AddCustomOption(customOption);
                }
            }

            foreach (var relatedProduct in ProductService.GetRelatedProducts(sourceProduct.ProductId, RelatedType.Related))
            {
                ProductService.AddRelatedProduct(product.ProductId, relatedProduct.ProductId, RelatedType.Related);
            }

            foreach (var relatedProduct in ProductService.GetRelatedProducts(sourceProduct.ProductId, RelatedType.Alternative))
            {
                ProductService.AddRelatedProduct(product.ProductId, relatedProduct.ProductId, RelatedType.Alternative);
            }

            foreach (var gift in OfferService.GetProductGifts(sourceProduct.ProductId))
            {
                OfferService.AddProductGift(product.ProductId, gift.OfferId);
            }


            foreach (var type in AttachedModules.GetModules<IProductCopy>())
            {
                var module = (IProductCopy)Activator.CreateInstance(type);
                module.AfterCopyProduct(sourceProduct, product);
            }

            ProductService.PreCalcProductParams(product.ProductId);

            return product.ProductId;
        }
    }
}
