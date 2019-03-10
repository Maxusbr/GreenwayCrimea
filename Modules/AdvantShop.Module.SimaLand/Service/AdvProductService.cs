using AdvantShop.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Catalog;

namespace AdvantShop.Module.SimaLand.Service
{
    public class AdvProductService
    {

        public static int GetProductIdByArtNo(string artno)
        {
            var pars = new SqlParameter[] { new SqlParameter("@artno", artno), new SqlParameter("@SimaLand", SimaLand.ModuleStringId) };

            var query = @"SELECT ProductId FROM Catalog.Product WHERE ArtNo=@artno";
            if (PSLModuleSettings.WorkOnlySimaLand)
            {
                query += " AND ModifiedBy=@SimaLand";
            }
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text, pars);
        }

        public static bool HasPhoto(int productId)
        {
            var query = @"declare @hasPhoto int = (SELECT COUNT(*) FROM Catalog.Photo WHERE ObjId=@productId and Type='Product')
                            IF (@hasPhoto=0)
                            SELECT 0
                            ELSE
                            SELECT 1";
            return ModulesRepository.ModuleExecuteScalar<bool>(query, CommandType.Text, new SqlParameter("productId", productId));
        }

        public static List<int> GetCategoriesIDsByProductId(int productId)
        {
            var query = @"select CategoryID From Catalog.ProductCategories Where ProductId=" + productId;
            return ModulesRepository.Query<int>(query, CommandType.Text).ToList();
        }

        public static void LinkCategories(int productId, int[] cats)
        {
            var advCatIds = SimalandCategoryService.GetAdvCategoryIdsBySlCatIds(cats);

            if (advCatIds.Length > 0)
            {
                var exlink = GetCategoriesIDsByProductId(productId);
                var childCats = advCatIds.Except(exlink);
                if (childCats.Count() > 0)
                {
                    foreach (var cId in childCats)
                    {
                        ProductService.AddProductLink(productId, cId, 0, true, false);
                    }
                }
            }
        }

        public static bool isSLProduct(string ArtNo)
        {
            var query = "SELECT Count(*) FROM Catalog.Product WHERE ArtNo = '" + ArtNo + "' AND ModifiedBy='" + SimaLand.ModuleStringId+"'";

            var c = ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);

            return c > 0;
        }

        public static void SetNotAvailableProducts(int advCategoryId)
        {
            try
            {
                var query = @"UPDATE Catalog.Offer SET Amount=0 WHERE ProductID in (
                            SELECT Catalog.Product.ProductID FROM Catalog.Product
                            INNER JOIN Catalog.ProductCategories ON Catalog.Product.ProductId = catalog.ProductCategories.ProductID
                            INNER JOIN Catalog.Category ON Catalog.Category.CategoryID = Catalog.ProductCategories.CategoryID
                            WHERE Catalog.ProductCategories.Main = 1 AND Catalog.Category.CategoryID = @advCategoryId)";
                ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text, new SqlParameter("@advCategoryId", advCategoryId));
            }
            catch (Exception ex)
            {
                Diagnostics.Debug.Log.Error(ex);
            }
        }

    }
}
