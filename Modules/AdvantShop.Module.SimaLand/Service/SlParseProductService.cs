using AdvantShop.Core.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public class SlParseProductService
    {
        public static void PrePareProducts()
        {
            ClearParseProduct();
            var query = @"INSERT INTO Module." + ModuleTables.SimalandParseProduct + " SELECT ProductId FROM Module." + ModuleTables.SimalandProducts;
            ModuleService.Query(query);
        }

        public static void ClearParseProduct()
        {
            var query = @"DELETE FROM Module." + ModuleTables.SimalandParseProduct;
            ModuleService.Query(query);
        }

        public static void Remove(int advProductId)
        {
            var query = @"DELETE FROM Module." + ModuleTables.SimalandParseProduct + " WHERE ProductId = " + advProductId;
            ModuleService.Query(query);
        }

        public static void ExistProductIdAsNotAvailable()
        {
            var query = @"UPDATE Catalog.Offer SET Amount = 0 WHERE OfferId in (SELECT Offer.OfferId FROM Module."+ModuleTables.SimalandParseProduct+ @"
                            INNER JOIN Catalog.Product ON Product.ProductId = " + ModuleTables.SimalandParseProduct + @".ProductId
                            INNER JOIN Catalog.Offer ON Offer.ProductId = Product.ProductId)
                            DELETE FROM Module." + ModuleTables.SimalandParseProduct;
            ModuleService.Query(query);
        }

        public static bool ExistProductsToUpdate()
        {
            var query = @"SELECT COUNT(*) FROM Module." + ModuleTables.SimalandParseProduct;
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text) > 0;
        }

        public static bool ContainsToUpdate(int slProductId)
        {
            var query = @"SELECT COUNT(*) FROM Module." + ModuleTables.SimalandProducts + " WHERE SlProductId = " + slProductId;
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text) > 0;
        }

        public static List<int> GetSlProductIds()
        {
            var query = @"SELECT SlProductId FROM Module." + ModuleTables.SimalandProducts;

            return ModulesRepository.Query<int>(query, CommandType.Text).ToList(); 
        }

        public static void NotAvailableProduct(int slId)
        {
            var query = @"UPDATE Catalog.Offer SET Amount = 0 WHERE OfferId in (SELECT Offer.OfferId FROM Module." + ModuleTables.SimalandParseProduct + @"
                            INNER JOIN Catalog.Product ON Product.ProductId = " + ModuleTables.SimalandProducts + @".ProductId
                            INNER JOIN Catalog.Offer ON Offer.ProductId = Product.ProductId
                            WHERE "+ModuleTables.SimalandProducts+".SlProductId = "+slId+")";
            ModuleService.Query(query);
        }

    }
}
