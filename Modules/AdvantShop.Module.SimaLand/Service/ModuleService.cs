using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Core.Modules;
using System.Data;
using AdvantShop.Diagnostics;
using System.Data.SqlClient;
using AdvantShop.Module.SimaLand.Models;
//using System.Data.Entity;
using System.IO;

namespace AdvantShop.Module.SimaLand.Service
{
    public class ModuleService
    {
        private static bool CreateTables()
        {
            var created = true;
            if (!ModulesRepository.IsExistsModuleTable("Module", ModuleTables.SimalandCategory))
            {
                var query = @"CREATE TABLE Module." + ModuleTables.SimalandCategory + @"(
                                id int not null,
                                name nvarchar(255) not null,
                                level int not null,
                                is_adult bit not null,
                                is_leaf bit not null,
                                path nvarchar(255) not null,
                                custom bit not null,
                                full_slug nvarchar(255) not null,
                                CategoryId int null,
                                hidden bit not null default 0,
                                CONSTRAINT PK_SimalandCategory PRIMARY KEY CLUSTERED (Id))";
                Query(query);
                created = ModulesRepository.IsExistsModuleTable("Module", ModuleTables.SimalandCategory);
            }

            if (created && !ModulesRepository.IsExistsModuleTable("Module", ModuleTables.SimalandProducts))
            {
                var query = @"CREATE TABLE Module." + ModuleTables.SimalandProducts + @"(
                                ProductId int not null,
                                SlProductId int not null,
                                UpdateDate datetime not null default GETDATE(),
                                CONSTRAINT FK_SimalandProducts FOREIGN KEY (ProductId) REFERENCES Catalog.Product(ProductId) ON DELETE CASCADE)";
                Query(query);
                created = ModulesRepository.IsExistsModuleTable("Module", ModuleTables.SimalandProducts);
            }

            if (created && !ModulesRepository.IsExistsModuleTable("Module", ModuleTables.SimalandParseProduct))
            {
                var query = @"CREATE TABLE Module." + ModuleTables.SimalandParseProduct + @"(
                                ProductId int not null,
                                CONSTRAINT FK_SimalandParseProduct FOREIGN KEY (ProductId) REFERENCES Catalog.Product(ProductId) ON DELETE CASCADE)";
                Query(query);
                created = ModulesRepository.IsExistsModuleTable("Module", ModuleTables.SimalandParseProduct);
            }

            return created;
        }

        private static bool DeleteTables()
        {
            var deleted = true;
            if (ModulesRepository.IsExistsModuleTable("Module", ModuleTables.SimalandCategory))
            {
                var query = @"DROP TABLE Module." + ModuleTables.SimalandCategory;
                Query(query);
                deleted = !ModulesRepository.IsExistsModuleTable("Module", ModuleTables.SimalandCategory);
            }
            if (ModulesRepository.IsExistsModuleTable("Module", ModuleTables.SimalandProducts))
            {
                var query = @"DROP TABLE Module." + ModuleTables.SimalandProducts;
                Query(query);
                deleted = !ModulesRepository.IsExistsModuleTable("Module", ModuleTables.SimalandProducts);
            }
            return deleted;
        }

        private static bool ReturnViewOrder()
        {
            if (PSLModuleSettings.LinkInViewOrder && System.IO.File.Exists(PSLModuleSettings.PathToBackFile))
            {
                System.IO.File.Copy(PSLModuleSettings.PathToBackFile, PSLModuleSettings.PathToFile, true);

                PSLModuleSettings.LinkInViewOrder = false;
            }
            return true;
        }

        public static bool ExistRows()
        {
            var query = @"SELECT COUNT(*) FROM Module." + ModuleTables.SimalandCategory;
            var count = ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);
            return count > 0;
        }

        public static bool Query(string query, SqlParameter[] parameters = null)
        {
            try
            {
                if (parameters != null)
                {
                    ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text, parameters);
                }
                else
                {
                    ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogService.ErrLog(ex.Message);
                Debug.Log.Error(ex);
                return false;
            }
        }
        
        public static bool Install()
        {
            var install = true;
            install = CreateTables();
            return install;
        }

        public static bool UnInstall()
        {
            var uninstall = true;
            uninstall = DeleteTables();
            uninstall = uninstall ? ReturnViewOrder() : uninstall;
            return uninstall;
        }
    }

    public class ModuleTables
    {
        public static string SimalandCategory = "SimalandCategory";
        public static string SimalandProducts = "SimalandProducts";
        public static string SimalandParseProduct = "SimalandParseProduct";
    }

    //public partial class DatabaseContext : DbContext
    //{
    //    public DatabaseContext() : base("AdvantConnectionString")
    //    {
    //    }
    //    public virtual DbSet<SimalandCategory> SimalandCategory { get; set; }
    //}
}
