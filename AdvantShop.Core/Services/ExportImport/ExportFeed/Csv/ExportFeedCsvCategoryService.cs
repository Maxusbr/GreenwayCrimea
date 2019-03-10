using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.SEO;

namespace AdvantShop.ExportImport
{
    public class ExportFeedCsvCategoryService
    {
        private const string Separator = ";";

        public static ExportFeedCsvCategory GetCsvCategoriesFromReader(SqlDataReader reader, List<CategoryFields> fieldMapping)
        {
            var category = CategoryService.GetCategoryFromReader(reader);

            var categoryCsv = new ExportFeedCsvCategory()
            {
                CategoryId = category.CategoryId.ToString(),
                Name = category.Name,
                Slug = category.UrlPath,
                ParentCategory = category.ParentCategoryId.ToString(),
                SortOrder = category.SortOrder.ToString(),
                Enabled = category.Enabled ? "+" : "-",
                Hidden = category.Hidden ? "+" : "-",
                BriefDescription = category.BriefDescription,
                Description = category.Description,
                DisplayStyle = category.DisplayStyle.ToString(),
                Sorting = category.Sorting.ToString(),
                DisplayBrandsInMenu = category.DisplayBrandsInMenu ? "+" : "-",
                DisplaySubCategoriesInMenu = category.DisplaySubCategoriesInMenu ? "+" : "-",
            };

            if (fieldMapping.Contains(CategoryFields.Tags))
            {
                categoryCsv.Tags = String.Join(Separator, category.Tags.Select(x => x.Name));
            }

            if (fieldMapping.Contains(CategoryFields.Picture))
            {
                categoryCsv.Picture = category.Picture.PhotoName;
            }

            if (fieldMapping.Contains(CategoryFields.MiniPicture))
            {
                categoryCsv.MiniPicture = category.MiniPicture.PhotoName;
            }

            if (fieldMapping.Contains(CategoryFields.Icon))
            {
                categoryCsv.Icon = category.Icon.PhotoName;
            }
            
            if (fieldMapping.Contains(CategoryFields.Title) ||
                fieldMapping.Contains(CategoryFields.H1) ||
                fieldMapping.Contains(CategoryFields.MetaKeywords) ||
                fieldMapping.Contains(CategoryFields.MetaDescription))
            {
                var meta = MetaInfoService.GetMetaInfo(category.CategoryId, MetaType.Category) ??
                           new MetaInfo(0, 0, MetaType.Category, string.Empty, string.Empty, string.Empty, string.Empty);

                categoryCsv.Title = meta.Title;
                categoryCsv.H1 = meta.H1;
                categoryCsv.MetaKeywords = meta.MetaKeywords;
                categoryCsv.MetaDescription = meta.MetaDescription;
            }

            if (fieldMapping.Contains(CategoryFields.PropertyGroups))
            {
                var groups = PropertyGroupService.GetListByCategory(category.CategoryId);
                categoryCsv.PropertyGroups = String.Join(Separator, groups.Select(x => x.Name));
            }

            return categoryCsv;
        }

        public static IEnumerable<ExportFeedCsvCategory> GetCsvCategories(List<CategoryFields> fieldMapping)
        {
            return SQLDataAccess.ExecuteReadIEnumerable(
                   "Select * From [Catalog].[Category]", //  Where CategoryId <> 0
                    CommandType.Text,
                    reader => GetCsvCategoriesFromReader(reader, fieldMapping));
        }

        public static int GetCsvCategoriesCount()
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "Select Count(CategoryId) From [Catalog].[Category]",
                CommandType.Text);
        }
    }
}