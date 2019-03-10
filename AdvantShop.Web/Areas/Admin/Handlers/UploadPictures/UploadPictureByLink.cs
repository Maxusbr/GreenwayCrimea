using System.Drawing;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.Categories;

namespace AdvantShop.Web.Admin.Handlers.UploadPictures
{
    public class UploadPictureByLink
    {
        private readonly int _categoryId;
        private readonly bool _isEditMode;
        private readonly PhotoType _type;
        private readonly string _fileLink;

        public UploadPictureByLink(PhotoType type, int? categoryId, string fileLink)
        {
            _categoryId = categoryId != null ? categoryId.Value : -1;
            _isEditMode = _categoryId != -1;
            _type = type;
            _fileLink = fileLink;
        }

        public UploadCategoryPictureResult Execute()
        {
            UploadCategoryPictureResult result;

            FileHelpers.UpdateDirectories();

            if (_isEditMode)
            {
                var cacheKey = CacheNames.GetCategoryCacheObjectName(_categoryId);
                CacheManager.Remove(cacheKey);
            }


            switch (_type)
            {
                case PhotoType.CategoryBig:
                    result = AddPhoto(_fileLink, PhotoType.CategoryBig, CategoryImageType.Big,
                                    SettingsPictureSize.BigCategoryImageWidth,
                                    SettingsPictureSize.BigCategoryImageHeight);
                    break;
                case PhotoType.CategorySmall:
                    result = AddPhoto(_fileLink, PhotoType.CategorySmall, CategoryImageType.Small,
                                    SettingsPictureSize.SmallCategoryImageWidth,
                                    SettingsPictureSize.SmallCategoryImageHeight);
                    break;
                case PhotoType.CategoryIcon:
                    result = AddPhoto(_fileLink, PhotoType.CategoryIcon, CategoryImageType.Icon,
                                    SettingsPictureSize.IconCategoryImageWidth,
                                    SettingsPictureSize.IconCategoryImageHeight);
                    break;
                default:
                    result = new UploadCategoryPictureResult() { Error = "Файл не найден" };
                    break;
            }

            CategoryService.ClearCategoryCache(_categoryId);

            return result;
        }


        private UploadCategoryPictureResult AddPhoto(string fileLink, PhotoType type, CategoryImageType imgType, int width, int height)
        {
            if (_isEditMode)
                PhotoService.DeletePhotos(_categoryId, type);

            var photo = new Photo(0, _categoryId, type) { OriginName = fileLink };
            var photoName = PhotoService.AddPhoto(photo);
            var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

            if (!string.IsNullOrWhiteSpace(photoName))
            {
                if (FileHelpers.DownloadRemoteImageFile(fileLink, photoFullName))
                {
                    using (var image = Image.FromFile(photoFullName))
                    {
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageCategoryPathAbsolut(imgType, photoName),
                            width, height, image);
                    }

                    return new UploadCategoryPictureResult()
                    {
                        Result = true,
                        PictureId = photo.PhotoId,
                        Picture = FoldersHelper.GetImageCategoryPath(imgType, photoName, false)
                    };
                }
            }

            return new UploadCategoryPictureResult() { Error = "Файл не найден" };
        }

    }
}
