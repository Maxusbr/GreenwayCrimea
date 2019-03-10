using AdvantShop.Models;

namespace AdvantShop.ViewModel.Common
{
    public partial class ShoppingCartViewModel: BaseModel
    {
        public string Amount { get; set; }
        public string Type { get; set; }
    }
}