using AdvantShop.CMS;
using AdvantShop.Core.Services.InplaceEditor;

namespace AdvantShop.Handlers.Inplace
{
    public class InplaceStaticBlockHandler
    {
        public bool Execute(int id, string content, StaticBlockInplaceField field)
        {
            var sb = StaticBlockService.GetPagePart(id);
            if (sb == null)
                return false;

            sb.Content = content;

            StaticBlockService.UpdatePagePart(sb);
            return true;
        }
    }
}