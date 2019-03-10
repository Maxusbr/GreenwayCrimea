using System;
using AdvantShop.ExportImport;

namespace AdvantShop.Core.Common.Attributes
{
    public class CsvFieldsStatusAttribute : Attribute, IAttribute<CsvFieldStatus>
    {
        private readonly CsvFieldStatus _name;
        public CsvFieldsStatusAttribute(CsvFieldStatus name)
        {
            _name = name;
        }

        public CsvFieldStatus Value
        {
            get { return _name; }
        }
    }
}