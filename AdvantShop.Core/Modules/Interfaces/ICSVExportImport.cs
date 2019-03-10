//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.ExportImport;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface ICSVExportImport : IModule
    {
        IList<CSVField> GetCSVFields();

        string PrepareField(CSVField field, int productId, string columnSeparator, string propertySeparator);

        bool ProcessField(CSVField field, int productId, string value, string columnSeparator, string propertySeparator);
    }
}
