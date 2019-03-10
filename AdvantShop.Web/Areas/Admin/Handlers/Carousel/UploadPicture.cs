using System.Web;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Trial;

namespace AdvantShop.Web.Admin.Handlers.Carousel
{
    class UploadPicture
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
                FileHelpers.DeleteFilesFromImageTemp();
                file.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, file.FileName));

                TrialService.TrackEvent(ETrackEvent.Trial_AddCarousel);

                return new UploadPictureResult()
                {
                    Result = true,
                    Picture = FoldersHelper.GetPath(FolderType.ImageTemp, file.FileName, true),
                    FileName = file.FileName
                };
            }

            return new UploadPictureResult() { Error = "Файл не найден" };
        }
    }
}
