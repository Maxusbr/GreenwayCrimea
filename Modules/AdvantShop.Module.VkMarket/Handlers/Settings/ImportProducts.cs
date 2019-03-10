using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.FullSearch;
using AdvantShop.Helpers;
using AdvantShop.Module.VkMarket.Domain;
using AdvantShop.Module.VkMarket.Models.Settings;
using AdvantShop.Module.VkMarket.Services;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Infrastructure.Handlers;
using VkNet;
using VkNet.Model;

namespace AdvantShop.Module.VkMarket.Handlers.Settings
{
    public class ImportProducts : AbstractCommandHandler<bool>
    {
        private readonly ImportSettingsModel _model;
        private readonly VkCategoryService _categoryService;
        private readonly VkProductService _productService;
        private readonly VkMarketApiService _apiService;

        public ImportProducts(ImportSettingsModel model)
        {
            _model = model;
            _categoryService = new VkCategoryService();
            _productService = new VkProductService();
            _apiService = new VkMarketApiService();
        }

        protected override bool Handle()
        {
            Task.Run(() => Import());
            return true;
        }

        private void Import()
        {
            try
            {
                var vk = _apiService.Auth();

                var groupId = -VkMarketSettings.Group.Id;
                var marketCategories = _apiService.GetMarketCategories();
                var marketCategoryId = marketCategories != null && marketCategories.Count > 0
                    ? marketCategories[0].Id
                    : 0;
                var currencyId = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3).CurrencyId;

                var albums = _apiService.GetAllAlbums(vk);

                ImportProgress.Start(albums != null ? albums.Count + 1 : 1);

                if (albums != null && albums.Count > 0)
                {
                    var sortOrder = 0;

                    foreach (var album in albums)
                    {
                        try
                        {
                            VkCategory vkCategory = null;
                            var categoryId = GetOrAddCategory(vk, groupId, album, sortOrder, marketCategoryId, out vkCategory);

                            foreach (var product in _apiService.GetProducts(vk, groupId, album.Id))
                            {
                                AddProduct(product, categoryId, currencyId, vkCategory.VkId);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log.Error("VkMarket.ImportProducts Альбом: " + album.Title, ex);
                        }
                        sortOrder += 10;
                        ImportProgress.Inc();
                    }
                }

                foreach (var product in _apiService.GetProducts(vk, groupId, null))
                {
                    AddProduct(product, CategoryService.DefaultNonCategoryId, currencyId, 0);
                }
                ImportProgress.Inc();

                LuceneSearch.CreateAllIndexInBackground();
                ProductService.PreCalcProductParamsMassInBackground();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        private int GetOrAddCategory(VkApi vk, long groupId, MarketAlbum album, int sortOrder, long marketCategoryId, out VkCategory vkCategory)
        {
            vkCategory = _categoryService.GetByVkId(album.Id.Value);
            if (vkCategory != null)
            {
                var cats = _categoryService.GetLinkedCategories(vkCategory.Id);
                if (cats != null && cats.Count > 0)
                    return cats[0].CategoryId;
            }

            var category = new Category()
            {
                Name = album.Title,
                UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Category, album.Title.Trim()),
                Enabled = true,
                DisplayStyle = ECategoryDisplayStyle.Tile,
                Sorting = ESortOrder.NoSorting,
                SortOrder = sortOrder
            };
            CategoryService.AddCategory(category, true);

            if (album.Photo != null)
            {
                var photoUrl = album.Photo.BigPhotoSrc ?? album.Photo.Photo1280 ?? album.Photo.Photo807 ?? album.Photo.Photo130;
                if (photoUrl != null)
                {
                    AddPhoto(category.CategoryId, photoUrl.ToString(), PhotoType.CategorySmall);
                }
            }

            var firstProduct = _apiService.GetFirstProduct(vk, groupId, album.Id);

            vkCategory = new VkCategory()
            {
                VkId = album.Id.Value,
                Name = album.Title,
                SortOrder = sortOrder,
                VkCategoryId = 
                    firstProduct != null && firstProduct.MarketCategory != null && firstProduct.MarketCategory.Id != null
                        ? firstProduct.MarketCategory.Id.Value
                        : marketCategoryId
            };
            _categoryService.Add(vkCategory);
            _categoryService.AddLink(category.CategoryId, vkCategory.Id);
            
            return category.CategoryId;
        }

        private void AddProduct(Market market, int categoryId, int currencyId, long vkId)
        {
            var p = _productService.Get(market.Id);

            if (p != null)
            {
                // update?
                return;
            }

            var description = market.Description.Replace("\r\n", "<br/> ");

            var product = new Product()
            {
                Name = market.Title,
                Description = description,
                BriefDescription = description,
                ArtNo = null,
                UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Product, market.Title),
                CurrencyID = currencyId,
                Multiplicity = 1,
                Offers = new List<Offer>()
                {
                    new Offer()
                    {
                        ArtNo = null,
                        Amount = 1,
                        BasePrice = Convert.ToSingle(market.Price.Amount/100),
                        Main = true,
                    }
                },
                Enabled = true,
                Meta = null,
                ModifiedBy = "Модуль Товары из ВКонтакте"
            };

            var productId = ProductService.AddProduct(product, false);

            if (productId != 0 && categoryId != 0 && categoryId != CategoryService.DefaultNonCategoryId)
            {
                ProductService.EnableDynamicProductLinkRecalc();
                ProductService.AddProductLink(productId, categoryId, 0, true);
                ProductService.DisableDynamicProductLinkRecalc();
                ProductService.SetProductHierarchicallyEnabled(product.ProductId);
            }

            var photos = market.Photos;
            var hasPhoto = photos != null && photos.Count > 0;
            if (hasPhoto)
            {
                foreach (var photo in photos)
                {
                    var photoUrl = photo.BigPhotoSrc ?? photo.Photo1280 ?? photo.Photo807 ?? photo.Photo130;
                    if (photoUrl != null)
                    {
                        AddPhoto(productId, photoUrl.ToString(), PhotoType.Product);
                    }
                }
            }

            _productService.Add(new VkProduct()
            {
                Id = market.Id,
                ProductId = productId,
                AlbumId = vkId,
                MainPhotoId = hasPhoto && photos[0].Id != null ? photos[0].Id.Value : 0,
                PhotoIdsList = hasPhoto ? photos.Where(x => x.Id != null).Select(x => x.Id.Value) : null
            });
        }

        private bool AddPhoto(int objId, string fileLink, PhotoType type)
        {
            try
            {
                var photo = new Photo(0, objId, type) { OriginName = fileLink };
                var photoName = PhotoService.AddPhoto(photo);
                var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                if (!string.IsNullOrWhiteSpace(photoName))
                {
                    if (FileHelpers.DownloadRemoteImageFile(fileLink, photoFullName))
                    {
                        using (var image = Image.FromFile(photoFullName))
                        {
                            if (type == PhotoType.Product)
                                FileHelpers.SaveProductImageUseCompress(photoName, image);

                            if (type == PhotoType.CategorySmall)
                                FileHelpers.SaveResizePhotoFile(
                                    FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, photoName),
                                    SettingsPictureSize.SmallCategoryImageWidth,
                                    SettingsPictureSize.SmallCategoryImageHeight,
                                    image);
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return false;
        }

    }
}
