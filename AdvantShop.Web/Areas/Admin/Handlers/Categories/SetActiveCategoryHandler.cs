using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Categories
{
    public class SetActiveCategoryHandler
    {
        private readonly int _categoryId;
        private readonly bool _active;


        public SetActiveCategoryHandler(int categoryId, bool active)
        {
            _categoryId = categoryId;
            _active = active;
        }

        public bool Execute()
        {
            var category = CategoryService.GetCategory(_categoryId);

            if (category == null)
            {
                return false;
            }
            category.Enabled = _active;
            CategoryService.UpdateCategory(category, true);

            return true;
        }
    }
}
