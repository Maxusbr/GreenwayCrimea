using AdvantShop.Configuration;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Trial;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Carousel
{
    class UploadPictureByLink
    {
        private readonly string _fileLink;

        public UploadPictureByLink(string fileLink)
        {
            _fileLink = fileLink;
        }

        public UploadPictureResult Execute()
        {
            if(string.IsNullOrEmpty(_fileLink))
                return new UploadPictureResult() { Error = "Файл не найден" };

            return AddPhoto(_fileLink);
        }

        private UploadPictureResult AddPhoto(string fileLink)
        {
            var fileName = fileLink.Substring(fileLink.LastIndexOf("/") + 1);

            if (FileHelpers.CheckFileExtension(fileName, EAdvantShopFileTypes.Image))
            {
                FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
                FileHelpers.DeleteFilesFromImageTemp();
                var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, fileName);

                if (FileHelpers.DownloadRemoteImageFile(fileLink, photoFullName))
                {                   
                    TrialService.TrackEvent(TrialEvents.ChangeLogo, "");
                    return new UploadPictureResult()
                    {
                        Result = true,
                        Picture = FoldersHelper.GetPath(FolderType.ImageTemp, fileName, true),
                        FileName = fileName
                    };
                }
            }

            return new UploadPictureResult() { Error = "Файл не найден" };
        }
    }
}
