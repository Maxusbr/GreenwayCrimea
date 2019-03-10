using System.Data;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.VkMarket.Services
{
    public static class InstallUpdateModuleService
    {
        public static bool Install()
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "VkCategory"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE [Module].[VkCategory](
	                    [Id] [int] IDENTITY(1,1) NOT NULL,
	                    [VkId] [bigint] NOT NULL,
	                    [VkCategoryId] [bigint] NOT NULL,
	                    [Name] [nvarchar](max) NOT NULL,
	                    [SortOrder] [int] NOT NULL,
                        CONSTRAINT [PK_VkCategory] PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]", 
                    CommandType.Text);
            }

            if (!ModulesRepository.IsExistsModuleTable("Module", "VkCategory_Category"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                  @"CREATE TABLE [Module].[VkCategory_Category](
	                    [CategoryId] [int] NOT NULL,
	                    [VkCategoryId] [int] NOT NULL,
                     CONSTRAINT [PK_VkCategory_Category] PRIMARY KEY CLUSTERED 
                    (
	                    [CategoryId] ASC,
	                    [VkCategoryId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    ALTER TABLE [Module].[VkCategory_Category]  WITH CHECK ADD  CONSTRAINT [FK_VkCategory_Category_Category] FOREIGN KEY([CategoryId])
                    REFERENCES [Catalog].[Category] ([CategoryID])
                    ON DELETE CASCADE

                    ALTER TABLE [Module].[VkCategory_Category]  WITH CHECK ADD  CONSTRAINT [FK_VkCategory_Category_VkCategory] FOREIGN KEY([VkCategoryId])
                    REFERENCES [Module].[VkCategory] ([Id])
                    ON DELETE CASCADE

                    ALTER TABLE [Module].[VkCategory_Category] CHECK CONSTRAINT [FK_VkCategory_Category_VkCategory]",
                    CommandType.Text);
            }

            if (!ModulesRepository.IsExistsModuleTable("Module", "VkProduct"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE [Module].[VkProduct](
	                    [Id] [bigint] NOT NULL,
	                    [ProductId] [int] NOT NULL,
	                    [MainPhotoId] [bigint] NOT NULL,
	                    [PhotoIds] [nvarchar](max) NOT NULL,
	                    [AlbumId] [bigint] NOT NULL,
                     CONSTRAINT [PK_VkProduct] PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC,
	                    [ProductId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                    
                    ALTER TABLE Module.VkProduct ADD CONSTRAINT FK_VkProduct_Product FOREIGN KEY (ProductId) 
                    REFERENCES Catalog.Product (ProductId) 
                    ON UPDATE  NO ACTION 
                    ON DELETE  CASCADE
                    ",
                    CommandType.Text);
            }


            return true;
        }

        public static bool Delete()
        {
            ModuleSettingsProvider.RemoveSqlSettings(VkMarket.ModuleId);

            return true;
        }
    }
}
