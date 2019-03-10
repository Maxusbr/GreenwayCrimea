
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AdvantShop.ExportImport
{
    [Serializable()]
    public class ExportFeedResellerOptions : ExportFeedSettings
    {
        [JsonProperty(PropertyName = "ResellerCode")]
        public string ResellerCode { get; set; }

        [JsonProperty(PropertyName = "RecomendedPriceMargin")]
        public float RecomendedPriceMargin { get; set; }

        [JsonProperty(PropertyName = "CsvEnconing")]
        public string CsvEnconing { get; set; }

        [JsonProperty(PropertyName = "CsvSeparator")]
        public string CsvSeparator { get; set; }

        [JsonProperty(PropertyName = "CsvColumSeparator")]
        public string CsvColumSeparator { get; set; }

        [JsonProperty(PropertyName = "CsvPropertySeparator")]
        public string CsvPropertySeparator { get; set; }

        [JsonProperty(PropertyName = "CsvExportNoInCategory")]
        public bool CsvExportNoInCategory { get; set; }

        [JsonProperty(PropertyName = "CsvCategorySort")]
        public bool CsvCategorySort { get; set; }

        [JsonProperty(PropertyName = "FieldMapping")]
        public List<ProductFields> FieldMapping { get; set; }

        [JsonProperty(PropertyName = "ModuleFieldMapping")]
        public List<CSVField> ModuleFieldMapping { get; set; }
    }
}
