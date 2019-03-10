using AdvantShop.Catalog;
using AdvantShop.Core.Services.InplaceEditor;

namespace AdvantShop.Handlers.Inplace
{
    public class InplaceCategoryHandler
    {
        public bool Execute(int id, string content, CategoryInplaceField field)
        {
            var category = CategoryService.GetCategory(id);
            if (category == null)
                return false;

            switch (field)
            {
                case CategoryInplaceField.Description:
                    category.Description = content;
                    break;
                case CategoryInplaceField.BriefDescription:
                    category.BriefDescription = content;
                    break;
            }

            return CategoryService.UpdateCategory(category, true);
        }
    }
}