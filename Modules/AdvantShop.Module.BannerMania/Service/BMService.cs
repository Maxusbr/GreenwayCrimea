using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using System;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Module.BannerMania.Models;
using AdvantShop.Helpers;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using AdvantShop.Catalog;

namespace AdvantShop.Module.BannerMania.Service
{
    public class BMService
    {
        public const string imagesPath = "userfiles/Modules/BannerMania/Pictures/";

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
                Debug.Log.Error(ex);
                return false;
            }
        }

        public static bool Install()
        {
            var install = BMSettings.SetDefaultSettings();

            var isSuccessQuery = ModulesRepository.IsExistsModuleTable("Module", "BannerMania");
            if (!isSuccessQuery)
            {
                isSuccessQuery = Query(@"CREATE TABLE [Module].[BannerMania]
                                        (
                                            [BannerId] [int] IDENTITY(1, 1) NOT NULL,
	                                        [EntityId] [int] NOT NULL,
	                                        [EntityName] [nvarchar](250) NOT NULL,
	                                        [EntityType] [nvarchar](50) NOT NULL,
	                                        [ImagePath] [nvarchar](250) NULL,
	                                        [Placement] [nvarchar](250) NOT NULL,
	                                        [URL] [nvarchar](max) NULL,
	                                        [NewWindow] [bit] NOT NULL,
                                            [Enabled] [bit] NOT NULL
                                        ) ON [PRIMARY]

                                        ALTER TABLE [Module].[BannerMania] ADD CONSTRAINT
	                                    PK_BannerMania PRIMARY KEY CLUSTERED 
	                                    (
	                                        [BannerId]
	                                    ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");

            }

            return isSuccessQuery && install;
        }

        public static bool Update()
        {
            var isUpdated = true;
            if (ModulesRepository.IsExistsModuleTable("Module", "BannerMania"))
            {
                if (!IsExistColumn("Enabled"))
                {
                    isUpdated &= Query("ALTER TABLE [Module].[BannerMania] ADD [Enabled] [bit] NOT NULL");
                }
            }

            return isUpdated;
        }

        private static bool IsExistColumn(string columnName)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(
                "IF COLUMNPROPERTY(OBJECT_ID('[Module].[BannerMania]'), '" + columnName + "', 'AllowsNull') IS NOT NULL SELECT 1 ELSE SELECT 0",
                CommandType.Text) > 0;
        }

        public static bool UnInstall()
        {
            return true;
        }

        //-----------------------------------------------------------------------------

        private static BannerEntity GetBannerEntityFromReader(SqlDataReader reader)
        {
            return new BannerEntity
            {
                BannerId = SQLDataHelper.GetInt(reader, "BannerId"),
                EntityId = SQLDataHelper.GetInt(reader, "EntityId"),
                EntityName = SQLDataHelper.GetString(reader, "EntityName"),
                EntityType = (EntityType)Enum.Parse(typeof(EntityType), SQLDataHelper.GetString(reader, "EntityType"), true),
                ImagePath = SQLDataHelper.GetString(reader, "ImagePath"),
                Placement = (PlacementType)Enum.Parse(typeof(PlacementType), SQLDataHelper.GetString(reader, "Placement"), true),
                URL = SQLDataHelper.GetString(reader, "URL"),
                NewWindow = SQLDataHelper.GetBoolean(reader, "NewWindow"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled")
            };
        }

        public static BannerEntity GetBannerEntity(int bannerId)
        {
            return ModulesRepository.ModuleExecuteReadOne<BannerEntity>(
                "SELECT * FROM [Module].[BannerMania] WHERE [BannerId] = @BannerID",
                CommandType.Text,
                GetBannerEntityFromReader,
                new SqlParameter("@BannerID", bannerId));
        }

        public static BannerEntity GetBannerEntity(int entityId, EntityType entityType, PlacementType placementType)
        {
            return ModulesRepository.ModuleExecuteReadOne<BannerEntity>(
                "SELECT * FROM [Module].[BannerMania] WHERE [EntityId] = @EntityID AND [EntityType] = @EntityType AND [Placement] = @Placement",
                CommandType.Text,
                GetBannerEntityFromReader,
                new SqlParameter("@EntityID", entityId),
                new SqlParameter("@EntityType", entityType.ToString()),
                new SqlParameter("@Placement", placementType.ToString()));
        }
        
        public static List<BannerEntity> GetBannerEntities()
        {
            return ModulesRepository.ModuleExecuteReadList<BannerEntity>(
                "SELECT * FROM [Module].[BannerMania]", CommandType.Text, GetBannerEntityFromReader);
        }

        public static List<BannerEntity> GetBannerEntitiesByEntityId(int entityId)
        {
            return ModulesRepository.ModuleExecuteReadList<BannerEntity>(
                "SELECT * FROM [Module].[BannerMania] WHERE [EntityId] = @EntityID",
                CommandType.Text, GetBannerEntityFromReader, new SqlParameter("@EntityID", entityId));
        }

        public static List<BannerEntity> GetBannerEntitiesByEntityType(EntityType entityType)
        {
            return ModulesRepository.ModuleExecuteReadList<BannerEntity>(
                "SELECT * FROM [Module].[BannerMania] WHERE [EntityType] = @EntityType",
                CommandType.Text,
                GetBannerEntityFromReader,
                new SqlParameter("@EntityType", entityType.ToString()));
        }

        public static List<BannerEntity> GetBannerEntitiesByEntityIdAndType(int entityId, EntityType entityType)
        {
            return ModulesRepository.ModuleExecuteReadList<BannerEntity>(
                "SELECT * FROM [Module].[BannerMania] WHERE [EntityId] = @EntityID AND [EntityType] = @EntityType",
                CommandType.Text,
                GetBannerEntityFromReader,
                new SqlParameter("@EntityID", entityId),
                new SqlParameter("@EntityType", entityType.ToString()));
        }

        public static void AddBannerEntity(BannerEntity bannerEntity)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                @"INSERT INTO [Module].[BannerMania] ([EntityId], [EntityName], [EntityType], [ImagePath], [Placement], [URL], [NewWindow], [Enabled])
                VALUES (@EntityId, @EntityName, @EntityType, @ImagePath, @Placement, @URL, @NewWindow, @Enabled)", CommandType.Text,
                new SqlParameter("@EntityId", bannerEntity.EntityId),
                new SqlParameter("@EntityName", bannerEntity.EntityName),
                new SqlParameter("@EntityType", bannerEntity.EntityType.ToString()),
                new SqlParameter("@ImagePath", bannerEntity.ImagePath ?? string.Empty),
                new SqlParameter("@Placement", bannerEntity.Placement.ToString()),
                new SqlParameter("@URL", bannerEntity.URL ?? string.Empty),
                new SqlParameter("@NewWindow", bannerEntity.NewWindow),
                new SqlParameter("@Enabled", bannerEntity.Enabled));
        }

        public static void UpdateBannerEntity(BannerEntity bannerEntity)
        {
            ModulesRepository.ModuleExecuteNonQuery(@"UPDATE [Module].[BannerMania] SET
                                                      [ImagePath] = @ImagePath, 
                                                      [URL] = @URL, 
                                                      [NewWindow] = @NewWindow,
                                                      [Enabled] = @Enabled
                                                      WHERE [EntityId] = @EntityID AND [EntityType] = @EntityType AND [Placement] = @Placement",
                CommandType.Text,
                new SqlParameter("@EntityID", bannerEntity.EntityId),
                new SqlParameter("@ImagePath", bannerEntity.ImagePath ?? string.Empty),
                new SqlParameter("@EntityType", bannerEntity.EntityType.ToString()),
                new SqlParameter("@Placement", bannerEntity.Placement.ToString()),
                new SqlParameter("@URL", bannerEntity.URL ?? string.Empty),
                new SqlParameter("@NewWindow", bannerEntity.NewWindow),
                new SqlParameter("@Enabled", bannerEntity.Enabled));
        }

        public static void UpdateBannerEntityById(BannerEntity bannerEntity)
        {
            ModulesRepository.ModuleExecuteNonQuery(@"UPDATE [Module].[BannerMania] SET
                                                      [ImagePath] = @ImagePath, 
                                                      [Placement] = @Placement, 
                                                      [URL] = @URL, 
                                                      [NewWindow] = @NewWindow,
                                                      [Enabled] = @Enabled
                                                      WHERE [BannerId] = @BannerID",
                CommandType.Text,
                new SqlParameter("@BannerID", bannerEntity.BannerId),
                new SqlParameter("@ImagePath", bannerEntity.ImagePath ?? string.Empty),
                new SqlParameter("@Placement", bannerEntity.Placement.ToString()),
                new SqlParameter("@URL", bannerEntity.URL ?? string.Empty),
                new SqlParameter("@NewWindow", bannerEntity.NewWindow),
                new SqlParameter("@Enabled", bannerEntity.Enabled));
        }

        public static void DeleteBannerEntity(int entityId, PlacementType placement)
        {
            ModulesRepository.ModuleExecuteNonQuery("DELETE FROM [Module].[BannerMania] WHERE [EntityId] = @EntityID AND [Placement] = @Placement",
                CommandType.Text,
                new SqlParameter("@EntityId", entityId),
                new SqlParameter("@Placement", placement.ToString()));
        }

        public static void DeleteBannerEntity(int bannerId)
        {
            ModulesRepository.ModuleExecuteNonQuery("DELETE FROM [Module].[BannerMania] WHERE [BannerId] = @BannerID",
                CommandType.Text,
                new SqlParameter("@BannerID", bannerId));
        }
        
        public static void UpdateImagePath(int entityId, EntityType entityType, PlacementType placement, string imagePath)
        {
            ModulesRepository.ModuleExecuteNonQuery("UPDATE [Module].[BannerMania] SET [ImagePath] = @ImagePath " +
                "WHERE [EntityId] = @EntityID AND [EntityType] = @EntityType AND [Placement] = @Placement",
                CommandType.Text,
                new SqlParameter("@EntityId", entityId),
                new SqlParameter("@EntityType", entityType.ToString()),
                new SqlParameter("@Placement", placement.ToString()),
                new SqlParameter("@ImagePath", imagePath));
        }

        public static void SetBannerEntityNewWindow(int bannerId, bool newWindow)
        {
            ModulesRepository.ModuleExecuteNonQuery("UPDATE [Module].[BannerMania] SET [NewWindow] = @NewWindow WHERE [BannerId] = @BannerID",
                                          CommandType.Text,
                                          new SqlParameter("@BannerID", bannerId),
                                          new SqlParameter("@NewWindow", newWindow));
        }

        public static void SetBannerEntityEnabled(int bannerId, bool enabled)
        {
            ModulesRepository.ModuleExecuteNonQuery("UPDATE [Module].[BannerMania] SET [Enabled] = @Enabled WHERE [BannerId] = @BannerID",
                                          CommandType.Text,
                                          new SqlParameter("@BannerID", bannerId),
                                          new SqlParameter("@Enabled", enabled));
        }

        //-----------------------------------------------------------------------------

        public static string GetEntityNameByEntityId(int entityId, EntityType entityType)
        {
            return ModulesRepository.ModuleExecuteScalar<string>(
                string.Format("SELECT [Name] FROM [Catalog].{0} WHERE {1} = @EntityID",
                entityType == EntityType.Products ? "[Product]" : "[Category]",
                entityType == EntityType.Products ? "[ProductId]" : "[CategoryID]"),
                CommandType.Text,
                new SqlParameter("@EntityID", entityId));
        }

        public static string GetPlacementTypeName(string name)
        {
            switch (name)
            {
                case "UnderDeliveryInfo": return "Под инфо о доставке";
                case "AboveDeliveryInfo": return "Над инфо о доставке";
                case "AboveFooter": return "Над подвалом";
                case "UnderMenu": return "Под блоком меню";
                case "AboveFilter": return "Над фильтром";
                case "UnderFilter": return "Под фильтром";
                default: return "Default placement";
            }
        }

        public static string GetEntityTypeName(string name)
        {
            switch (name)
            {
                case "Products": return "Для отдельных товаров";
                case "ProductsByCategories": return "Для товаров по категориям";
                case "Categories": return "Для категорий товаров";
                default: return "Default entity";
            }
        }
        //-----------------------------------------------------------------------------
        public static string GetPath(string imagePathBase)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("~/") + imagePathBase;
        }

        public static List<Category> GetAllCategories()
        {
            return ModulesRepository.ModuleExecuteReadList<Category>(
                "SELECT [CategoryID], [Name], [ParentCategory], [SortOrder] FROM [Catalog].[Category] Where [CategoryId] <> 0 AND [Enabled] = 1 AND [HirecalEnabled] = 1",
                    CommandType.Text,
                    reader => new Category
                    {
                        CategoryId = SQLDataHelper.GetInt(reader, "CategoryId"),
                        Name = SQLDataHelper.GetString(reader, "Name"),
                        ParentCategoryId = SQLDataHelper.GetInt(reader, "ParentCategory"),
                        SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                    });
        }

        public static void LoadAllCategories(List<Category> categories, List<ShortCategory> list, int categoryId, string offset)
        {
            foreach (var category in categories.Where(c => c.ParentCategoryId == categoryId).OrderBy(c => c.SortOrder).ToList())
            {
                list.Add(new ShortCategory(category.CategoryId, HttpUtility.HtmlDecode(offset + category.Name)));

                if (categories.Any(c => c.ParentCategoryId == category.CategoryId))
                {
                    LoadAllCategories(categories, list, category.CategoryId, offset + "&nbsp;&nbsp;");
                }
            }
        }
    }
}
