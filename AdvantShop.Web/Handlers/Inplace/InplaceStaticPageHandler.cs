using AdvantShop.CMS;
using AdvantShop.Core.Services.InplaceEditor;

namespace AdvantShop.Handlers.Inplace
{
    public class InplaceStaticPageHandler
    {
        public bool Execute(int id, string content, StaticPageInplaceField field)
        {
            var page = StaticPageService.GetStaticPage(id);
            if (page == null)
                return false;

            switch (field)
            {
                case StaticPageInplaceField.PageText:
                    page.PageText = content;
                    break;
            }
            
            StaticPageService.UpdateStaticPage(page);
            return true;
        }
    }
}