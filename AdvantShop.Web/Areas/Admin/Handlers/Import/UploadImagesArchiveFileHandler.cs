using System.Web;

using AdvantShop.Helpers;
using AdvantShop.FilePath;
using AdvantShop.Web.Admin.Models.Import;
using System;
using AdvantShop.Diagnostics;

namespace AdvantShop.Web.Admin.Handlers.Import
{
    public class UploadImagesArchiveFileHandler
    {
        private readonly HttpPostedFileBase _file;
                
        public UploadImagesArchiveFileHandler(HttpPostedFileBase file)
        {
            _file = file;

            FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.PriceTemp));            
        }

        public UploadFileResult Execute()
        {
            if (_file == null || string.IsNullOrEmpty(_file.FileName))
            {
                return new UploadFileResult { Result = false, Error = "Не найден файл." };
            }

            if (!FileHelpers.CheckFileExtension(_file.FileName, EAdvantShopFileTypes.Zip))
            {             
                return new UploadFileResult { Result = false, Error = "Не найден файл." };
            }

            FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
            var fullPath = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, _file.FileName);

            try
            {                
                FileHelpers.DeleteFilesFromPath(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
                _file.SaveAs(fullPath);
                var res = FileHelpers.UnZipFile(fullPath);
                
                if (!res)
                {
                    return new UploadFileResult { Result = false, FilePath = fullPath, Error = "Admin_ImportCsv_ErrorAtUnZip" };                    
                }
            }
            catch (Exception ex)
            {                
                Debug.Log.Error(ex);
                return new UploadFileResult { Result = false, FilePath = fullPath, Error = "Admin_ImportCsv_ErrorAtUploadFile" };
                //ReturnResult(context, "error", Resource.Admin_ImportCsv_ErrorAtUploadFile);
            }            

            FileHelpers.DeleteFile(fullPath);
            
            return new UploadFileResult { Result = true, FilePath = fullPath };
        }
    }
}
