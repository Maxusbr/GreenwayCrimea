//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.ExportImport;

namespace AdvantShop.Module.ProductTabs.Domain
{
    public class ProductTabsRepository
    {
        #region Module
        
        private const string _sqlScript =
                @"CREATE TABLE Module.TabTitle
	            (
	            TabTitleId int NOT NULL IDENTITY (1, 1),
	            Title nvarchar(150) NOT NULL,
                Active bit NOT NULL,
	            SortOrder int NOT NULL
	            )  ON [PRIMARY]

            ALTER TABLE Module.TabTitle ADD CONSTRAINT
	            PK_TabTitle PRIMARY KEY CLUSTERED 
	            (
	            TabTitleId
	            ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

            CREATE TABLE Module.TabBody
	            (
	            TabBodyId int NOT NULL IDENTITY (1, 1),
	            TabTitleId int NOT NULL,
                ProductId int NOT NULL,
	            Body nvarchar(MAX) NOT NULL
	            )  ON [PRIMARY]
	             TEXTIMAGE_ON [PRIMARY]

            ALTER TABLE Module.TabBody ADD CONSTRAINT
	            PK_TabBody PRIMARY KEY CLUSTERED 
	            (
	            TabBodyId
	            ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

            ALTER TABLE Module.TabBody ADD CONSTRAINT
	            FK_TabBody_TabTitle FOREIGN KEY
	            (
	            TabTitleId
	            ) REFERENCES Module.TabTitle
	            (
	            TabTitleId
	            ) ON UPDATE  NO ACTION 
	             ON DELETE  CASCADE";

        public static bool InstallDetailsCommonTabs()
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "TabTitle") &&
                !ModulesRepository.IsExistsModuleTable("Module", "TabBody"))
            {
                ModulesRepository.ModuleExecuteNonQuery(_sqlScript, CommandType.Text);
            }
            else
            {
                //todo: update script
            }

            return ModulesRepository.IsExistsModuleTable("Module", "TabTitle") &&
                   ModulesRepository.IsExistsModuleTable("Module", "TabBody");
        }

        public bool UnInstallProductTabs()
        {
            return true;
        }

