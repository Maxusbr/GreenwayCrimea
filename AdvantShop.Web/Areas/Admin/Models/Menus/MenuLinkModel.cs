using AdvantShop.CMS;

namespace AdvantShop.Web.Admin.Models.Menus
{
    public class MenuLinkModel
    {
        public EMenuItemUrlType Type { get; set; }

        public int BrandId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int StaticPageId { get; set; }
        public int NewsId { get; set; }
    }
}
