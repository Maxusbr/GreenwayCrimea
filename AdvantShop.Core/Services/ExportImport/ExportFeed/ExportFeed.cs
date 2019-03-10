//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


using System;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.ExportImport
{
    public enum EExportFeedType
    {
        None,
        [Localize("Core.ExportImport.ExportFeed.YandexType")]
        YandexMarket,
        [Localize("Core.ExportImport.ExportFeed.GoogleType")]
        GoogleMerchentCenter,
        [Localize("Core.ExportImport.ExportFeed.CsvType")]
        Csv,
        [Localize("Core.ExportImport.ExportFeed.ResellerType")]
        Reseller
    }

    public enum EncodingsEnum
    {
        [StringName("Windows-1251")]
        Windows1251,
        [StringName("UTF-8")]
        Utf8,
        [StringName("UTF-16")]
        Utf16,
        [StringName("KOI8-R")]
        Koi8R
    }

    public enum SeparatorsEnum
    {
        [StringName(",")]
        [Localize("Core.ExportImport.Separarator.Comma")]
        CommaSeparated,
        [StringName("\t")]
        [Localize("Core.ExportImport.Separarator.Tab")]
        TabSeparated,
        [StringName(";")]
        [Localize("Core.ExportImport.Separarator.Semicolon")]
        SemicolonSeparated,
        [StringName("")]
        [Localize("Core.ExportImport.Separarator.Custom")]
        Custom
    }

    public class ExportFeed
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public EExportFeedType Type { get; set; }

        public string Description { get; set; }
        
        public DateTime? LastExport { get; set; }

        public string LastExportFileFullName { get; set; }
    }
}