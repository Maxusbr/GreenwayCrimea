using System.Web;
using System.Drawing;
using AdvantShop.Catalog;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.News;
using System.IO;
using AdvantShop.News;
using AdvantShop.Configuration;

namespace AdvantShop.Web.Admin.Handlers.News
{
    public class UploadNewsPicture
    {
        private readonly int _newsId;
        private readonly bool _isEditMode;

        public UploadNewsPicture(int? newsId)
        {
            _newsId = newsId != null ? newsId.Value : -1;
            _isEditMode = _newsId != -1;
        }

        public UploadNewsPictureResult Execute()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request.Files.Count == 0)
                return new UploadNewsPictureResult() { Error = "Файл не найден" };

            FileHelpers.UpdateDirectories();


            var img = HttpContext.Current.Request.Files["file"];
            if (img != null && img.ContentLength > 0)
            {
                return AddPhoto(img);
            }

            return new UploadNewsPictureResult() { Error = "Файл не найден" };
        }

        private UploadNewsPictureResult AddPhoto(HttpPostedFile file)
        {
            var filePath = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, file.FileName);

            if (File.Exists(filePath))
            {
                FileHelpers.DeleteFilesFromPath(filePath);
            }

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                if (_isEditMode)
                {
                    PhotoService.DeletePhotos(_newsId, PhotoType.News, false);
                }

                var photo = new Photo(0, _newsId, PhotoType.News) {OriginName = file.FileName};
                PhotoService.AddPhoto(photo);

                if (string.IsNullOrWhiteSpace(photo.PhotoName))
                    return new UploadNewsPictureResult() { Error = "Ошибка при добавлении" };

                var fileViewPath = FoldersHelper.GetPath(FolderType.News, photo.PhotoName, true);
                using (var img = Image.FromStream(file.InputStream))
                {
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.News, photo.PhotoName), SettingsPictureSize.NewsImageWidth, SettingsPictureSize.NewsImageHeight, img);
                }
                
                NewsService.ClearCache();

                return new UploadNewsPictureResult()
                {
                    Result = true,
                    Picture = fileViewPath,
                    FileName = file.FileName,
                    PictureId = photo.PhotoId
                };
            }

            return new UploadNewsPictureResult() { Error = "Файл не найден" };
        }
    }
}
