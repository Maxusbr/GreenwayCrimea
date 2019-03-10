using System.Drawing;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.Brands;

namespace AdvantShop.Web.Admin.Handlers.Brands
{
    public class UploadBrandPicture
    {
        private readonly int _brandId;
        private readonly bool _isEditMode;

        public UploadBrandPicture(int? brandId)
        {
            _brandId = brandId != null ? brandId.Value : -1;
            _isEditMode = _brandId != -1;
        }

        public UploadBrandPictureResult Execute()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request.Files.Count == 0)
                return new UploadBrandPictureResult() {Error = "Файл не найден"};

            FileHelpers.UpdateDirectories();
            

            var img = HttpContext.Current.Request.Files["file"];
            if (img != null && img.ContentLength > 0)
            {
                return AddPhoto(img, PhotoType.Brand,
                                SettingsPictureSize.BrandLogoWidth,
                                SettingsPictureSize.BrandLogoHeight);
            }

            return new UploadBrandPictureResult() { Error = "Файл не найден" };
        }


        private UploadBrandPictureResult AddPhoto(HttpPostedFile file, PhotoType type, int width, int height)
        {
            if (_isEditMode)
                PhotoService.DeletePhotos(_brandId, type);

            var photo = new Photo(0, _brandId, type) { OriginName = file.FileName };
            var photoName = PhotoService.AddPhoto(photo);

            if (!string.IsNullOrWhiteSpace(photoName))
            {
                using (Image image = Image.FromStream(file.InputStream))
                {
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, photoName),
                                  SettingsPictureSize.BrandLogoWidth, SettingsPictureSize.BrandLogoHeight, image);
                }                

                return new UploadBrandPictureResult()
                {
                    Result = true,
                    PictureId = photo.PhotoId,
                    Picture = FoldersHelper.GetPath(FolderType.BrandLogo, photoName, true)                    
                };
            }

            return new UploadBrandPictureResult() { Error = "Файл не найден" };
        }

    }
}
