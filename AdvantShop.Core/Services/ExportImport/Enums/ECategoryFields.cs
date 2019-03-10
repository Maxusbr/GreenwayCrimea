using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.ExportImport
{
    public enum CategoryFields
    {
        [StringName("none")]
        [Localize("Core.ExportImport.CategoryFields.NotSelected")]
        [CsvFieldsStatus(CsvFieldStatus.None)]
        None,

        [StringName("categoryid")]
        [Localize("Core.ExportImport.CategoryFields.CategoryId")]
        [CsvFieldsStatus(CsvFieldStatus.Int)]
        CategoryId,

        [StringName("name")]
        [Localize("Core.ExportImport.CategoryFields.Name")]
        [CsvFieldsStatus(CsvFieldStatus.StringRequired)]
        Name,

        [StringName("slug")]
        [Localize("Core.ExportImport.CategoryFields.Slug")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Slug,

        [StringName("parentcategory")]
        [Localize("Core.ExportImport.CategoryFields.ParentCategory")]
        [CsvFieldsStatus(CsvFieldStatus.Int)]
        ParentCategory,

        [StringName("sortorder")]
        [Localize("Core.ExportImport.CategoryFields.SortOrder")]
        [CsvFieldsStatus(CsvFieldStatus.Int)]
        SortOrder,

        [StringName("enabled")]
        [Localize("Core.ExportImport.CategoryFields.Enabled")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Enabled,

        [StringName("hidden")]
        [Localize("Core.ExportImport.CategoryFields.Hidden")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Hidden,

        [StringName("briefdescription")]
        [Localize("Core.ExportImport.CategoryFields.BriefDescription")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        BriefDescription,

        [StringName("description")]
        [Localize("Core.ExportImport.CategoryFields.Description")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Description,



        [StringName("displaystyle")]
        [Localize("Core.ExportImport.CategoryFields.DisplayStyle")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        DisplayStyle,

        [StringName("sorting")]
        [Localize("Core.ExportImport.CategoryFields.Sorting")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Sorting,

        [StringName("displaybrandsinmenu")]
        [Localize("Core.ExportImport.CategoryFields.DisplayBrandsInMenu")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        DisplayBrandsInMenu,

        [StringName("displaysubcategoriesinmenu")]
        [Localize("Core.ExportImport.CategoryFields.DisplaySubCategoriesInMenu")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        DisplaySubCategoriesInMenu,

        [StringName("tags")]
        [Localize("Core.ExportImport.CategoryFields.Tags")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Tags,



        [StringName("picture")]
        [Localize("Core.ExportImport.CategoryFields.Picture")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Picture,

        [StringName("minipicture")]
        [Localize("Core.ExportImport.CategoryFields.MiniPicture")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        MiniPicture,

        [StringName("icon")]
        [Localize("Core.ExportImport.CategoryFields.Icon")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Icon,



        [StringName("title")]
        [Localize("Core.ExportImport.CategoryFields.SeoTitle")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Title,

        [StringName("metakeywords")]
        [Localize("Core.ExportImport.CategoryFields.MetaKeywords")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        MetaKeywords,

        [StringName("metadescription")]
        [Localize("Core.ExportImport.CategoryFields.MetaDescription")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        MetaDescription,

        [StringName("h1")]
        [Localize("Core.ExportImport.CategoryFields.H1")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        H1,



        [StringName("propertygroups")]
        [Localize("Core.ExportImport.CategoryFields.PropertyGroups")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        PropertyGroups,
    }
}
