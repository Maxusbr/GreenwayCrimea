//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Threading.Tasks;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Diagnostics;
using AdvantShop.Statistic;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace AdvantShop.ExportImport
{
    public class CsvExportCategories
    {
        private const int MaxCellLength = 60000;
        private readonly string _path;
        private readonly string _encodeType;
        private readonly string _delimeter;
        private readonly List<CategoryFields> _fieldMapping;
        private readonly int _categoriesCount;
        private readonly IEnumerable<ExportFeedCsvCategory> _categories;

        private CsvExportCategories(IEnumerable<ExportFeedCsvCategory> categories, ExportFeedCsvOptions options, List<CategoryFields> fieldMapping,  int categoriesCount)
        {
            _path = options.FileFullPath;
            _encodeType = options.CsvEnconing;
            _delimeter = options.CsvSeparator;
            _fieldMapping = fieldMapping;

            _categories = categories;
            _categoriesCount = categoriesCount;
        }

        public static CsvExportCategories Factory(IEnumerable<ExportFeedCsvCategory> categories, ExportFeedCsvOptions options, List<CategoryFields> fieldMapping, int categoriesCount)
        {
            return new CsvExportCategories(categories, options, fieldMapping, categoriesCount);
        }

        private CsvWriter InitWriter()
        {
            var writer = new CsvWriter(new StreamWriter(_path, false, Encoding.GetEncoding(_encodeType)));
            writer.Configuration.Delimiter = _delimeter;
            return writer;
        }

        public void SaveCategoriesToCsv()
        {
            using (var writer = InitWriter())
            {
                WriteHeader(writer);

                if (_categories == null) return;

                foreach (ExportFeedCsvCategory category in _categories)
                {                    
                    if (!CommonStatistic.IsRun) return;

                    if (_fieldMapping.Contains(CategoryFields.Description) && category.Description.Length > MaxCellLength)
                    {
                        CommonStatistic.WriteLog(string.Format(LocalizationService.GetResource("Core.ExportImport.ExportCsv.TooLargeDescription"), category.Name, category.CategoryId));
                        CommonStatistic.TotalErrorRow++;
                        continue;
                    }

                    if (_fieldMapping.Contains(CategoryFields.BriefDescription) && category.BriefDescription.Length > MaxCellLength)
                    {
                        CommonStatistic.WriteLog(string.Format(LocalizationService.GetResource("Core.ExportImport.ExportCsv.TooLargeBriefDescription"), category.Name, category.CategoryId));
                        CommonStatistic.TotalErrorRow++;
                        continue;
                    }

                    WriteItem(writer, category);
                    CommonStatistic.RowPosition++;
                }
            }
        }

        private void WriteHeader(ICsvWriter writer)
        {
            foreach (var item in _fieldMapping)
                writer.WriteField(item.StrName());
            writer.NextRecord();
        }

        private void WriteItem(ICsvWriter writer, ExportFeedCsvCategory model)
        {
            foreach (var item in _fieldMapping)
            {
                if (item == CategoryFields.CategoryId)
                    writer.WriteField(model.CategoryId);

                else if (item == CategoryFields.Name)
                    writer.WriteField(model.Name);

                else if (item == CategoryFields.Slug)
                    writer.WriteField(model.Slug);

                else if (item == CategoryFields.ParentCategory)
                    writer.WriteField(model.ParentCategory);

                else if (item == CategoryFields.SortOrder)
                    writer.WriteField(model.SortOrder);

                else if (item == CategoryFields.Enabled)
                    writer.WriteField(model.Enabled);

                else if (item == CategoryFields.Hidden)
                    writer.WriteField(model.Hidden);

                else if (item == CategoryFields.BriefDescription)
                    writer.WriteField(model.BriefDescription);

                else if (item == CategoryFields.Description)
                    writer.WriteField(model.Description);

                else if (item == CategoryFields.DisplayStyle)
                    writer.WriteField(model.DisplayStyle);

                else if (item == CategoryFields.Sorting)
                    writer.WriteField(model.Sorting);

                else if (item == CategoryFields.DisplayBrandsInMenu)
                    writer.WriteField(model.DisplayBrandsInMenu);

                else if (item == CategoryFields.DisplaySubCategoriesInMenu)
                    writer.WriteField(model.DisplaySubCategoriesInMenu);

                else if (item == CategoryFields.Tags)
                    writer.WriteField(model.Tags);

                else if (item == CategoryFields.Picture)
                    writer.WriteField(model.Picture);

                else if (item == CategoryFields.MiniPicture)
                    writer.WriteField(model.MiniPicture);

                else if (item == CategoryFields.Icon)
                    writer.WriteField(model.Icon);
                
                else if (item == CategoryFields.Title)
                    writer.WriteField(model.Title);

                else if (item == CategoryFields.H1)
                    writer.WriteField(model.H1);

                else if (item == CategoryFields.MetaKeywords)
                    writer.WriteField(model.MetaKeywords);

                else if (item == CategoryFields.MetaDescription)
                    writer.WriteField(model.MetaDescription);

                else if (item == CategoryFields.PropertyGroups)
                    writer.WriteField(model.PropertyGroups);
            }

            writer.NextRecord();
        }

        public Task Process()
        {
            var ctx = HttpContext.Current;

            CommonStatistic.TotalRow = _categoriesCount;
            return CommonStatistic.StartNew(() =>
            {
                CommonStatistic.IsRun = true;

                if (ctx != null)
                    HttpContext.Current = ctx;
                try
                {
                    SaveCategoriesToCsv();
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    CommonStatistic.WriteLog(ex.Message);
                }
                finally
                {
                    CommonStatistic.IsRun = false;
                }
            });
        }
    }
}