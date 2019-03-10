using System.IO;
using System.Collections.Generic;

using AdvantShop.Statistic;
using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Web.Admin.Handlers.Import
{
    public class StartImportCustomersHandler
    {
        private readonly string _inputFilePath;
        private readonly string _columnSeparator;        
        private readonly string _encoding;
        private readonly bool _haveHeader;        

        private readonly Dictionary<string, int> _fieldMapping;


        public StartImportCustomersHandler(List<string> selectedCustomersFields, string inputFilePath, string columnSeparator,  string encoding, bool haveHeader)
        {
            _inputFilePath = inputFilePath;

            _columnSeparator = columnSeparator;            
            _encoding = encoding;
            _haveHeader = haveHeader;
            
            _fieldMapping = new Dictionary<string, int>();
            for (var index = 0; index < selectedCustomersFields.Count; index++)
            {
                _fieldMapping.Add(selectedCustomersFields[index], index);
            }
        }

        public object Execute()
        {                       
            if (!_fieldMapping.ContainsKey(ECustomerFields.Email.StrName()) && !_fieldMapping.ContainsKey(ECustomerFields.Email.StrName()))
            {
                return new { Result = false, error = "Resource.Admin_ImportCsv_SelectEmail" };
            }

            if (!File.Exists(_inputFilePath))
            {
                return new { Result = false, error = "Файл не найден" };
            }

            if (CommonStatistic.IsRun)
            {
                return new { };
            }

            CommonStatistic.Init();
            CommonStatistic.CurrentProcess = "import/importCustomers";
            CommonStatistic.CurrentProcessName = "Загрузка покупателей";
            
            CommonStatistic.IsRun = true;
            var importCustomers = new CsvImportCustomers(_inputFilePath, _haveHeader, _columnSeparator, _encoding, _fieldMapping);
            importCustomers.Process();
            
            return new { Result = true };
        }
    }
}
