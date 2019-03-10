using System.Web;
using System.IO;

using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.Import;

namespace AdvantShop.Web.Admin.Handlers.Import
{
    public class UploadCsvFileHandler
    {
        private readonly HttpPostedFileBase _file;
        private readonly string _outputFilePath;
               
        public UploadCsvFileHandler(HttpPostedFileBase file, string outputFilePath)
        {
            _file = file;
            _outputFilePath = outputFilePath;
            
            FileHelpers.DeleteFile(outputFilePath);
        }

        public UploadFileResult Execute()
        {
            if (_file == null || string.IsNullOrEmpty(_file.FileName))
            {
                return new UploadFileResult { Result = false, Error = "Не найден файл." };
            }

            _file.SaveAs(_outputFilePath);

            if (!File.Exists(_outputFilePath))
            {
                return new UploadFileResult { Result = false, Error = "Не найден файл." };
            }

            return new UploadFileResult { Result = true, FilePath = _outputFilePath };
        }
    }
}