        public bool UpdateProductTabs()
        {
            return true;
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static List<ITab> GetTabsByProductId(int productId)
        {
            return ModulesRepository.ModuleExecuteReadList<ITab>(
                //@"SELECT [TabBody].[TabTitleId],[TabBodyId],[Title],[Body],[ProductId],[SortOrder],[Active] 
                //    FROM [Module].[TabTitle] 
                //    INNER JOIN [Module].[TabBody] ON [TabTitle].[TabTitleId] = [TabBody].[TabTitleId] 
                //    WHERE ([ProductId] = @ProductId OR [ProductId] = 0) AND [Active] = 1 Order By [SortOrder], [ProductId] desc",
                @"
declare @TabBodyId int;
declare @Title nvarchar(150);

declare @tempTable table 
(tabTitleId int,tabBodyId int,title nvarchar(150),body nvarchar(max),productId int,sortOrder int,active bit )

Declare mycursor CURSOR  FOR 
SELECT [TabBodyId] ,[Title]
FROM [Module].[TabTitle] 
	INNER JOIN [Module].[TabBody] ON [TabTitle].[TabTitleId] = [TabBody].[TabTitleId] 
WHERE ([ProductId] = @ProductId OR [ProductId] = 0) AND [Active] = 1 Order By [ProductId] desc

open mycursor

	FETCH NEXT FROM mycursor INTO @TabBodyId, @Title
	WHILE @@FETCH_STATUS = 0
	BEGIN
		if (Select Count (*) from @tempTable Where Title = @Title) = 0
		Begin 
			Insert Into @tempTable (tabTitleId,tabBodyId,title,body,productId,sortOrder,active)
				(Select [TabBody].[TabTitleId],[TabBodyId],[Title],[Body],[ProductId],[SortOrder],[Active] 
				FROM [Module].[TabTitle] 
					INNER JOIN [Module].[TabBody] ON [TabTitle].[TabTitleId] = [TabBody].[TabTitleId] 
				WHERE ([ProductId] = @ProductId OR [ProductId] = 0) AND [Active] = 1 And [TabBodyId] = @TabBodyId )
		End
		FETCH NEXT FROM mycursor INTO @TabBodyId, @Title
	END

CLOSE mycursor
DEALLOCATE mycursor

Select * from @tempTable Order By [SortOrder]",

                CommandType.Text,
                reader => new ProductTab
                    {
                        TabTitleId = ModulesRepository.ConvertTo<int>(reader, "TabTitleId"),
                        TabBodyId = ModulesRepository.ConvertTo<int>(reader, "TabBodyId"),
                        ProductId = ModulesRepository.ConvertTo<int>(reader, "ProductId"),
                        Body = ModulesRepository.ConvertTo<string>(reader, "Body"),
                        Title = ModulesRepository.ConvertTo<string>(reader, "Title"),
                        SortOrder = ModulesRepository.ConvertTo<int>(reader, "SortOrder"),
                        Active = ModulesRepository.ConvertTo<bool>(reader, "Active")
                    },
                new SqlParameter("@ProductId", productId));
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static List<ITab> GetTabsWithoutCommonByProductId(int productId)
        {
            return ModulesRepository.ModuleExecuteReadList<ITab>(
                @"SELECT [TabTitle].[TabTitleId],[TabBodyId],[Title],[Body],[ProductId],[SortOrder],[Active] 
                    FROM [Module].[TabTitle] Left JOIN [Module].[TabBody] ON [TabTitle].[TabTitleId] = [TabBody].[TabTitleId] AND ([ProductId] = @ProductId)
                    WHERE [TabTitle].[TabTitleId] not in (Select [TabTitleId] From [Module].[TabBody] Where [ProductId] = 0)
                    Order By [SortOrder]",
                CommandType.Text,
                reader => new ProductTab
                    {
                        TabTitleId = ModulesRepository.ConvertTo<int>(reader, "TabTitleId"),
                        TabBodyId = ModulesRepository.ConvertTo<int>(reader, "TabBodyId"),
                        ProductId = ModulesRepository.ConvertTo<int>(reader, "ProductId"),
                        Body = ModulesRepository.ConvertTo<string>(reader, "Body"),
                        Title = ModulesRepository.ConvertTo<string>(reader, "Title"),
                        SortOrder = ModulesRepository.ConvertTo<int>(reader, "SortOrder"),
                        Active = ModulesRepository.ConvertTo<bool>(reader, "Active")
                    },
                new SqlParameter("@ProductId", productId));
        }

        #region CommonTabs

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static List<ITab> GetCommonTabs()
        {
            return ModulesRepository.ModuleExecuteReadList<ITab>(
                @"SELECT [TabBody].[TabTitleId],[TabBodyId],[Title],[Body],[ProductId],[SortOrder],[Active] 
                  FROM [Module].[TabTitle] INNER JOIN [Module].[TabBody] ON [TabTitle].[TabTitleId] = [TabBody].[TabTitleId] 
                  WHERE [ProductId] = 0 
                  Order By [SortOrder]",
                CommandType.Text,
                reader => new ProductTab
                    {
                        TabTitleId = ModulesRepository.ConvertTo<int>(reader, "TabTitleId"),
                        TabBodyId = ModulesRepository.ConvertTo<int>(reader, "TabBodyId"),
                        ProductId = ModulesRepository.ConvertTo<int>(reader, "ProductId"),
                        Body = ModulesRepository.ConvertTo<string>(reader, "Body"),
                        Title = ModulesRepository.ConvertTo<string>(reader, "Title"),
                        SortOrder = ModulesRepository.ConvertTo<int>(reader, "SortOrder"),
                        Active = ModulesRepository.ConvertTo<bool>(reader, "Active")
                    });
        }

        public static ProductTab GetProductCommonTab(int tabTitleId, int tabBodyId)
        {
            return ModulesRepository.ModuleExecuteReadOne(
                @"SELECT [TabBody].[TabTitleId],[TabBodyId],[Title],[Body],[ProductId],[SortOrder],[Active] 
                  FROM [Module].[TabTitle] INNER JOIN [Module].[TabBody] ON [TabTitle].[TabTitleId] = [TabBody].[TabTitleId] 
                  WHERE [TabBody].[TabTitleId] = @TabTitleId AND [TabBodyId] = @TabBodyId AND [ProductId] = 0 
                  Order By [SortOrder]",
                CommandType.Text,
                reader => new ProductTab
                    {
                        TabTitleId = ModulesRepository.ConvertTo<int>(reader, "TabTitleId"),
                        TabBodyId = ModulesRepository.ConvertTo<int>(reader, "TabBodyId"),
                        ProductId = ModulesRepository.ConvertTo<int>(reader, "ProductId"),
                        Body = ModulesRepository.ConvertTo<string>(reader, "Body"),
                        Title = ModulesRepository.ConvertTo<string>(reader, "Title"),
                        SortOrder = ModulesRepository.ConvertTo<int>(reader, "SortOrder"),
                        Active = ModulesRepository.ConvertTo<bool>(reader, "Active")
                    },
                new SqlParameter("@TabTitleId", tabTitleId),
                new SqlParameter("@TabBodyId", tabBodyId));
        }

        #endregion

        #region ProductTabTitle

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static List<TabTitle> GetProductTabTitles()
        {
            return ModulesRepository.ModuleExecuteReadList(
                "SELECT * FROM [Module].[TabTitle] WHERE TabTitleId NOT IN (Select TabTitleId FROM [Module].[TabBody] WHERE [ProductId] = 0) ORDER BY [SortOrder]",
                CommandType.Text,
                reader => new TabTitle
                    {
                        TabTitleId = ModulesRepository.ConvertTo<int>(reader, "TabTitleId"),
                        Title = ModulesRepository.ConvertTo<string>(reader, "Title"),
                        SortOrder = ModulesRepository.ConvertTo<int>(reader, "SortOrder"),
                        Active = ModulesRepository.ConvertTo<bool>(reader, "Active")
                    });
        }

        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static TabTitle GetProductTabTitle(string title)
        {
            return ModulesRepository.ModuleExecuteReadOne(
                "SELECT * FROM [Module].[TabTitle] WHERE [Title] = @Title",
                CommandType.Text,
                reader => new TabTitle
                    {
                        TabTitleId = ModulesRepository.ConvertTo<int>(reader, "TabTitleId"),
                        Title = ModulesRepository.ConvertTo<string>(reader, "Title"),
                        SortOrder = ModulesRepository.ConvertTo<int>(reader, "SortOrder"),
                        Active = ModulesRepository.ConvertTo<bool>(reader, "Active")
                    },
                new SqlParameter("@Title", title));
        }

        /// <summary>
        /// </summary>
        /// <param name="tabTitleId"></param>
        /// <returns></returns>
        public static TabTitle GetProductTabTitle(int tabTitleId)
        {
            return ModulesRepository.ModuleExecuteReadOne(
                "SELECT * FROM [Module].[TabTitle] WHERE [TabTitleId] = @TabTitleId",
                CommandType.Text,
                reader => new TabTitle
                    {
                        TabTitleId = ModulesRepository.ConvertTo<int>(reader, "TabTitleId"),
                        Title = ModulesRepository.ConvertTo<string>(reader, "Title"),
                        SortOrder = ModulesRepository.ConvertTo<int>(reader, "SortOrder"),
                        Active = ModulesRepository.ConvertTo<bool>(reader, "Active")
                    },
                new SqlParameter("@TabTitleId", tabTitleId));
        }

        /// <summary>
        /// </summary>
        /// <param name="tabTitle"></param>
        public static void UpdateProductTabTitle(TabTitle tabTitle)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "UPDATE [Module].[TabTitle] SET [Title] = @Title, [SortOrder] = @SortOrder, [Active] = @Active WHERE [TabTitleId] = @TabTitleId",
                CommandType.Text,
                new SqlParameter("@Title", tabTitle.Title),
                new SqlParameter("@SortOrder", tabTitle.SortOrder),
                new SqlParameter("@TabTitleId", tabTitle.TabTitleId),
                new SqlParameter("@Active", tabTitle.Active));
        }

