using System.Web;

using AdvantShop.Helpers;
using AdvantShop.FilePath;
using AdvantShop.Web.Admin.Models.Import;
using System;
using AdvantShop.Diagnostics;

namespace AdvantShop.Web.Admin.Handlers.Import
{
    public class DeleteImagesArchiveFileHandler
    {
        public DeleteImagesArchiveFileHandler()
        {
            FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.PriceTemp));
        }

        public UploadFileResult Execute()
        {
            try
            {
                FileHelpers.DeleteFilesFromPath(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return new UploadFileResult { Result = false, Error = "Admin_ImportCsv_ErrorAtUploadFile" };
            }

            return new UploadFileResult { Result = true };
        }
    }
}
