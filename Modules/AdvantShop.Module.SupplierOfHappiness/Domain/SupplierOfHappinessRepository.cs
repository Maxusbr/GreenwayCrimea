using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.SupplierOfHappiness.Domain
{
    public class SupplierOfHappinessRepository
    {
        public static bool InstallModule()
        {

            if (ModulesRepository.IsExistsModuleTable("Module", "SupplierOfHappinessCategories"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                @"if (SELECT COUNT(*) FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_SupplierOfHappinessCategories_Category') = 0
					ALTER TABLE Module.SupplierOfHappinessCategories ADD CONSTRAINT FK_SupplierOfHappinessCategories_Category FOREIGN KEY ( AdvCategoryId ) REFERENCES Catalog.Category ( CategoryID ) 	ON UPDATE  NO ACTION ON DELETE SET NULL;",
                CommandType.Text);
                return true;
            }

            ModulesRepository.ModuleExecuteNonQuery(
                "CREATE TABLE Module.SupplierOfHappinessCategories(Category nvarchar(250) NOT NULL,SubCategory nvarchar(250) NOT NULL,AdvCategoryId int NULL) ON [PRIMARY];" +
                "ALTER TABLE Module.SupplierOfHappinessCategories ADD CONSTRAINT FK_SupplierOfHappinessCategories_Category FOREIGN KEY ( AdvCategoryId ) REFERENCES Catalog.Category ( CategoryID ) ON UPDATE  NO ACTION ON DELETE SET NULL;",
                CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(
               "UPDATE [Settings].[ModuleSettings] Set [ModuleName] = 'SupplierOfHappiness' WHERE [ModuleName] = 'supplierofhappiness'",
               CommandType.Text);

            return ModulesRepository.IsExistsModuleTable("Module", "SupplierOfHappinessCategories");
        }

        public static bool UpdateModule()
        {
            ModulesRepository.ModuleExecuteNonQuery(
               "UPDATE [Settings].[ModuleSettings] Set [ModuleName] = 'SupplierOfHappiness' WHERE [ModuleName] = 'supplierofhappiness'",
               CommandType.Text);

            return true;
        }

        public static List<SupplierOfHappinessCategory> GetCategories()
        {
            return ModulesRepository.ModuleExecuteReadList<SupplierOfHappinessCategory>(
                "Select * From [Module].[SupplierOfHappinessCategories] Order By Category Asc, SubCategory Asc",
                CommandType.Text,
                reader => new SupplierOfHappinessCategory
                {
                    Category = ModulesRepository.ConvertTo<string>(reader, "Category"),
                    SubCategory = ModulesRepository.ConvertTo<string>(reader, "SubCategory"),
                    AdvCategoryId = ModulesRepository.ConvertTo<int?>(reader, "AdvCategoryId")
                });
        }

        public static bool AddCategory(SupplierOfHappinessCategory sohCategory)
        {
            return ModulesRepository.ModuleExecuteScalar<bool>(
                "If (Select Count(SubCategory) From [Module].[SupplierOfHappinessCategories]  Where Category=@Category And SubCategory = @SubCategory) = 0  " +
                "Begin Insert Into [Module].[SupplierOfHappinessCategories] (Category, SubCategory, AdvCategoryId) VALUES (@Category, @SubCategory, @AdvCategoryId); Select 1 End " +
                "Else Select 0",
                CommandType.Text,
                new SqlParameter("@Category", sohCategory.Category),
                new SqlParameter("@SubCategory", sohCategory.SubCategory),
                new SqlParameter("@AdvCategoryId", sohCategory.AdvCategoryId ?? (object)DBNull.Value));
        }

        public static void UpdateCategory(SupplierOfHappinessCategory sohCategory)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "Update [Module].[SupplierOfHappinessCategories] Set AdvCategoryId = @AdvCategoryId Where Category=@Category And SubCategory = @SubCategory",
                CommandType.Text,
                new SqlParameter("@Category", sohCategory.Category),
                new SqlParameter("@SubCategory", sohCategory.SubCategory),
                new SqlParameter("@AdvCategoryId", sohCategory.AdvCategoryId ?? (object)DBNull.Value));
        }
    }
}