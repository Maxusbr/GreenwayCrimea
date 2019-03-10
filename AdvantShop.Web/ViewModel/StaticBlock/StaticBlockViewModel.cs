using System.Web;

namespace AdvantShop.ViewModel.StaticBlock
{
    public class StaticBlockViewModel
    {
        public string CssClass { get; set; }

        public IHtmlString InplaceAttributes { get; set; }

        public string Content { get; set; }

        public bool OnlyContent { get; set; }

        public string Key { get; set; }

        public bool CanUseInplace { get; set; }
    }
}