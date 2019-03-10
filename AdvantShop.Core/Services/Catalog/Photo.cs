//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;

namespace AdvantShop.Catalog
{
    public enum PhotoType
    {
        None,
        Product,
        Product360,
        CategoryBig,
        CategorySmall,
        CategoryIcon,
        News,
        StaticPage,
        Brand,
        Carousel,
        MenuIcon,
        Shipping,
        Payment,
        Color,
        Manager,
        Review,
        Logo,
        Favicon
    }

    public class Photo
    {
        public int PhotoId { get; set; }

        public int ObjId { get; set; }

        public PhotoType Type { get; set; }

        public string PhotoName { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Description { get; set; }

        public int PhotoSortOrder { get; set; }

        public bool Main { get; set; }

        public string OriginName { get; set; }

        public int? ColorID { get; set; }

        public Photo(int photoId, int objId, PhotoType type)
        {
            PhotoId = photoId;
            ObjId = objId;
            Type = type;
        }
    }


    public class BrandPhoto : Photo
    {
        #region Constructor

        public BrandPhoto() : base(0, 0, PhotoType.Brand) { }

        public BrandPhoto(int photoId, int objId) : base(photoId, objId, PhotoType.Brand) { }

        #endregion
        
        public string ImageSrc()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? UrlService.GetUrl() + "images/nophoto_xsmall.jpg"
                : FoldersHelper.GetPath(FolderType.BrandLogo, PhotoName, false);
        }
    }

    public class NewsPhoto : Photo
    {
        #region Constructor

        public NewsPhoto() : base(0, 0, PhotoType.News) { }

        public NewsPhoto(int photoId, int objId) : base(photoId, objId, PhotoType.News) { }

        #endregion

        public string ImageSrc()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? ""
                : FoldersHelper.GetPath(FolderType.News, PhotoName, false);
        }
    }

    [Obsolete("Not used since 6.0")]
    public class ManagerPhoto : Photo
    {
        #region Constructor

        public ManagerPhoto() : base(0, 0, PhotoType.Manager) { }

        public ManagerPhoto(int photoId, int objId) : base(photoId, objId, PhotoType.Manager) { }

        #endregion

        public string ImageSrc()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? ""
                : FoldersHelper.GetPath(FolderType.ManagerPhoto, PhotoName, false);
        }
    }


    public class CategoryPhoto : Photo
    {
        #region Constructor

        public CategoryPhoto() : base(0, 0, PhotoType.None) { }

        public CategoryPhoto(int objId, PhotoType type, string photoName) : base(0, objId, type)
        {
            PhotoName = photoName;
        }

        #endregion

        public string ImageSrcSmall()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? UrlService.GetUrl() + "images/nophoto_small.jpg"
                : FoldersHelper.GetImageCategoryPath(CategoryImageType.Small, PhotoName, false);
        }

        public string ImageSrcBig()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? UrlService.GetUrl() + "images/nophoto_big.jpg"
                : FoldersHelper.GetImageCategoryPath(CategoryImageType.Big, PhotoName, false);
        }

        public string IconSrc(bool noPhoto = false)
        {
            return string.IsNullOrEmpty(PhotoName)
                ? (noPhoto ? UrlService.GetUrl() + "images/nophoto_xsmall.jpg" : "")
                : FoldersHelper.GetImageCategoryPath(CategoryImageType.Icon, PhotoName, false);
        }
    }


    public class ProductPhoto : Photo
    {
        #region Constructor

        public ProductPhoto() : base(0, 0, PhotoType.None) { }

        public ProductPhoto(int objId, PhotoType type, string photoName)
            : base(0, objId, type)
        {
            PhotoName = photoName;
        }

        #endregion

        public string Title { get; set; }

        public string Alt { get; set; }

        public string ImageSrcXSmall()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? UrlService.GetUrl() + "images/nophoto_xsmall.jpg"
                : FoldersHelper.GetImageProductPath(ProductImageType.XSmall, PhotoName, false);
        }

        public string ImageSrcSmall()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? UrlService.GetUrl() + "images/nophoto_small.jpg"
                : FoldersHelper.GetImageProductPath(ProductImageType.Small, PhotoName, false);
        }

        public string ImageSrcMiddle()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? UrlService.GetUrl() + "images/nophoto_big.jpg"
                : FoldersHelper.GetImageProductPath(ProductImageType.Middle, PhotoName, false);
        }

        public string ImageSrcBig()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? UrlService.GetUrl() + "images/nophoto_big.jpg"
                : FoldersHelper.GetImageProductPath(ProductImageType.Big, PhotoName, false);
        }

        public string ImageSrcRotate()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? UrlService.GetUrl() + "images/nophoto_small.jpg"
                : UrlService.GetUrl() + "pictures/product/rotate/" + ObjId + "/" + PhotoName;
                //FoldersHelper.GetImageProductPath(ProductImageType.Rotate, PhotoName, false);
        }
    }

    public class CarouselPhoto : Photo
    {
        #region Constructor

        public CarouselPhoto() : base(0, 0, PhotoType.Carousel) { }

        public CarouselPhoto(int photoId, int objId) : base(photoId, objId, PhotoType.Carousel) { }

        #endregion

        public string ImageSrc()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? UrlService.GetUrl() + "images/nophoto_carousel_" + SettingsMain.Language + ".jpg"
                : FoldersHelper.GetPath(FolderType.Carousel, PhotoName, false);
        }
    }

    public class ColorPhoto : Photo
    {
        #region Constructor

        public ColorPhoto() : base(0, 0, PhotoType.Color) { }

        public ColorPhoto(int photoId, int objId) : base(photoId, objId, PhotoType.Color) { }

        #endregion

        public int ImageWidthDetails
        {
            get { return SettingsPictureSize.ColorIconWidthDetails; }
        }

        public int ImageHeightDetails
        {
            get { return SettingsPictureSize.ColorIconHeightDetails; }
        }

        public int ImageWidthCatalog
        {
            get { return SettingsPictureSize.ColorIconWidthCatalog; }
        }

        public int ImageHeightCatalog
        {
            get { return SettingsPictureSize.ColorIconHeightCatalog; }
        }


        public string ImageSrc(ColorImageType type)
        {
            return string.IsNullOrEmpty(PhotoName)
                ? string.Empty
                : FoldersHelper.GetImageColorPath(type, PhotoName, false);
        }
    }

    public class ReviewPhoto : Photo
    {
        #region Constructor

        public ReviewPhoto() : base(0, 0, PhotoType.Review) { }

        public ReviewPhoto(int photoId, int objId) : base(photoId, objId, PhotoType.Review) { }

        #endregion

        public int ImageWidth
        {
            get { return SettingsPictureSize.ReviewImageWidth; }
        }

        public int ImageHeight
        {
            get { return SettingsPictureSize.ReviewImageHeight; }
        }

        public string ImageSrc()
        {
            return string.IsNullOrEmpty(PhotoName)
                ? string.Empty
                : FoldersHelper.GetPath(FolderType.ReviewImage, PhotoName, false);
        }
    }
}