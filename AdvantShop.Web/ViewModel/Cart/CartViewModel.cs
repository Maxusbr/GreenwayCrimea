using AdvantShop.Orders;

namespace AdvantShop.ViewModel.Cart
{
    public class CartViewModel
    {
        public ShoppingCart Cart { get; set; }

        public bool IsDemo { get; set; }

        public bool ShowConfirmButton { get; set; }

        public bool ShowBuyOneClick { get; set; }

        public int PhotoWidth { get; set; }
    }
}