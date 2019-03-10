using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using System.Data;
using System.Data.SqlClient;

namespace AdvantShop.Core.Services.Catalog
{
    public static class ProductLandingPageService
    {

        public static void UpdateProductDescription(int productId, string description)
        {
            if (description.IsNullOrEmpty())
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    "Delete from Module.LandingPageProductDescription where productid=@productid",
                    CommandType.Text, new SqlParameter("@productid", productId));
            }
            else
            {
                var sql =
                    @"if (select count(productid) from Module.LandingPageProductDescription where productid=@productid) = 0
                     Insert into Module.LandingPageProductDescription (productid, description) values (@productid, @description)
                   else
                    update Module.LandingPageProductDescription set description = @description where productid= @productid";

                ModulesRepository.ModuleExecuteNonQuery(sql, CommandType.Text, 
                    new SqlParameter("@productid", productId),
                    new SqlParameter("@description", description));
            }
        }

        public static string GetDescriptionByProductId(int productid, bool forAdmin)
        {
            var desc =
                  ModulesRepository.ModuleExecuteScalar<string>(
                      "select description from Module.LandingPageProductDescription where productid=@productid",
                      CommandType.Text, new SqlParameter("@productid", productid));

            if (string.IsNullOrWhiteSpace(desc) && !forAdmin)
            {
                desc = SettingsLandingPage.LandingPageCommonStatic;
            }

            return desc;
        }
    }
}
