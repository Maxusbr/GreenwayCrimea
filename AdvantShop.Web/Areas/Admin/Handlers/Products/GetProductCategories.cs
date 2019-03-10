using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Products
{
    public class GetProductCategories
    {
        private readonly int _productId;

        public GetProductCategories(int productId)
        {
            _productId = productId;
        }

        public List<KeyValuePair<string, string>> Execute()
        {
            var result = new List<KeyValuePair<string, string>>();

            var productCategoryIds = ProductService.GetCategoriesIDsByProductId(_productId);
            if (productCategoryIds != null)
            {
                var mainCategoryId = ProductService.GetFirstCategoryIdByProductId(_productId);

                foreach (var categoryId in productCategoryIds.OrderByDescending(x => x == mainCategoryId))
                {
                    var parentCategories = CategoryService.GetParentCategories(categoryId);

                    var sb = new StringBuilder();
                    for (int i = parentCategories.Count - 1; i >= 0; i--)
                    {
                        if (sb.Length == 0)
                        {
                            sb.Append(parentCategories[i].Name);
                        }
                        else
                        {
                            sb.Append(" → " + parentCategories[i].Name);
                        }
                    }
                    if (ProductService.IsMainLink(_productId, categoryId))
                        sb.AppendFormat(" ({0})", "Основная"); //  Resource.Admin_Product_MainCategory

                    result.Add(new KeyValuePair<string, string>(categoryId.ToString(), sb.ToString()));
                }
            }

            return result;
        }
    }
}
