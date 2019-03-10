using System;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.Brands;

namespace AdvantShop.Web.Admin.Handlers.Menus
{
    public class UploadMenuIcon
    {
        private readonly int _itemId;

        public UploadMenuIcon(int? itemId)
        {
            _itemId = itemId != null ? itemId.Value : 0;
        }

        public UploadBrandPictureResult Execute()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request.Files.Count == 0)
                return new UploadBrandPictureResult() {Error = "Файл не найден"};

            FileHelpers.UpdateDirectories();
            

            var img = HttpContext.Current.Request.Files["file"];
            if (img != null && img.ContentLength > 0)
            {
                if (!FileHelpers.CheckFileExtension(img.FileName, EAdvantShopFileTypes.Image))
                    return new UploadBrandPictureResult() { Error = "Файл имеет неправильное разрешение" };

                try
                {
                    var photo = new Photo(0, _itemId, PhotoType.MenuIcon) {OriginName = img.FileName};
                    var tempName = PhotoService.AddPhoto(photo);
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        img.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, tempName));
                    }

                    return new UploadBrandPictureResult()
                    {
                        Result = true,
                        PictureId = photo.PhotoId,
                        PictureName = photo.PhotoName,
                        Picture = FoldersHelper.GetPath(FolderType.MenuIcons, photo.PhotoName, false)
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
