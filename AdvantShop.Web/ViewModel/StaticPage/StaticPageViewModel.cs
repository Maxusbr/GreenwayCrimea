using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Models;

namespace AdvantShop.ViewModel.StaticPage
{
    public partial class StaticPageViewModel : BaseModel
    {
        public StaticPageViewModel()
        {
            SubPages = new List<StaticPageViewModel>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string UrlPath { get; set; }

        public string H1 { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public List<StaticPageViewModel> SubPages { get; set; }
    }
}