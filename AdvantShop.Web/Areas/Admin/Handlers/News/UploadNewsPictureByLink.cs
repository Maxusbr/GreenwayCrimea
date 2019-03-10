using System.Drawing;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.News;
using AdvantShop.News;

namespace AdvantShop.Web.Admin.Handlers.News
{
    public class UploadNewsPictureByLink
    {
        private readonly int _newsId;
        private readonly bool _isEditMode;
        private readonly string _fileLink;

        public UploadNewsPictureByLink(int? newsId, string fileLink)
        {
            _newsId = newsId ?? -1;
            _isEditMode = _newsId != -1;
            _fileLink = fileLink;
        }

        public UploadNewsPictureResult Execute()
        {
            if (string.IsNullOrEmpty(_fileLink))
                return new UploadNewsPictureResult() { Error = "Файл не найден" };

            FileHelpers.UpdateDirectories();

            return AddPhoto(_fileLink, PhotoType.News, SettingsPictureSize.NewsImageWidth, SettingsPictureSize.NewsImageHeight);
        }

        private UploadNewsPictureResult AddPhoto(string fileLink, PhotoType type, int width, int height)
        {
            if (_isEditMode)
                PhotoService.DeletePhotos(_newsId, type);

            var photo = new Photo(0, _newsId, type) { OriginName = fileLink };
            var photoName = PhotoService.AddPhoto(photo);
            var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

            if (!string.IsNullOrWhiteSpace(photoName))
            {
                NewsService.ClearCache();

                if (FileHelpers.DownloadRemoteImageFile(fileLink, photoFullName))
                {
                    using (var image = Image.FromFile(photoFullName))
                    {
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.News, photoName),
                            width, height, image);
                    }

                    return new UploadNewsPictureResult()
                    {
                        Result = true,
                        PictureId = photo.PhotoId,
                        Picture = FoldersHelper.GetPath(FolderType.News, photoName, true)
                    };
                }
            }

            return new UploadNewsPictureResult() { Error = "Файл не найден" };
        }
    }
}
