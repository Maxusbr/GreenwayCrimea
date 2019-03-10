using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Common
{
    public class UploadFaviconPictureByLinkHandler
    {
        private readonly string _fileLink;

        public UploadFaviconPictureByLinkHandler(string fileLink)
        {
            _fileLink = fileLink;
        }

        public UploadPictureResult Execute()
        {
            if (string.IsNullOrEmpty(_fileLink))
                return new UploadPictureResult() { Error = "Файл не найден" };

            return AddPhoto(_fileLink);
        }


        private UploadPictureResult AddPhoto(string fileLink)
        {
            var fileName = fileLink.Substring(fileLink.LastIndexOf("/") + 1);
            var newFileName = fileName.FileNamePlusDate("favicon");

            if (FileHelpers.CheckFileExtension(newFileName, EAdvantShopFileTypes.Favicon))
            {
                var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.Pictures, newFileName);

                if (FileHelpers.DownloadRemoteImageFile(fileLink, photoFullName))
                {
                    FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
                    FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.FaviconImageName));

                    SettingsMain.FaviconImageName = newFileName;                    
                    return new UploadPictureResult()
                    {
                        Result = true,
                        Picture = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.FaviconImageName, true)
                    };
                }
            }

            return new UploadPictureResult() { Error = "Файл не найден" };
        }
    }
}
