using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.AdditionalMarkers.Service
{
    public class ModuleService
    {
        public static bool CreateTables()
        {
            var created = true;
            var query = "";

            if (created && !ModulesRepository.IsExistsModuleTable("Module","Marker"))
            {
                query = @"CREATE TABLE Module.Marker(
                                MarkerId int not null IDENTITY(1,1),
                                Name nvarchar(100) not null,
                                Color nvarchar(7) not null,
                                ColorName nvarchar(7) not null,
                                Url nvarchar(max) null,
                                Description nvarchar(max) null,
                                OpenNewTab bit not null default(1),
                                SortOrder int not null,
                                CONSTRAINT PK_AM_MarkerId PRIMARY KEY CLUSTERED (MarkerId)
                                )";

                created = NonQuery(query);
            }

            if (created && !ModulesRepository.IsExistsModuleTable("Module","ProductMarker"))
            {
                query = @"CREATE TABLE Module.ProductMarker(
                            ProductId int not null,
                            MarkerId int not null,
                            CONSTRAINT FK_MPM_ProductId FOREIGN KEY (ProductId) REFERENCES Catalog.Product(ProductId) ON DELETE CASCADE,
                            CONSTRAINT FK_MAM_MarkerId FOREIGN KEY (MarkerId) REFERENCES Module.Marker(MarkerId) ON DELETE CASCADE
                            )";

                created = NonQuery(query);
            }

            return created;
        }

        public static bool DropTables()
        {
            var drop = true;

            var query = @"DROP TABLE Module.ProductMarker";
            drop = drop && NonQuery(query);
            query = @"DROP TABLE Module.Marker";
            drop = drop && NonQuery(query);

            return drop;
        }

        public static bool NonQuery(string query, SqlParameter[] parameters = null)
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
            install = ModuleSettings.SetDefaultSettings();
            install = install && CreateTables();
            return install;
        }

        public static bool UnInstall()
        {
            var uninstall = true;
            //uninstall = ModuleSettings.RemoveSettings();
            //uninstall = uninstall && DropTables();
            return uninstall;
        }
    }
}
