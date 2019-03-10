//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.ExportImport
{
    public enum ProductFields
    {
        [StringName("none")]
        [Localize("Core.ExportImport.ProductFields.NotSelected")]        
        [CsvFieldsStatus(CsvFieldStatus.None)]
        None,

        [StringName("sku")]
        [Localize("Core.ExportImport.ProductFields.Sku")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Sku,

        [StringName("name")]
        [Localize("Core.ExportImport.ProductFields.Name")]
        [CsvFieldsStatus(CsvFieldStatus.StringRequired)]
        Name,

        [StringName("paramsynonym")]
        [Localize("Core.ExportImport.ProductFields.Synonym")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        ParamSynonym,

        [StringName("category")]
        [Localize("Core.ExportImport.ProductFields.Categories")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Category,

        [StringName("sorting")]
        [Localize("Core.ExportImport.ProductFields.Sorting")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Sorting,

        [StringName("enabled")]
        [Localize("Core.ExportImport.ProductFields.Enabled")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Enabled,

        [StringName("currency")]
        [Localize("Core.ExportImport.ProductFields.Currency")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Currency,

        [StringName("price")]
        [Localize("Core.ExportImport.ProductFields.Price")]
        [CsvFieldsStatus(CsvFieldStatus.Float)]
        Price,

        [StringName("purchaseprice")]
        [Localize("Core.ExportImport.ProductFields.PurchasePrice")]
        [CsvFieldsStatus(CsvFieldStatus.Float)]
        PurchasePrice,

        [StringName("amount")]
        [Localize("Core.ExportImport.ProductFields.Amount")]
        [CsvFieldsStatus(CsvFieldStatus.Float)]
        Amount,

        [StringName("sku:size:color:price:purchaseprice:amount")]
        [Localize("Core.ExportImport.ProductFields.MultiOffer")]
        [CsvFieldsStatus(CsvFieldStatus.NotEmptyString)]
        MultiOffer,

        [StringName("unit")]
        [Localize("Core.ExportImport.ProductFields.Unit")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Unit,

        [StringName("discount")]
        [Localize("Core.ExportImport.ProductFields.Discount")]
        [CsvFieldsStatus(CsvFieldStatus.Float)]
        Discount,

        [StringName("discountamount")]
        [Localize("Core.ExportImport.ProductFields.DiscountAmount")]
        [CsvFieldsStatus(CsvFieldStatus.Float)]
        DiscountAmount,

        [StringName("shippingprice")]
        [Localize("Core.ExportImport.ProductFields.ShippingPrice")]
        [CsvFieldsStatus(CsvFieldStatus.NullableFloat)]
        ShippingPrice,

        [StringName("weight")]
        [Localize("Core.ExportImport.ProductFields.Weight")]
        [CsvFieldsStatus(CsvFieldStatus.Float)]
        Weight,

        [StringName("size")]
        [Localize("Core.ExportImport.ProductFields.Size")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Size,

        [StringName("briefdescription")]
        [Localize("Core.ExportImport.ProductFields.BriefDescription")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        BriefDescription,

        [StringName("description")]
        [Localize("Core.ExportImport.ProductFields.Description")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Description,

        [StringName("title")]
        [Localize("Core.ExportImport.ProductFields.SeoTitle")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Title,

        [StringName("metakeywords")]
        [Localize("Core.ExportImport.ProductFields.MetaKeywords")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        MetaKeywords,

        [StringName("metadescription")]
        [Localize("Core.ExportImport.ProductFields.MetaDescription")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        MetaDescription,

        [StringName("h1")]
        [Localize("Core.ExportImport.ProductFields.H1")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        H1,
        
        [StringName("photos")]
        [Localize("Core.ExportImport.ProductFields.Photos")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Photos,

        [StringName("videos")]
        [Localize("Core.ExportImport.ProductFields.Videos")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Videos,

        [StringName("markers")]
        [Localize("Core.ExportImport.ProductFields.Markers")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Markers,

        [StringName("properties")]
        [Localize("Core.ExportImport.ProductFields.Properties")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Properties,

        [StringName("producer")]
        [Localize("Core.ExportImport.ProductFields.Producer")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Producer,

        [StringName("preorder")]
        [Localize("Core.ExportImport.ProductFields.PreOrder")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        OrderByRequest,

        [StringName("salesnote")]
        [Localize("Core.ExportImport.ProductFields.SalesNotes")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        SalesNotes,


        [StringName("related sku")]
        [Localize("Core.ExportImport.ProductFields.RelatedSKU")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Related,

        [StringName("alternative sku")]
        [Localize("Core.ExportImport.ProductFields.AlternativeSKU")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Alternative,

        [StringName("custom options")]
        [Localize("Core.ExportImport.ProductFields.CustomOptions")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        CustomOption,

        [StringName("gtin")]
        [Localize("Core.ExportImport.ProductFields.Gtin")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Gtin,

        [StringName("googleproductcategory")]
        [Localize("Core.ExportImport.ProductFields.GoogleProductCategory")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        GoogleProductCategory,
        
        [StringName("yandexproductcategory")]
        [Localize("Core.ExportImport.ProductFields.YandexProductCategory")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        YandexProductCategory,
        
        [StringName("yandextypeprefix")]
        [Localize("Core.ExportImport.ProductFields.YandexTypePrefix")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        YandexTypePrefix,

        [StringName("yandexname")]
        [Localize("Core.ExportImport.ProductFields.YandexName")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        YandexName,
        
        [StringName("yandexmodel")]
        [Localize("Core.ExportImport.ProductFields.YandexModel")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        YandexModel,

        [StringName("adult")]
        [Localize("Core.ExportImport.ProductFields.Adult")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Adult,

        [StringName("manufacturer_warranty")]
        [Localize("Core.ExportImport.ProductFields.ManufacturerWarranty")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        ManufacturerWarranty,

        [StringName("tags")]
        [Localize("Core.ExportImport.ProductFields.Tags")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Tags,

        [StringName("gifts")]
        [Localize("Core.ExportImport.ProductFields.Gifts")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Gifts,

        [StringName("minamount")]
        [Localize("Core.ExportImport.ProductFields.Minamount")]
        [CsvFieldsStatus(CsvFieldStatus.Float)]
        MinAmount,

        [StringName("maxamount")]
        [Localize("Core.ExportImport.ProductFields.Maxamount")]
        [CsvFieldsStatus(CsvFieldStatus.Float)]
        MaxAmount,

        [StringName("multiplicity")]
        [Localize("Core.ExportImport.ProductFields.Multiplicity")]
        [CsvFieldsStatus(CsvFieldStatus.Float)]
        Multiplicity,

        [StringName("cbid")]
        [Localize("Core.ExportImport.ProductFields.Cbid")]
        [CsvFieldsStatus(CsvFieldStatus.Float)]
        Cbid,

        [StringName("fee")]
        [Localize("Core.ExportImport.ProductFields.Fee")]
        [CsvFieldsStatus(CsvFieldStatus.Float)]
        Fee,

        [StringName("barcode")]
        [Localize("Core.ExportImport.ProductFields.BarCode")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        BarCode,

        [StringName("tax")]
        [Localize("Core.ExportImport.ProductFields.Tax")]
        [CsvFieldsStatus(CsvFieldStatus.String)]
        Tax
    }
}