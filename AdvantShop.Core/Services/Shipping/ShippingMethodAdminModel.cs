using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Taxes;

namespace AdvantShop.Core.Services.Shipping
{
    public class ShippingMethodAdminModel
    {
        public ShippingMethodAdminModel()
        {
            Params = new Dictionary<string, string>();

            TaxTypes =
                Enum.GetValues(typeof(TaxType))
                    .Cast<TaxType>()
                    .Select(x => new SelectListItem()
                            {
                                Text = (x == TaxType.Other ? "Не выбран" : x.Localize()),
                                Value = ((int) x).ToString()
                            })
                    .ToList();
        }

        public int ShippingMethodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int SortOrder { get; set; }
        public string ShippingType { get; set; }

        public string ShippingTypeLocalized
        {
            get
            {
                var list = AdvantshopConfigService.GetDropdownShippings();
                var type = list.FirstOrDefault(x => x.Value.ToLower() == ShippingType.ToLower());
                return type != null ? type.Text : ShippingType;
            }
        }
        public bool DisplayCustomFields { get; set; }
        public bool DisplayIndex { get; set; }
        public bool ShowInDetails { get; set; }
        public string ZeroPriceMessage { get; set; }
        public string Icon { get; set; }

        public Dictionary<string, string> Params { get; set; }

        public virtual string ModelType
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }

        public virtual string ShippingViewPath
        {
            get { return "_" + ShippingType; }
        }

        public string Payments { get; set; }
        public TaxType TaxType { get; set; }
        public List<SelectListItem> TaxTypes { get; set; }
    }
}
