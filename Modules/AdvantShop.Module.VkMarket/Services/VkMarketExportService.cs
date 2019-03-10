using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Module.VkMarket.Domain;
using AdvantShop.Module.VkMarket.Models.Settings;
using AdvantShop.Repository.Currencies;
using log4net;
using VkNet;
using VkNet.Exception;

namespace AdvantShop.Module.VkMarket.Services
{
    public class VkMarketExportService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(VkMarketExportService));

        private readonly VkMarketApiService _apiService;
        private readonly VkCategoryService _categoryService;
        private readonly VkProductService _productService;

        private readonly CurrentMarketExportSettings _settings;
        
        #region Constructor

        public VkMarketExportService()
        {
            _apiService = new VkMarketApiService();

            _categoryService = new VkCategoryService();
            _productService = new VkProductService();
            

            _settings = new CurrentMarketExportSettings()
            {
                //ExportOffers = VkMarketExportSettings.ExportOffers,
                ExportUnavailableProducts = VkMarketExportSettings.ExportUnavailableProducts,
                AddSizeAndColorInDescription = VkMarketExportSettings.AddSizeAndColorInDescription,
                AddSizeAndColorInName = VkMarketExportSettings.AddSizeAndColorInName,
                ShowDescription = VkMarketExportSettings.ShowDescription,
                AddLinkToSite = VkMarketExportSettings.AddLinkToSite,
                TextBeforeLinkToSite = VkMarketExportSettings.TextBeforeLinkToSite,
                OwnerId = -VkMarketSettings.Group.Id,
                SiteUrl = SettingsMain.SiteUrl.TrimEnd('/'),
                Currency = CurrencyService.GetCurrencyByIso3(VkMarketSettings.CurrencyIso3 ?? CurrencyService.CurrentCurrency.Iso3),
                ShowProperties = VkMarketExportSettings.ShowProperties
            };
        }

        #endregion

        public bool StartExport()
        {
            if (VkMarketExportState.IsRun)
                return false;

            VkMarketExportState.Start();
            var result = Export();
            VkMarketExportState.Stop();

            return result;
        }

        private bool Export()
        {
            try
            {
                var vk = _apiService.Auth();
                if (vk == null)
                {
                    VkMarketExportState.WriteLog("Не удалось авторизоваться в vk. Пересоздайте подключение к ВКонтакте заново.");
                    return false;
                }

                var categories = _categoryService.GetList();

                ExportProgress.Start(categories.Count);

                foreach (var vkCategory in categories)
                {
                    var ids = new List<long>();

                    foreach (var product in GetProducts(vkCategory.Id)) // _settings.ExportOffers ? GetProductsByOffers(vkCategory.Id) :
                    {
                        if (!VkMarketExportState.IsRun)
                            return false;

                        try
                        {
                            ExportProduct(product, vk, vkCategory);
                        }
                        catch(VkApiException vkEx)
                        {
                            _logger.Error(vkEx);
                            VkMarketExportState.WriteLog("Ошибка при экспорте товара {0}: {1}", product.ProductArtNo, vkEx.Message);

                            if (vkEx.Message != null && vkEx.Message.Contains("Flood control: too much captcha requests"))
                                return false;
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex);
                            VkMarketExportState.WriteLog("Ошибка при экспорте товара {0}: {1}", product.ProductArtNo, ex.Message);
                        }

                        if (product.VkProductId != 0)
                            ids.Add(product.VkProductId);
                    }

                    _productService.DeleteByAlbumAndNotInList(vk, vkCategory.Id, ids);
                    ExportProgress.Inc();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                VkMarketExportState.WriteLog("Ошибка при экспорте");

                return false;
            }
            finally
            {
                FileHelpers.DeleteFilesFromImageTempInBackground();
            }

            return true;
        }

        #region GetProducts

        private IEnumerable<ExportProductModel> GetProductsByOffers(int vkCategoryId)
        {
            return SQLDataAccess.Query<ExportProductModel>(
                "Select " +
                "p.ProductId, " +
                "p.Name, " +
                "p.UrlPath, " +
                "p.Description, " +
                "p.BriefDescription, " +
                "p.ArtNo as ProductArtNo, " +
                "p.Discount, " +
                "p.DiscountAmount, " +
                "p.AllowPreOrder, " +
                "p.Enabled, " +

                "o.OfferId, " +
                "o.ColorID, " +
                "o.SizeID, " +
                "o.ArtNo as OfferArtNo, " +
                "o.Price, " +
                "o.Main, " +

                "ColorName, " +
                "SizeName, " +
                "CurrencyValue, " +

                "VkProduct.Id as VkProductId, " +
                "VkProduct.MainPhotoId as VkMainPhotoId, " +
                "VkProduct.PhotoIds as VkPhotoIds " +

                "From Catalog.Product p " +
                "Inner Join Catalog.Offer o On o.ProductID = p.ProductID " +
                "Inner Join Catalog.ProductCategories pc On pc.ProductID = p.ProductID " + //  and pc.Main = 1
                "Left Join Catalog.Category cat On cat.CategoryId = pc.CategoryId " +

                "Left Join Catalog.Color On Color.ColorID = o.ColorID " +
                "Left Join Catalog.Size On Size.SizeID = o.SizeID " +
                "Left Join Module.VkProduct On VkProduct.ProductId = p.ProductId " +
                "Inner Join Catalog.Currency On Currency.CurrencyID = p.CurrencyID " +

                "Where (p.Enabled = 1 Or @exportNotAvailable = 1) " +
                "AND (o.Price > 0 OR @exportNotAvailable = 1) " +
                "AND (o.Amount > 0 OR p.AllowPreOrder = 1 OR @exportNotAvailable = 1) " +
                
                "AND pc.CategoryId in (Select CategoryId From Module.VkCategory_Category Where VkCategoryId=@vkCategoryId) " +

                "Order By cat.SortOrder, pc.SortOrder, o.Main desc",

                new { vkCategoryId, exportNotAvailable = _settings.ExportUnavailableProducts });
        }

        private IEnumerable<ExportProductModel> GetProducts(int vkCategoryId)
        {
            return SQLDataAccess.Query<ExportProductModel>(
                "Select " +
                "p.ProductId, " +
                "p.Name, " +
                "p.UrlPath, " +
                "p.Description, " +
                "p.BriefDescription, " +
                "p.ArtNo as ProductArtNo, " +
                "p.Discount, " +
                "p.DiscountAmount, " +
                "p.AllowPreOrder, " +
                "p.Enabled, " +

                "o.OfferId, " +
                "o.ColorID, " +
                "o.SizeID, " +
                "o.ArtNo as OfferArtNo, " +
                "o.Price, " + // "pExt.MinPrice as Price, "
                "o.Main, " +

                "ColorName, " +
                "SizeName, " +
                "CurrencyValue, " +

                "VkProduct.Id as VkProductId, " +
                "VkProduct.MainPhotoId as VkMainPhotoId, " +
                "VkProduct.PhotoIds as VkPhotoIds " +

                "From Catalog.Product p " +
                "Left Join Catalog.ProductExt pExt on p.ProductId = pExt.ProductId " +
                "Left Join Catalog.Offer o On o.ProductID = pExt.ProductID " +
                "Inner Join Catalog.ProductCategories pc On pc.ProductID = p.ProductID " +
                "Left Join Catalog.Category cat On cat.CategoryId = pc.CategoryId " +

                "Left Join Catalog.Color On Color.ColorID = o.ColorID " +
                "Left Join Catalog.Size On Size.SizeID = o.SizeID " +
                "Left Join Module.VkProduct On VkProduct.ProductId = p.ProductId " +
                "Inner Join Catalog.Currency On Currency.CurrencyID = p.CurrencyID " +

                "Where (p.Enabled = 1 Or @exportNotAvailable = 1) " +
                "AND (o.Main = 1 Or o.Main is null) " +
                "AND (o.Price > 0) " + //  OR @exportNotAvailable = 1
                "AND (o.Amount > 0 OR p.AllowPreOrder = 1 OR @exportNotAvailable = 1) " +

                "AND pc.CategoryId in (Select CategoryId From Module.VkCategory_Category Where VkCategoryId=@vkCategoryId) " +

                "Order By cat.SortOrder, pc.SortOrder",

                new { vkCategoryId, exportNotAvailable = _settings.ExportUnavailableProducts });
        }

        #endregion

        private void ExportProduct(ExportProductModel product, VkApi vk, VkCategory vkCategory)
        {
            var name = product.Name +
                        (_settings.AddSizeAndColorInName
                            ? (!string.IsNullOrEmpty(product.ColorName) ? " " + product.ColorName : "") +
                              (!string.IsNullOrEmpty(product.SizeName) ? " " + product.SizeName : "")
                            : "");

            if (name.Length > 100)
                name = name.Substring(0, 100);

            var price = GetPrice(product);
            var description = GetDescription(product);

            var vkProduct = new VkProduct()
            {
                Id = product.VkProductId,
                OwnerId = _settings.OwnerId,
                ProductId = product.ProductId,
                Name = name,
                Description = description,
                Price = price > 0 ? price : 0.01m,
                CategoryId = vkCategory.VkCategoryId,
                Deleted = false,
                AlbumId = vkCategory.VkId
            };

            if (product.VkProductId == 0)
            {
                var vp = _productService.GetIdByProductId(product.ProductId);
                if (vp != 0)
                    return;

                var photoIds = ExportPhotos(product, vk);
                if (photoIds == null || photoIds.Count == 0)
                {
                    VkMarketExportState.WriteLog("У товара {0} нет ни одной фотографии, поэтому он не будет загружен", product.ProductArtNo);
                    return;
                }

                vkProduct.MainPhotoId = photoIds[0];
                vkProduct.PhotoIdsList = photoIds.Count > 1 ? photoIds.Skip(1) : null;

                product.VkProductId = _apiService.AddProduct(vk, vkProduct);

                _productService.Add(vkProduct);
            }
            else
            {
                vkProduct.MainPhotoId = product.VkMainPhotoId;
                vkProduct.PhotoIdsList =
                    !string.IsNullOrEmpty(product.VkPhotoIds)
                        ? product.VkPhotoIds.Split(',').Select(x => x.TryParseLong()).Where(x => x != 0)
                        : null;

                _apiService.UpdateProduct(vk, vkProduct);
            }
        }

        #region Photo

        public List<long> ExportPhotos(ExportProductModel product, VkApi vk)
        {
            var photos = PhotoService.GetPhotos<ProductPhoto>(product.ProductId, PhotoType.Product);
            if (photos.Count == 0)
                return null;

            var photo = photos.FirstOrDefault(x => x.ColorID == product.ColorId) ?? photos.FirstOrDefault();
            if (photo == null)
                return null;

            var photoIds = new List<long>();

            try
            {
                var groupId = _settings.OwnerId < 0 ? -_settings.OwnerId : _settings.OwnerId;

                var photoId = AddPhoto(photo, true, vk, groupId);
                if (photoId != 0)
                    photoIds.Add(photoId);

                foreach (var ph in photos.Where(x => x.PhotoId != photo.PhotoId)
                                         .OrderByDescending(x => x.ColorID == product.ColorId)
                                         .ThenByDescending(x => x.ColorID != null)
                                         .Take(4))
                {
                    photoId = AddPhoto(ph, false, vk, groupId);
                    if (photoId != 0)
                        photoIds.Add(photoId);
                }
            }
            catch (VkApiException ex)
            {
                VkMarketExportState.WriteLog(product.ProductArtNo + " " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return photoIds;
        }

        private long AddPhoto(ProductPhoto photo, bool mainPhoto, VkApi vk, long groupId)
        {
            var filePath = GetAbsolutePhotoPath(photo);
            if (filePath == null)
                return 0;

            var vkPhoto = _apiService.AddPhoto(vk, groupId, mainPhoto, filePath);

            return vkPhoto == null || vkPhoto.Id == null ? 0 : vkPhoto.Id.Value;
        }

        private string GetAbsolutePhotoPath(ProductPhoto photo)
        {
            var photoName = photo.PhotoName;
            var photoPath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, photo.PhotoName);

            if (photo.PhotoName.Contains("://")) // http://cs71.advantshop.net/15705.jpg
            {
                photoName = photoPath.Split('/').LastOrDefault();
                photoPath = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                var name = photo.PhotoName.Split('/').LastOrDefault();
                var url = photo.PhotoName.Replace(name, "") + "pictures/product/big/" + name.Replace(".", "_big.");

                if (!FileHelpers.DownloadRemoteImageFile(url, photoPath))
                    return null;

                photoName = photoName.Replace(".", "_tmp.");
            }
            
            var tempPhoto = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);
            FileHelpers.DeleteFile(tempPhoto);

            using (var image = Image.FromFile(photoPath))
            {
                // если одна из сторон < 400px или соотношение сторон больше чем 1:12
                if (image.Width < 400 || image.Height < 400 || image.Width / image.Height >= 12 || image.Height / image.Width >= 12)
                {
                    if (Resize(image, tempPhoto))
                        return tempPhoto;

                    return null;
                }
            }

            return photoPath;
        }

        private bool Resize(Image image, string resultPath)
        {
            var max = image.Width > image.Height ? image.Width : image.Height;
            if (max < 400)
                max = 400;

            try
            {
                using (var img = new Bitmap(image))
                using (var result = new Bitmap(max, max))
                {
                    result.MakeTransparent();
                    using (var graphics = Graphics.FromImage(result))
                    {
                        graphics.Clear(System.Drawing.Color.White);
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.DrawImage(img, (max - img.Width) / 2, (max - img.Height) / 2, img.Width, img.Height);

                        graphics.Flush();
                        using (var stream = new FileStream(resultPath, FileMode.CreateNew))
                        {
                            result.Save(stream, ImageFormat.Jpeg);
                            stream.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
            return true;
        }

        #endregion
        
        #region Description

        private string GetDescription(ExportProductModel product)
        {
            var description = "";

            if (_settings.AddSizeAndColorInDescription)
            {
                var offers = OfferService.GetProductOffers(product.ProductId)
                    .Where(x => (x.BasePrice > 0 && x.Amount > 0) || product.AllowPreOrder || _settings.ExportUnavailableProducts)
                    .ToList();

                if (offers.Count > 0)
                {
                    var colors = offers.Where(x => x.ColorID != null && x.Color != null).Select(x => x.Color.ColorName).Distinct().ToList();
                    if (colors.Count > 0)
                        description += SettingsCatalog.ColorsHeader + ": " + string.Join(", ", colors) + "\r\n";

                    var sizes = offers.Where(x => x.SizeID != null && x.Size != null).Select(x => x.Size.SizeName).Distinct().ToList();
                    if (sizes.Count > 0)
                        description += SettingsCatalog.SizesHeader + ": " + string.Join(", ", sizes) + "\r\n";
                }
            }

            if (_settings.AddLinkToSite == AddLinkToSiteMode.Top)
                description += _settings.TextBeforeLinkToSite + " " + GetLink(product) + "\r\n";

            switch (_settings.ShowDescription)
            {
                case ShowDescriptionMode.Full:
                    description += RemoveHtml(product.Description);
                    break;
                case ShowDescriptionMode.Short:
                    description += RemoveHtml(product.BriefDescription);
                    break;
            }

            if (_settings.ShowProperties)
                description += GetProperties(product);

            if (_settings.AddLinkToSite == AddLinkToSiteMode.Bottom || description.Length < 10)
                description += "\r\n" + _settings.TextBeforeLinkToSite + " " + GetLink(product) + "\r\n";
            
            if (description.Length > 3600)
                description = description.Substring(0, 3600);

            return description;
        }

        private string RemoveHtml(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return "";

            return
                StringHelper.RemoveHTML(
                    val.Replace("\r\n", "")
                        .Replace("</div>", "</div>\r\n")
                        .Replace("</p>", "</p>\r\n")
                        .Replace("</li>", "</li>\r\n"));
        }

        private string GetLink(ExportProductModel product)
        {
            var suffix = "";
            if (!product.Main)
            {
                if (product.ColorId != 0)
                    suffix = "?color=" + product.ColorId;

                if (product.SizeId != 0)
                    suffix += (!string.IsNullOrEmpty(suffix) ? "&" : "?") + "size=" + product.SizeId;
            }
            return _settings.SiteUrl + "/products/" + product.UrlPath + suffix;
        }

        private string GetProperties(ExportProductModel product)
        {
            var properties = PropertyService.GetPropertyValuesByProductId(product.ProductId);
            if (properties != null)
            {
                var sb = new StringBuilder();

                foreach (var propertyValue in properties.Where(x => x.Property.UseInDetails))
                {
                    sb.AppendFormat("{0}: {1}\r\n", propertyValue.Property.Name, propertyValue.Value);
                }
                return sb.ToString();
            }
            return "";
        }

        #endregion

        private decimal GetPrice(ExportProductModel product)
        {
            var discountPrice = product.Discount != 0 ? product.Price * product.Discount / 100 : product.DiscountAmount;
            var price = product.Price - discountPrice;

            return (decimal)PriceService.RoundPrice(price, _settings.Currency, product.CurrencyValue);
        }

    }
}
