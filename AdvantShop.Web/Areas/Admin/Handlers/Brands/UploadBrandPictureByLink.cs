using System.Drawing;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.Brands;

namespace AdvantShop.Web.Admin.Handlers.Brands
{
    public class UploadBrandPictureByLink
    {
        private readonly int _brandId;
        private readonly bool _isEditMode;
        private readonly string _fileLink;

        public UploadBrandPictureByLink(int? brandId, string fileLink)
        {
            _brandId = brandId != null ? brandId.Value : -1;
            _isEditMode = _brandId != -1;
            _fileLink = fileLink;
        }

        public UploadBrandPictureResult Execute()
        {
            if (string.IsNullOrEmpty(_fileLink))
                return new UploadBrandPictureResult() { Error = "Файл не найден" };

            FileHelpers.UpdateDirectories();

            return AddPhoto(_fileLink, PhotoType.Brand, SettingsPictureSize.BrandLogoWidth, SettingsPictureSize.BrandLogoHeight);
        }


        private UploadBrandPictureResult AddPhoto(string fileLink, PhotoType type, int width, int height)
        {
            if (_isEditMode)
                PhotoService.DeletePhotos(_brandId, type);

            var photo = new Photo(0, _brandId, type) { OriginName = fileLink };
            var photoName = PhotoService.AddPhoto(photo);
            var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

            if (!string.IsNullOrWhiteSpace(photoName))
            {

                if (FileHelpers.DownloadRemoteImageFile(fileLink, photoFullName))
                {
                    using (var image = Image.FromFile(photoFullName))
                    {
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, photoName),
                            width, height, image);
                    }

                    return new UploadBrandPictureResult()
                    {
                        Result = true,
                        PictureId = photo.PhotoId,
                        Picture = FoldersHelper.GetPath(FolderType.BrandLogo, photoName, true)
                    };
                }
            }

            return new UploadBrandPictureResult() { Error = "Файл не найден" };
        }
    }
}
