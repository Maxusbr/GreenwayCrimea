//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

namespace AdvantShop.FilePath
{
    public enum ProductImageType
    {
        Big,
        Middle,
        Small,
        XSmall,
        Original,
        Rotate
    }

    public enum CategoryImageType
    {
        Big,
        Small,
        Icon
    }

    public enum ColorImageType
    {
        Details,
        Catalog,
    }

    public enum FolderType
    {
        Pictures,
        MenuIcons,
        Product,
        Color,
        Carousel,
        Category,
        News,
        StaticPage,
        BrandLogo,
        PaymentLogo,
        ShippingLogo,
        PriceTemp,
        ImageTemp,
        OneCTemp,
        ManagerPhoto,
        ReviewImage,
        Combine,
        AdminContent,
        Avatar,
        TaskAttachment,
        LeadAttachment,
        UserFiles,
    }

    public class FoldersHelper
    {
        public static readonly Dictionary<FolderType, string> PhotoFoldersPath = new Dictionary<FolderType, string>
        {
            {FolderType.Pictures, "pictures/"},
            {FolderType.MenuIcons, "pictures/icons/"},
            {FolderType.Product, "pictures/product/"},
            {FolderType.Color, "pictures/color/"},
            {FolderType.Carousel, "pictures/carousel/"},
            {FolderType.News, "pictures/news/"},
            {FolderType.Category, "pictures/category/"},
            {FolderType.BrandLogo, "pictures/brand/"},
            {FolderType.ManagerPhoto, "pictures/manager/"},
            {FolderType.PaymentLogo, "pictures/payment/"},
            {FolderType.ShippingLogo, "pictures/shipping/"},
            {FolderType.StaticPage, "pictures/staticpage/"},
            {FolderType.ReviewImage, "pictures/review/"},
            {FolderType.PriceTemp, "content/price_temp/"},
            {FolderType.ImageTemp, "content/upload_images/"},
            {FolderType.OneCTemp, "content/1c_temp/"},
            {FolderType.Combine, "combine/"},
            {FolderType.AdminContent, "areas/admin/content/"},
            {FolderType.Avatar, "pictures/avatar/"},
            {FolderType.TaskAttachment, "content/attachments/tasks/"},
            {FolderType.LeadAttachment, "content/attachments/leads/"},
            {FolderType.UserFiles, "userfiles/" },
        };

        public static readonly Dictionary<CategoryImageType, string> CategoryPhotoPrefix = new Dictionary<CategoryImageType, string>
        {
            {CategoryImageType.Small, "small/"},
            {CategoryImageType.Big, ""},
            {CategoryImageType.Icon, "icon/"},
        };

        public static readonly Dictionary<ColorImageType, string> ColorPhotoPrefix = new Dictionary<ColorImageType, string>
        {
            {ColorImageType.Details, "details/"},
            {ColorImageType.Catalog, "catalog/"},
        };

        public static readonly Dictionary<ProductImageType, string> ProductPhotoPrefix = new Dictionary<ProductImageType, string>
        {
            {ProductImageType.XSmall, "xsmall/"},
            {ProductImageType.Small,  "small/"},
            {ProductImageType.Middle, "middle/"},
            {ProductImageType.Big,    "big/"},
            {ProductImageType.Original, "original/"},
            {ProductImageType.Rotate, "rotate/"}
        };

        public static readonly Dictionary<ProductImageType, string> ProductPhotoPostfix = new Dictionary<ProductImageType, string>
        {
            {ProductImageType.XSmall, "_xsmall"},
            {ProductImageType.Small,  "_small"},
            {ProductImageType.Middle, "_middle"},
            {ProductImageType.Big,    "_big"},
            {ProductImageType.Original, "_original"},
            {ProductImageType.Rotate, "_rotate"}

        };

        private static string GetPath(string imagePathBase, bool isForAdministration, string remoteUrl)
        {
            if (!string.IsNullOrWhiteSpace(remoteUrl))
            {
                return remoteUrl + imagePathBase;
            }

            if (HttpContext.Current == null)
                return (isForAdministration ? "../" : SettingsMain.SiteUrl.Trim('/') + '/') + imagePathBase;

            return UrlService.GetUrl() + imagePathBase;
        }

        private static string GetPathAbsolut(string imagePathBase)
        {
            return SettingsGeneral.AbsolutePath + imagePathBase;
        }

        //_____________
        public static string GetPath(FolderType type, string photoPath, bool isForAdministration)
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return GetPath(PhotoFoldersPath[type], isForAdministration, string.Empty);

            if (photoPath.Contains("://"))
            {
                var fileName = Path.GetFileName(photoPath);
                var path = photoPath.Replace(fileName, "");
                return GetPath(PhotoFoldersPath[type], isForAdministration, path) + fileName;
            }

            return GetPath(PhotoFoldersPath[type], isForAdministration, string.Empty) + photoPath;
        }

