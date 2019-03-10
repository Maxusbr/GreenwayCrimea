using AdvantShop.Models;

namespace AdvantShop.ViewModel.ProductDetails
{
    public abstract class BaseProductViewModel : BaseModel
    {
        public string CustomViewPath { get; set; }
    }
}
