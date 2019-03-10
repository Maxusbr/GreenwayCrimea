using AdvantShop.Orders;

namespace AdvantShop.ViewModel.Shared
{
    public class CompareViewModel
    {
        public CompareViewModel()
        {
            
        }

        public CompareViewModel(int offerId)
        {
            NgOfferId = offerId.ToString();
            Checked = ShoppingCartService.CurrentCompare.Find(x => x.OfferId == offerId) != null;
        }

        public string NgOfferId { get; set; }

        public string NgNameCallbackInit { get; set; }

        public bool Checked { get; set; }

        public string Mode { get; set; }
    }
}