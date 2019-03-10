using System.IO;
using System;
using System.Collections.Generic;

using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Localization;
using AdvantShop.FilePath;
using AdvantShop.Customers;

namespace AdvantShop.Web.Admin.Handlers.Import
{
    public class GetExampleCustomersFileHandler
    {
        private readonly string _columnSeparator;
        private readonly string _encoding;
        private readonly string _outputFilePath;

        public GetExampleCustomersFileHandler(string outputFilePath, string columnSeparator, string encoding)
        {
            _columnSeparator = columnSeparator;
            _encoding = encoding;
            _outputFilePath = outputFilePath;
        }

        private Dictionary<ECustomerFields, string> customerExample;

        public object Execute()
        {
            if (File.Exists(_outputFilePath))
            {
                File.Delete(_outputFilePath);
            }

            GetCustomerExample();

            using (var streamWriter = new StreamWriter(_outputFilePath, false, System.Text.Encoding.GetEncoding(_encoding)))
            {
                var headerRow = string.Empty;
                var customerRow = string.Empty;
                foreach (ECustomerFields item in Enum.GetValues(typeof(ECustomerFields)))
                {
                    if (item != ECustomerFields.None)
                    {
                        headerRow += item.StrName().ToLower() + _columnSeparator;
                        customerRow += customerExample[item] + _columnSeparator;
                    }
                }

                foreach (var additionalField in CustomerFieldService.GetCustomerFields(true))
                {
                    headerRow += additionalField.Name + _columnSeparator;
                    customerRow += additionalField.Name + _columnSeparator;
                }

                streamWriter.WriteLine(headerRow);
                streamWriter.WriteLine(customerRow);
            }
            return new { Result = true, };
        }

        private void GetCustomerExample()
        {
            customerExample = new Dictionary<ECustomerFields, string>
            {
                { ECustomerFields.FirstName, "Иван" },
                { ECustomerFields.LastName, "Иванов" },
                { ECustomerFields.Patronymic, "Иванович" },
                { ECustomerFields.Phone, "+79999999999" },
                { ECustomerFields.Email, "example@example.com" },
                { ECustomerFields.CustomerGroup, "Постоянный покупатель" },
                { ECustomerFields.Enabled, "True" },
                { ECustomerFields.AdminComment, "Комментарий администратора" },

                { ECustomerFields.City, "Москва" },
                { ECustomerFields.Region, "Московская область" },
                { ECustomerFields.Country, "Российская Федерация" },
                { ECustomerFields.Zip, "012345" },
                { ECustomerFields.Street, "Красная площадь" },
                { ECustomerFields.House, "1" },
                { ECustomerFields.Apartment, "1" },
                { ECustomerFields.Address, "Красная площадь, 1, Царские палаты" },
            };
        }
    }
}