        public static string GetPathRelative(FolderType type, string photoPath, bool isForAdministration)
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return (isForAdministration ? "../" : string.Empty) + PhotoFoldersPath[type];

            return (isForAdministration ? "../" : string.Empty) + PhotoFoldersPath[type] + photoPath;
        }

        public static string GetPathAbsolut(FolderType type, string photoPath = "")
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return GetPathAbsolut(PhotoFoldersPath[type]);
            return GetPathAbsolut(PhotoFoldersPath[type]) + photoPath;
        }
        //_____________


        #region ProductImage
        public static string GetImageProductPath(ProductImageType type, string photoPath, bool isForAdministration)
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return UrlService.GetUrl() + "images/nophoto" + ProductPhotoPostfix[type] + ".jpg";

            if (photoPath.Contains("://"))
            {
                var fileName = Path.GetFileName(photoPath);
                var path = photoPath.Replace(fileName, "");
                return GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration, path) + ProductPhotoPrefix[type] + fileName.Replace(".", ProductPhotoPostfix[type] + ".");
            }

            return GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration, string.Empty) + ProductPhotoPrefix[type] + photoPath.Replace(".", ProductPhotoPostfix[type] + ".");
        }

        public static string GetImageProductPathRelative(ProductImageType type, string photoPath, bool isForAdministration)
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return "images/nophoto" + ProductPhotoPostfix[type] + ".jpg";

            return (isForAdministration ? "../" : string.Empty) + PhotoFoldersPath[FolderType.Product] +
                   ProductPhotoPrefix[type] + photoPath.Replace(".", ProductPhotoPostfix[type] + ".");
        }

        public static string GetImageProductPathAbsolut(ProductImageType type, string photoPath)
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return GetPathAbsolut(PhotoFoldersPath[FolderType.Product]) + ProductPhotoPrefix[type];
            return GetPathAbsolut(PhotoFoldersPath[FolderType.Product]) + ProductPhotoPrefix[type] + Path.GetFileNameWithoutExtension(photoPath) + ProductPhotoPostfix[type] + Path.GetExtension(photoPath);
        }
        #endregion


        #region CategoryImage
        public static string GetImageCategoryPathAbsolut(CategoryImageType type, string photoPath)
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return GetPathAbsolut(PhotoFoldersPath[FolderType.Category]) + CategoryPhotoPrefix[type];

            return GetPathAbsolut(PhotoFoldersPath[FolderType.Category]) + CategoryPhotoPrefix[type] + photoPath;
        }

        public static string GetImageCategoryPath(CategoryImageType type, string photoPath, bool isForAdministration)
        {
            if (photoPath.Contains("://"))
            {
                var fileName = Path.GetFileName(photoPath);
                var path = photoPath.Replace(fileName, "");
                return GetPath(PhotoFoldersPath[FolderType.Category], isForAdministration, path) + CategoryPhotoPrefix[type] + fileName;
            }

            return GetPath(PhotoFoldersPath[FolderType.Category], isForAdministration, string.Empty) + CategoryPhotoPrefix[type] + photoPath;
        }
        #endregion

        public static string GetImageColorPathAbsolut(ColorImageType type, string photoPath)
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return GetPathAbsolut(PhotoFoldersPath[FolderType.Color]) + ColorPhotoPrefix[type];

            return GetPathAbsolut(PhotoFoldersPath[FolderType.Color]) + ColorPhotoPrefix[type] + photoPath;
        }

        public static string GetImageColorPath(ColorImageType type, string photoPath, bool isForAdministration)
        {
            if (photoPath.Contains("://"))
            {
                var fileName = Path.GetFileName(photoPath);
                var path = photoPath.Replace(fileName, "");
                return GetPath(PhotoFoldersPath[FolderType.Color], isForAdministration, path) + ColorPhotoPrefix[type] + fileName;
            }

            return GetPath(PhotoFoldersPath[FolderType.Color], isForAdministration, string.Empty) + ColorPhotoPrefix[type] + photoPath;
        }

        public static void InitFolders()
        {
            foreach (var key in PhotoFoldersPath.Keys)
            {
                try
                {
                    var path = GetPathAbsolut(key);

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            }
        }

        public static void InitExtraCss()
        {
            try
            {
                var userfiles = GetPathAbsolut(FolderType.UserFiles);

                if (!Directory.Exists(userfiles))
                    Directory.CreateDirectory(userfiles);

                var extraCssPath = GetPathAbsolut(FolderType.UserFiles, "extra.css");
                if (!File.Exists(extraCssPath))
                    FileHelpers.CreateFile(extraCssPath);

                extraCssPath = GetPathAbsolut(FolderType.UserFiles, "extra-admin.css");
                if (!File.Exists(extraCssPath))
                    FileHelpers.CreateFile(extraCssPath);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }
    }
}