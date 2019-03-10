using System.Collections.Generic;

namespace AdvantShop.Web.Admin.ViewModels.Home
{
    public class OrderSourcesDasboardViewModel
    {
        public List<OrderSourceItemDasboardViewModel> OrderSources { get; set; } 
    }

    public class OrderSourceItemDasboardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int OrdersCount { get; set; }
        public int Percent { get; set; }
    }
}
