using System.Linq;
using System.IO;

using AdvantShop.ExportImport;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using AdvantShop.FilePath;
using AdvantShop.Core.UrlRewriter;


namespace AdvantShop.Web.Admin.Handlers.ExportCategories
{
    public class StartingExportCategoriesHandler
    {
        private readonly string _fileName = "export_categories";
        private readonly string _fileExtention = ".csv";

        public string Execute()
        {
            if (CommonStatistic.IsRun) return string.Empty;

            //delete old
            foreach (var item in Directory.GetFiles(FoldersHelper.GetPathAbsolut(FolderType.PriceTemp)).Where(f => f.Contains(_fileName)))
            {
                FileHelpers.DeleteFile(item);
            }

            var exportFeedCsvOptions = new ExportFeedCsvOptions()
            {
                FileName = FoldersHelper.GetPathRelative(FolderType.PriceTemp, _fileName, false),
                FileExtention = "csv",
                CsvEnconing = ExportFeedCsvCategorySettings.CsvEnconing,
                CsvSeparator = ExportFeedCsvCategorySettings.CsvSeparator,
            };

            CsvExportCategories.Factory(
                ExportFeedCsvCategoryService.GetCsvCategories(ExportFeedCsvCategorySettings.FieldMapping),
                exportFeedCsvOptions,
                ExportFeedCsvCategorySettings.FieldMapping,
                ExportFeedCsvCategoryService.GetCsvCategoriesCount()
                ).Process();

            return UrlService.GetAbsoluteLink(FoldersHelper.PhotoFoldersPath[FolderType.PriceTemp] + _fileName + _fileExtention);
        }
    }
}
