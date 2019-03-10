using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Models.Products;

namespace AdvantShop.Web.Admin.Handlers.Products
{
    public class UploadProductPictures360
    {
        private readonly int _productId;
        private readonly List<HttpPostedFileBase> _files;

        public UploadProductPictures360(List<HttpPostedFileBase> files, int productId)
        {
            _files = files;
            _productId = productId;
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

            if (_files.Count == 0)
                return new UploadPhotoResultModel() { Error = "Файл не найден" };
            

            foreach (var photo in PhotoService.GetPhotos<ProductPhoto>(_productId, PhotoType.Product360))
            {
                PhotoService.DeleteProductPhoto(photo.PhotoId);
            }

            var errors = "";

            foreach (var file in _files)
            {
                if (file == null || string.IsNullOrEmpty(file.FileName))
                    continue;

                if (!FileHelpers.CheckFileExtension(file.FileName, EAdvantShopFileTypes.Image))
                {
                    errors += string.Format("Файл {0} имеет неправильное расширение. ", file.FileName);
                    continue;
                }

                var add = AddPhoto(file);

                if (!add)
                    errors += "Ошибка при добавлении файла " + file.FileName;
            }

            ProductService.PreCalcProductParams(_productId);

            return new UploadPhotoResultModel() {Result = errors == "", Error = errors};
        }


        private bool AddPhoto(HttpPostedFileBase file)
        {
            var fileName = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);

            try
            {
                var photoId = PhotoService.AddPhotoWithOrignName(new Photo(0, _productId, PhotoType.Product360) { OriginName = file.FileName, PhotoName = fileName, Main = false });

                if (photoId == 0)
                    return false;

                var filePath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Rotate, string.Empty) + _productId + "/";

                FileHelpers.CreateDirectory(filePath);
                FileHelpers.SaveFile(filePath + fileName, file.InputStream);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex.Message + " at UploadProductPicture360", ex);
                return false;
            }

            return true;
        }

    }
}
