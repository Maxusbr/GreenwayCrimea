using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Taxes;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class TaxModel
    {
        public int TaxId { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public float Rate { get; set; }

        public TaxType TaxType { get; set; }
        public string TaxTypeFormatted { get { return TaxType.Localize(); } }

        private bool? _isDefault;
        public bool IsDefault
        {
            get
            {
                return _isDefault ?? (_isDefault = (TaxId == SettingsCatalog.DefaultTaxId)).Value;
            }
            set { _isDefault = value; }
        }

        public int ProductsCount { get; set; }

        public bool CanBeDeleted
        {
            get { return !IsDefault && ProductsCount == 0; }
        }
    }
}
