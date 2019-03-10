using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Orders;
using Newtonsoft.Json;

namespace AdvantShop.Models.Checkout
{
    public partial class BuyInOneClickModel
    {
        public int ProductId { get; set; }

        public List<CustomOption> CustomOptions { get; set; }

        public List<OptionItem> SelectedOptions { get; set; }

        public Customer Customer { get; set; }

        public string PageEnum { get; set; }
    }

    public partial class BuyOneInClickJsonModel : BaseModel
    {
        public int? OfferId { get; set; }

        public int? ProductId { get; set; }

        public float Amount { get; set; }

        public string AttributesXml { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Comment { get; set; }

        public BuyInOneclickPage Page { get; set; }

        public OrderType OrderType { get; set; }
    }

    public class BuyOneClickResult
    {
        [JsonIgnore]
        public int orderId { get; set; }
        public string error { get; set; }
        public string url { get; set; }
        public string orderNumber { get; set; }
        public string code { get; set; }
        public bool doGo { get; set; }
    }
}