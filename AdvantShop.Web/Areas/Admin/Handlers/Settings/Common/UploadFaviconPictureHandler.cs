using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Common
{
    public class UploadFaviconPictureHandler
    {
        public UploadPictureResult Execute()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request.Files.Count == 0)
                return new UploadPictureResult() { Error = "Файл не найден" };

            var img = HttpContext.Current.Request.Files["file"];

            if (img != null && img.ContentLength > 0)
            {
                return AddPhoto(img);
            }

            return new UploadPictureResult() { Error = "Файл не найден" };
        }

        private UploadPictureResult AddPhoto(HttpPostedFile file)
        {
            if (FileHelpers.CheckFileExtension(file.FileName, EAdvantShopFileTypes.Favicon))
            {
                FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
                FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.FaviconImageName));

                var newFile = file.FileName.FileNamePlusDate("favicon");
                SettingsMain.FaviconImageName = newFile;
                file.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.Pictures, newFile));

                return new UploadPictureResult()
                {
                    Result = true,
                    Picture = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.FaviconImageName, true)
                };
            }

            return new UploadPictureResult() { Error = "Файл не найден" };
        }

    }
}
