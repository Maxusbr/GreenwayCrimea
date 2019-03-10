using System.Collections.Generic;

namespace AdvantShop.ViewModel.ProductDetails
{
    public class ShippingsViewModel
    {
        public string City { get; set; }

        public int CityId { get; set; }

        public List<ShippingItemModel> Shippings { get; set; }

        public object AdvancedObj { get; set; }
    }

    public class ShippingItemModel
    {
        //public string Type { get; set; }

        //public string Ext { get; set; }

        public string Name { get; set; }

        public string DeliveryTime { get; set; }

        public string Rate { get; set; }

    }

}