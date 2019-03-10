using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.LastOrder.Service
{
    public class ModuleService
    {
        static string ModuleID = LastOrder.ModuleStringId;

        public static bool CreateTables()
        {
            var created = true;

            if (created && !ModulesRepository.IsExistsModuleTable("Module", ModuleID))
            {
                var query = @"Create Table Module." + ModuleID + @"(
                                ProductId int not null,
                                FakeDateTime datetime not null,
                                Name nvarchar(max) null,
                                City nvarchar(max) null,
                                CONSTRAINT FK_FNP_ProductID FOREIGN KEY (ProductId) REFERENCES Catalog.Product(ProductId) ON DELETE CASCADE)";
                SqlNonQuery(query);
                created = ModulesRepository.IsExistsModuleTable("Module", ModuleID);
            }

            return created;
        }

        public static bool DropTables()
        {
            var deleted = true;

            if (deleted && ModulesRepository.IsExistsModuleTable("Module", ModuleID))
            {
                var query = @"DROP TABLE Module." + ModuleID;
                SqlNonQuery(query);
                deleted = !ModulesRepository.IsExistsModuleTable("Module", ModuleID);
            }
            return deleted;
        }

        public static bool SqlNonQuery(string query, SqlParameter[] parameters = null)
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
                Debug.Log.Error(ex);
                return false;
            }
        }

        public static bool Install()
        {
            var install = true;
            install = install && ModuleSettings.SetDefaultSettings();
            install = install && CreateTables();
            return install;
        }

        public static bool UnInstall()
        {
            var uninstall = true;
            uninstall = uninstall && ModuleSettings.RemoveSettings();
            uninstall = uninstall && DropTables();
            return uninstall;
        }
    }
}
