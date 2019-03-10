using System;
using System.Drawing;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Models.Products;

namespace AdvantShop.Web.Admin.Handlers.Products
{
    public class UploadProductPicturesByLink
    {
        private readonly int _productId;
        private readonly string _fileLink;

        public UploadProductPicturesByLink(int productId, string fileLink)
        {
            _productId = productId;
            _fileLink = fileLink;
        }

        public UploadPhotoResultModel Execute()
        {
            if (SaasDataService.IsSaasEnabled)
            {
                var maxPhotoCount = SaasDataService.CurrentSaasData.PhotosCount;

                if (PhotoService.GetCountPhotos(_productId, PhotoType.Product) >= maxPhotoCount)
                {
                    return new UploadPhotoResultModel()
                    {
                        Error = LocalizationService.GetResource("Admin.UploadPhoto.MaxReached") + maxPhotoCount
                    };
                }
            }

            FileHelpers.UpdateDirectories();

            var errors = "";

            if (!FileHelpers.CheckFileExtension(_fileLink, EAdvantShopFileTypes.Image))
                return new UploadPhotoResultModel()
                {
                    Result = false,
                    Error = string.Format("Файл {0} имеет неправильное разрешение. ", _fileLink)
                };

            var add = AddPhoto(_fileLink, PhotoType.Product);

            if (!add)
                errors += "Ошибка при добавлении файла ";
            
            ProductService.PreCalcProductParams(_productId);

            return new UploadPhotoResultModel() {Result = errors == "", Error = errors};
        }


        private bool AddPhoto(string fileLink, PhotoType type)
        {
            try
            {
                var photo = new Photo(0, _productId, type) { OriginName = fileLink };
                var photoName = PhotoService.AddPhoto(photo);
                var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                if (!string.IsNullOrWhiteSpace(photoName))
                {
                    if (FileHelpers.DownloadRemoteImageFile(fileLink, photoFullName))
                    {
                        using (var image = Image.FromFile(photoFullName))
                            FileHelpers.SaveProductImageUseCompress(photoName, image);

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("UploadProductPicturesByLink", ex);
            }

            return false;
        }
    }
}