        /// <summary>
        /// </summary>
        /// <param name="tab"></param>
        public static int AddProductTabTitle(TabTitle tab)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(
                "INSERT INTO [Module].[TabTitle] ([Title],[SortOrder],[Active]) VALUES (@Title,@SortOrder,@Active); Select scope_identity();",
                CommandType.Text,
                new SqlParameter("@Title", tab.Title),
                new SqlParameter("@SortOrder", tab.SortOrder),
                new SqlParameter("@Active", tab.Active));
        }

        /// <summary>
        /// </summary>
        /// <param name="tabTitleId"></param>
        public static void DeleteProductTabTitle(int tabTitleId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Module].[TabTitle] WHERE [TabTitleId] = @TabTitleId",
                CommandType.Text,
                new SqlParameter("@TabTitleId", tabTitleId));
        }

        #endregion

        #region ProductTabBody

        /// <summary>
        /// </summary>
        /// <param name="tabBodyId"></param>
        /// <returns></returns>
        public static TabBody GetProductTabBody(int tabBodyId)
        {
            return ModulesRepository.ModuleExecuteReadOne(
                @"Select * FROM [Module].[TabBody] WHERE [TabBodyId] = @TabBodyId",
                CommandType.Text,
                reader => new TabBody
                    {
                        TabTitleId = ModulesRepository.ConvertTo<int>(reader, "TabTitleId"),
                        TabBodyId = ModulesRepository.ConvertTo<int>(reader, "TabBodyId"),
                        ProductId = ModulesRepository.ConvertTo<int>(reader, "ProductId"),
                        Body = ModulesRepository.ConvertTo<string>(reader, "Body")
                    },
                new SqlParameter("@TabBodyId", tabBodyId));
        }

        public static TabBody GetProductTabBody(int tabTitleId, int productId)
        {
            return ModulesRepository.ModuleExecuteReadOne(
                @"Select * FROM [Module].[TabBody] WHERE [TabTitleId] = @TabTitleId AND [ProductId] = @ProductId",
                CommandType.Text,
                reader => new TabBody
                    {
                        TabTitleId = ModulesRepository.ConvertTo<int>(reader, "TabTitleId"),
                        TabBodyId = ModulesRepository.ConvertTo<int>(reader, "TabBodyId"),
                        ProductId = ModulesRepository.ConvertTo<int>(reader, "ProductId"),
                        Body = ModulesRepository.ConvertTo<string>(reader, "Body")
                    },
                new SqlParameter("@TabTitleId", tabTitleId),
                new SqlParameter("@ProductId", productId));
        }


        /// <summary>
        /// </summary>
        /// <param name="tab"></param>
        public static void AddUpdateProductTabBody(TabBody tab)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                @"IF(Select Count(TabBodyId) FROM [Module].[TabBody] WHERE [TabTitleId] = @TabTitleId AND [ProductId] = @ProductId) > 0
	                    BEGIN
		                    UPDATE [Module].[TabBody] SET [Body] = @Body WHERE [TabTitleId] = @TabTitleId AND[ProductId] = @ProductId
	                    END
                    ELSE
	                    BEGIN
		                    INSERT INTO [Module].[TabBody] ([TabTitleId],[Body],[ProductId]) VALUES (@TabTitleId,@Body,@ProductId)
	                    END",
                CommandType.Text,
                new SqlParameter("@TabTitleId", tab.TabTitleId),
                new SqlParameter("@Body", tab.Body),
                new SqlParameter("@ProductId", tab.ProductId));
        }

        /// <summary>
        /// </summary>
        /// <param name="tabBodyId"></param>
        public static void DeleteProductTabBody(int tabBodyId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Module].[TabBody] WHERE [TabBodyId] = @TabBodyId",
                CommandType.Text,
                new SqlParameter("@TabBodyId", tabBodyId));
        }

        /// <summary>
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="tabTitleId"></param>
        public static void DeleteProductTabBody(int productId, int tabTitleId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Module].[TabBody] WHERE [ProductId] = @ProductId AND [TabTitleId] = @TabTitleId",
                CommandType.Text,
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@TabTitleId", tabTitleId));
        }

        #endregion

        #region CSV

        /// <summary>
        /// Prepare CSVField
        /// </summary>
        /// <param name="field">Field ObjId in CSVField is inited by TabTitleId</param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static string PrepareCSVField(CSVField field, int productId)
        {
            var tabBody = GetProductTabBody(field.ObjId, productId);
            return tabBody != null ? tabBody.Body : string.Empty;
        }

        /// <summary>
        /// Process CSVField
        /// </summary>
        /// <param name="field">Field ObjId in CSVField is inited by TabTitleId</param>
        /// <param name="productId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ProcessCSVField(CSVField field, int productId, string value)
        {
            if (string.IsNullOrEmpty(value))
                DeleteProductTabBody(productId, field.ObjId);
            else
                AddUpdateProductTabBody(new TabBody
                {
                    Body = value,
                    ProductId = productId,
                    TabTitleId = field.ObjId
                });
            return true;
        }

        #endregion
    }
}