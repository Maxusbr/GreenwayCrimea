using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Models;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.ViewModel.Home
{
    public partial class TopPanelViewModel : BaseModel
    {
        public TopPanelViewModel()
        {
            Currencies = new List<SelectListItem>();
        }

        public bool IsShowDesignConstructor { get; set; }

        public bool IsRegistered { get; set; }

        public bool IsShowAdminLink { get; set; }

        public bool IsDemoEnabled { get; set; }

        public bool IsShowCurrency { get; set; }

        public bool IsShowCity { get; set; }

        public bool IsShowWishList { get; set; }

        public string CurrentCity { get; set; }

        public string WishCount { get; set; }

        public Currency CurrentCurrency { get; set; }

        public List<SelectListItem> Currencies { get; set; }

        public bool IsTemplatePreview { get; set; }

        public bool HasTemplate { get; set; }

        public string TemplatePreviewName { get; set; }

        public string BuyTemplateLink { get; set; }

    }
}