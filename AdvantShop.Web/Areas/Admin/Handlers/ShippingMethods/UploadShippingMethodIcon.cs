using System;
using System.Drawing;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Shipping;
using AdvantShop.Web.Admin.Models.Brands;

namespace AdvantShop.Web.Admin.Handlers.ShippingMethods
{
    public class UploadShippingMethodIcon
    {
        private readonly int _methodId;

        public UploadShippingMethodIcon(int methodId)
        {
            _methodId = methodId;
        }

        public UploadBrandPictureResult Execute()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request.Files.Count == 0)
                return new UploadBrandPictureResult() {Error = "Файл не найден"};

            FileHelpers.UpdateDirectories();

            var method = ShippingMethodService.GetShippingMethod(_methodId);
            if (method == null)
                return new UploadBrandPictureResult() { Error = "Доставка не найдена" };


            var img = HttpContext.Current.Request.Files["file"];
            if (img != null && img.ContentLength > 0)
            {
                if (!FileHelpers.CheckFileExtension(img.FileName, EAdvantShopFileTypes.Image))
                    return new UploadBrandPictureResult() { Error = "Неправильное расширение файла" };

                try
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, _methodId, PhotoType.Shipping) { OriginName = img.FileName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        using (Image image = Image.FromStream(img.InputStream))
                        {
                            FileHelpers.SaveResizePhotoFile(
                                FoldersHelper.GetPathAbsolut(FolderType.ShippingLogo, tempName),
                                SettingsPictureSize.ShippingIconWidth, SettingsPictureSize.ShippingIconHeight, image);
                        }
                    }

                    return new UploadBrandPictureResult()
                    {
                        Result = true,
                        Picture = FoldersHelper.GetPath(FolderType.ShippingLogo, tempName, false)
                    };
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex.Message, ex);
                }
            }

            return new UploadBrandPictureResult() { Error = "Файл не найден" };
        }
    }
}
