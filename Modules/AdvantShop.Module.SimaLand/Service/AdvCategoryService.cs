using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Module.SimaLand.Models;

namespace AdvantShop.Module.SimaLand.Service
{
    public class AdvCategoryService
    {
        public static List<Category> GetAdvCategory(int parent)
        {
            var query = @"SELECT * FROM Catalog.Category WHERE ParentCategory=" + parent + " AND CategoryId<>0";
            return ModulesRepository.Query<Category>(query, CommandType.Text).ToList();
        }
        
        public static int GetParent(int categoryId)
        {
            var query = @"SELECT ParentCategory FROM Catalog.Category WHERE CategoryId=" + categoryId;
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);
        }        

        public static bool IsExistCategory(int id)
        {
            var query = @"SELECT Count(*) FROM Catalog.Category WHERE CategoryId = @id";
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text, new SqlParameter("@id", id)) > 0;
        }
    }

}
