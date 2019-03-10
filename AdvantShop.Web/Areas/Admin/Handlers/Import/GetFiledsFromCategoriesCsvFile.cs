using System.IO;
using System;
using System.Collections.Generic;

using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Web.Infrastructure.ActionResults;

namespace AdvantShop.Web.Admin.Handlers.Import
{
    public class GetFiledsFromCategoriesCsvFile
    {
        private readonly string _outputFilePath;

        private readonly string _columnSeparator;
        private readonly string _encoding;
        private readonly bool _haveHeader;

        public GetFiledsFromCategoriesCsvFile(string outputFilePath, string columnSeparator, string encoding, bool haveHeader)
        {
            _outputFilePath = outputFilePath;

            _columnSeparator = columnSeparator;
            _encoding = encoding;
            _haveHeader = haveHeader;
        }

        public CommandResult Execute()
        {
            var allFields = new Dictionary<string, string>();
            var headers = new List<string>();
            var firstCategory = new List<string>();

            if (!File.Exists(_outputFilePath))
            {
                return new CommandResult { Result = false, Message = "Не найден файл." };
            }

            var importCategories = new CsvImportCategories(_outputFilePath, false, _columnSeparator, _encoding, null);
            var csvrows = importCategories.ReadFirstRecord();

            if (csvrows.Count == 0)
            {
                return new CommandResult { Result = false, Message = "Не найден файл." };
            }

            if (_haveHeader && csvrows[0].HasDuplicates())
            {
                return new CommandResult { Result = false, Message = "Найдены дублирующие заголовки." };
            }

            for (int i = 0; i < csvrows[0].Length; i++)
            {
                var headerName = string.Empty;

                if (_haveHeader)
                {
                    var tempCsv = (csvrows[0][i].Length > 50 ? csvrows[0][i].Substring(0, 49) : csvrows[0][i]);

                    headerName = tempCsv.Trim().ToLower();
                }
                else
                {
                    headerName = LocalizationService.GetResource("Admin.ImportProducts.Empty");
                }
                headers.Add(headerName);
            }


            foreach (CategoryFields item in Enum.GetValues(typeof(CategoryFields)))
            {
                allFields.Add(item.StrName().ToLower(), item.Localize());
            }

            var dataRow = csvrows.Count > 1 ? csvrows[1] : csvrows[0];

            if (dataRow != null)
            {
                foreach (var data in dataRow)
                {
                    var field = string.Empty;
                    if (data != null)
                    {
                        field = data.Length > 50 ? data.Substring(0, 49).HtmlEncode() : data.HtmlEncode();
                    }
                    firstCategory.Add(field);
                }
            }

            return new CommandResult { Result = true, Obj = new { firstCategory, allFields, headers } };
        }
    }
}
