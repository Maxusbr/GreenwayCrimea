using System.Drawing;
using System.Web;
//using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.Categories;
using AdvantShop.Catalog;
using System.IO;

namespace AdvantShop.Web.Admin.Handlers.UploadPictures
{
    public class UploadPicture
    {
        private readonly int _objId;
        private readonly bool _isEditMode;
        private readonly PhotoType _type;
        private readonly HttpPostedFileBase _file;

        public UploadPicture(HttpPostedFileBase file, PhotoType type, int? objId)
        {
            _objId = objId.HasValue ? objId.Value : -1;
            _isEditMode = _objId != -1;
            _type = type;
            _file = file;
        }

        public UploadCategoryPictureResult Execute()
        {
            UploadCategoryPictureResult result = new UploadCategoryPictureResult();

            if (_file == null || string.IsNullOrEmpty(_file.FileName))
                return new UploadCategoryPictureResult() { Error = "Файл не найден" };

            if (!FileHelpers.CheckFileExtension(_file.FileName, EAdvantShopFileTypes.Image))
                return new UploadCategoryPictureResult() { Error = "Файл имеет неправильное разрешение" };

            FileHelpers.UpdateDirectories();

            if (_isEditMode)
            {
                //var cacheKey = CacheNames.GetCategoryCacheObjectName(_categoryId);
                //CacheManager.Remove(cacheKey);
            }

            //switch (_type)
            //{
            //    case CategoryImageType.Big:
            //        result = AddPhoto(_file, PhotoType.CategoryBig, CategoryImageType.Big,
            //                        SettingsPictureSize.BigCategoryImageWidth,
            //                        SettingsPictureSize.BigCategoryImageHeight);
            //        break;
            //    case CategoryImageType.Small:
            //        result = AddPhoto(_file, PhotoType.CategorySmall, CategoryImageType.Small,
            //                        SettingsPictureSize.SmallCategoryImageWidth,
            //                        SettingsPictureSize.SmallCategoryImageHeight);
            //        break;
            //    case CategoryImageType.Icon:
            //        result = AddPhoto(_file, PhotoType.CategoryIcon, CategoryImageType.Icon,
            //                        SettingsPictureSize.IconCategoryImageWidth,
            //                        SettingsPictureSize.IconCategoryImageHeight);
            //        break;
            //    default:
            //        result = new UploadCategoryPictureResult() { Error = "Файл не найден" };
            //        break;
            //}

            //CategoryService.ClearCategoryCache(_categoryId);

           // result = AddPhoto(_file, _type, SettingsPictureSize.IconCategoryImageWidth, SettingsPictureSize.IconCategoryImageHeight);

            return result;
        }


        //private UploadCategoryPictureResult AddPhoto(HttpPostedFileBase file, PhotoType type, int width, int height)
        //{
        //    //if (_isEditMode)
        //    //    PhotoService.DeletePhotos(_objId, type);

        //    var filePath = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, file.FileName);

        //    //var fileViewPath = FoldersHelper.GetPath(FolderType.ImageTemp, file.FileName, true);

        //    if (File.Exists(filePath))
        //    {
        //        FileHelpers.DeleteFilesFromPath(filePath);
        //    }

        //    if (!string.IsNullOrWhiteSpace(filePath))
        //    {
        //        var photo = new Photo(0, _objId, _type)
        //        {                    
        //            Description = string.Empty,
        //            OriginName = file.FileName
        //        };

        //        if (_isEditMode)
        //        {
        //            //PhotoService.DeletePhotos(_newsId, PhotoType.News, false);
        //        }

        //        PhotoService.AddPhoto(photo);
        //        if (string.IsNullOrWhiteSpace(photo.PhotoName)) return new PictureUploaderResult() { Error = "Ошибка добавления" };

        //        var fileViewPath = FoldersHelper.GetPath(FolderType.News, photo.PhotoName, true);
        //        file.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.News, photo.PhotoName));

        //        //else
        //        //{
        //        //    file.SaveAs(filePath);
        //        //}

        //        //NewsService.ClearCache();

        //        return new UploadNewsPictureResult()
        //        {
        //            Result = true,
        //            Picture = fileViewPath,
        //            FileName = file.FileName,
        //            PictureId = photo.PhotoId
        //        };

        //        /////////////////////////////////////////////////

        //        //var photo = new Photo(0, _objId, type) { OriginName = file.FileName };
        //        //var photoName = PhotoService.AddPhoto(photo);

        //        //if (!string.IsNullOrWhiteSpace(photoName))
        //        //{
        //        //    using (Image image = Image.FromStream(file.InputStream))
        //        //        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageCategoryPathAbsolut(_type, photoName), width, height, image);

        //        //    return new UploadCategoryPictureResult()
        //        //    {
        //        //        Result = true,
        //        //        PictureId = photo.PhotoId,
        //        //        Picture = FoldersHelper.GetImageCategoryPath(imgType, photoName, false)
        //        //    };
        //        //}

        //        return new UploadCategoryPictureResult() { Error = "Файл не найден" };
        //    }

        }
    }
