using System.IO;
using System;
using System.Collections.Generic;

using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Web.Infrastructure.ActionResults;

namespace AdvantShop.Web.Admin.Handlers.Import
{
    public class GetFiledsFromCsvFile
    {
        private readonly string _outputFilePath;

        private readonly string _columnSeparator;
        private readonly string _propertySeparator;
        private readonly string _propertyValueSeparator;
        private readonly string _encoding;
        private readonly bool _haveHeader;

        public GetFiledsFromCsvFile(string outputFilePath, string columnSeparator, string propertySeparator, string propertyValueSeparator, string encoding, bool haveHeader)
        {
            _outputFilePath = outputFilePath;

            _columnSeparator = columnSeparator;
            _propertySeparator = propertySeparator;
            _propertyValueSeparator = propertyValueSeparator;
            _encoding = encoding;
            _haveHeader = haveHeader;
        }

        public CommandResult Execute()
        {
            var allFields = new Dictionary<string, string>();
            var headers = new List<string>();
            var firstProduct = new List<string>();

            if (!File.Exists(_outputFilePath))
            {
                return new CommandResult { Result = false, Message = "Не найден файл." };
            }

            var csvrows = CsvImport.Factory(_outputFilePath, false, false, _columnSeparator, _encoding, null, _propertySeparator, _propertyValueSeparator).ReadFirst2();
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


            foreach (ProductFields item in Enum.GetValues(typeof(ProductFields)))
            {
                allFields.Add(item.StrName().ToLower(), item.Localize());
            }

            foreach (var moduleField in GetModuleFields())
            {
                allFields.Add(moduleField.StrName.ToLower(), moduleField.DisplayName);
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
                    firstProduct.Add(field);
                }
            }

            return new CommandResult { Result = true, Obj = new { firstProduct, allFields, headers } };
        }


        private List<CSVField> GetModuleFields()
        {
            var result = new List<CSVField>();
            foreach (var csvExportImportModule in AttachedModules.GetModules<ICSVExportImport>())
            {
                var classInstance = (ICSVExportImport)Activator.CreateInstance(csvExportImportModule, null);
                if (ModulesRepository.IsActiveModule(classInstance.ModuleStringId) && classInstance.CheckAlive())
                {
                    result.AddRange(classInstance.GetCSVFields());
                }
            }
            return result;
        }
    }
}
