using System.IO;
using System;
using System.Collections.Generic;

using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;

namespace AdvantShop.Web.Admin.Handlers.Import
{
    public class GetFiledsFromCustomersCsvFile
    {
        private readonly string _outputFilePath;

        private readonly string _columnSeparator;
        private readonly string _encoding;
        private readonly bool _haveHeader;

        public GetFiledsFromCustomersCsvFile(string outputFilePath, string columnSeparator, string encoding, bool haveHeader)
        {
            _outputFilePath = outputFilePath;

            _columnSeparator = columnSeparator;
            _encoding = encoding;
            _haveHeader = haveHeader;
        }

        public object Execute()
        {
            var allFields = new Dictionary<string, string>();
            var headers = new List<string>();
            var firstCustomer = new List<string>();

            if (!File.Exists(_outputFilePath))
            {
                return new { Result = false, Error = "Не найден файл." };
            }

            var importcustomers = new CsvImportCustomers(_outputFilePath, false, _columnSeparator, _encoding, null);
            var csvrows = importcustomers.ReadFirstRecord();

            if (csvrows.Count == 0)
            {
                return new { Result = false, Error = "Не найден файл." };
            }

            if (_haveHeader && csvrows[0].HasDuplicates())
            {
                return new { Result = false, Error = "Найдены дублирующие заголовки." };
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


            foreach (ECustomerFields item in Enum.GetValues(typeof(ECustomerFields)))
            {
                if (item != ECustomerFields.None)
                {
                    allFields.Add(item.StrName().ToLower(), item.Localize());
                }
            }

            foreach (var additionalField in CustomerFieldService.GetCustomerFields(true))
            {
                allFields.Add(additionalField.Name, additionalField.Name);
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
                    firstCustomer.Add(field);
                }
            }

            return new { Result = true, firstCustomer, allFields, headers };
        }
    }
}
