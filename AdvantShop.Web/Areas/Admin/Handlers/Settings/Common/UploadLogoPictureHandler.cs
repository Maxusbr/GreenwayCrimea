using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Common
{
    public class UploadLogoPictureHandler
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
            if (FileHelpers.CheckFileExtension(file.FileName, EAdvantShopFileTypes.Image))
            {
                FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
                FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.LogoImageName));

                var newFile = file.FileName.FileNamePlusDate("logo");
                SettingsMain.LogoImageName = newFile;
                file.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.Pictures, newFile));

                TrialService.TrackEvent(TrialEvents.ChangeLogo, "");
                TrialService.TrackEvent(ETrackEvent.Trial_ChangeLogo);

                return new UploadPictureResult()
                {
                    Result = true,
                    Picture = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, true)
                };
            }

            return new UploadPictureResult() { Error = "Файл не найден" };
        }

    }
}
