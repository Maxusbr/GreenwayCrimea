using System.Collections.Generic;
using AdvantShop.Catalog;

namespace AdvantShop.ViewModel.ProductDetails
{
    public class PropductPropertiesViewModel : BaseProductViewModel
    {
        public List<PropertyValue> PropertyValues { get; set; }

        public bool ShowInPlaceEditor { get; set; }

        public bool RenderInplaceAddBlock { get; set; }

        public int ProductId { get; set; }
    }
}